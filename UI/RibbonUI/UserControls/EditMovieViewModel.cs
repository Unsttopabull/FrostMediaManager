using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.GettextMarkupExtension;
using Frost.RibbonUI.Design;
using Frost.RibbonUI.Design.Fakes;
using Frost.RibbonUI.Design.Models;
using Frost.RibbonUI.Properties;
using Frost.RibbonUI.Util;
using Frost.RibbonUI.Util.ObservableWrappers;
using Frost.RibbonUI.Windows;
using Frost.RibbonUI.Windows.Add;
using Frost.RibbonUI.Windows.Edit;
using Frost.XamlControls.Commands;
using Microsoft.Win32;

namespace Frost.RibbonUI.UserControls {

    public class EditMovieViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IMoviesDataService _service;
        private ObservableMovie _selectedMovie;
        private ObservableCollection<IMovieSet> _sets;
        private MoviePlot _selectedPlot;

        public EditMovieViewModel() {
            _service = Gettext.IsInDesignMode
                ? new DesignMoviesDataService()
                : LightInjectContainer.GetInstance<IMoviesDataService>();

            if (Gettext.IsInDesignMode) {
                SelectedMovie = new ObservableMovie(new FakeMovie());
            }

            IEnumerable<IMovieSet> sets = _service.Sets;
            Sets = sets != null
                       ? new ObservableCollection<IMovieSet>(_service.Sets)
                       : new ObservableCollection<IMovieSet>();

            RegisterCommands();
        }

        public Window ParentWindow { get; set; }

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                OnPropertyChanged();
            }
        }

        public IMovieSet SelectedMovieSet {
            get {
                if (SelectedMovie != null) {
                    return SelectedMovie.Set;
                }
                return null;
            }
            set { SelectedMovie.Set = value; }
        }

        public MoviePlot SelectedPlot {
            get { return _selectedPlot; }
            set {
                if (Equals(value, _selectedPlot)) {
                    return;
                }
                _selectedPlot = value;
                OnPropertyChanged();
            }
        }

        public bool IsAddingPlot { get; set; }

        #region Collections

        public ObservableCollection<IMovieSet> Sets {
            get { return _sets; }
            set {
                if (Equals(value, _sets)) {
                    return;
                }
                _sets = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ICommands

        public ICommand AddStudioCommand { get; private set; }
        public RelayCommand<MovieStudio> RemoveStudioCommand { get; private set; }
        public RelayCommand<MovieStudio> EditStudioCommand { get; private set; }

        public RelayCommand<IGenre> AddMovieGenreCommand { get; private set; }
        public RelayCommand<IGenre> RemoveMovieGenreCommand { get; private set; }

        public RelayCommand AddGenreCommand { get; private set; }
        public RelayCommand<IGenre> EditGenreCommand { get; private set; }

        public ICommand AddActorCommand { get; private set; }
        public ICommand<MovieActor> RemoveActorCommand { get; private set; }
        public ICommand<MovieActor> EditActorCommand { get; private set; }

        public ICommand<MoviePlot> SetPlotLanguageCommand { get; private set; }
        public ICommand AddPlotCommand { get; private set; } 
        public ICommand<MoviePlot> RemovePlotCommand { get; private set; }

        public ICommand AddCountryCommand { get; private set; }
        public RelayCommand<MovieCountry> RemoveCountryCommand { get; private set; }

        public ICommand AddDirectorCommand { get; private set; }
        public RelayCommand<MoviePerson> RemoveDirectorCommand { get; private set; }
        public RelayCommand<MoviePerson> EditDirectorCommand { get; private set; }

        public ICommand TrailerSearchCommand { get; private set; }

        public ICommand SetPlotAsDefault { get; private set; }

        private void RegisterCommands() {
            SetPlotAsDefault = new RelayCommand<MoviePlot>(
                p => { SelectedMovie.MainPlot = p; },
                p => {
                    if (p == null || SelectedMovie == null) {
                        return false;
                    }

                    IPlot plot = SelectedMovie.ObservedEntity.MainPlot;
                    if (plot == null) {
                        return true;
                    }

                    if (plot.Id != p.ObservedEntity.Id) {
                        return true;
                    }
                    return false;
                });

            AddStudioCommand = new RelayCommand(AddStudioOnClick);
            RemoveStudioCommand = new RelayCommand<MovieStudio>(
                studio => SelectedMovie.RemoveStudio(studio),
                studio => studio != null
            );

            EditStudioCommand = new RelayCommand<MovieStudio>(EditStudioOnClick, studio => studio != null);

            AddMovieGenreCommand = new RelayCommand<IGenre>(
                genre => SelectedMovie.AddGenre(genre),
                genre => genre != null
            );

            RemoveMovieGenreCommand = new RelayCommand<IGenre>(
                genre => SelectedMovie.RemoveGenre(genre),
                genre => genre != null
            );

            AddGenreCommand = new RelayCommand(AddGenreClick);
            EditGenreCommand = new RelayCommand<IGenre>(EditGenreOnClick, genre => genre != null);

            AddActorCommand = new RelayCommand(AddActorOnClick);
            RemoveActorCommand = new RelayCommand<MovieActor>(
                actor => SelectedMovie.RemoveActor(actor),
                actor => actor != null
            );
            EditActorCommand = new RelayCommand<MovieActor>(
                actor => new EditPerson { Owner = ParentWindow, SelectedPerson = actor }.ShowDialog(),
                actor => actor != null
            );

            SetPlotLanguageCommand = new RelayCommand<MoviePlot>(SetPlotLanguageClick, plot => plot != null);
            AddPlotCommand = new RelayCommand(AddPlot, o => SelectedMovie != null && SelectedMovie["Plots"]);
            RemovePlotCommand = new RelayCommand<MoviePlot>(RemovePotOnClick, plot => SelectedMovie != null && plot != null && SelectedMovie["Plots"]);

            AddCountryCommand = new RelayCommand(AddCountryOnClick);
            RemoveCountryCommand = new RelayCommand<MovieCountry>(
                country => SelectedMovie.RemoveCountry(country),
                country => country != null
            );

            AddDirectorCommand = new RelayCommand(AddDirectorOnClick);
            RemoveDirectorCommand = new RelayCommand<MoviePerson>(
                director => SelectedMovie.RemoveDirector(director),
                director => director != null
            );

            EditDirectorCommand = new RelayCommand<MoviePerson>(
                director => new EditPerson { Owner = ParentWindow, SelectedPerson = director }.ShowDialog(),
                director => director != null
            );

            TrailerSearchCommand = new RelayCommand(TrailerSearchOnClick);
        }

        #endregion

        #region Country Handlers

        private void AddCountryOnClick() {
            IEnumerable<ICountry> countries = _service.Countries ?? UIHelper.GetCountries();

            AddCountries sc = new AddCountries {
                Owner = ParentWindow,
                Countries = new ObservableCollection<MovieCountry>(countries.Select(c => new MovieCountry(c)))
            };


            if (sc.ShowDialog() != true) {
                return;
            }

            foreach (MovieCountry country in sc.SelectedCountry.SelectedItems) {
                SelectedMovie.AddCountry(country.ObservedEntity);
            }
        }

        #endregion

        #region Directors Handlers

        private void AddDirectorOnClick() {
            AddPerson addPerson = new AddPerson { Owner = ParentWindow, People = _service.People };

            if (addPerson.ShowDialog() != true) {
                return;
            }

            IPerson director;
            if (addPerson.PeopleList.SelectedIndex == -1 || addPerson.PersonName.Text != ((IPerson) addPerson.PeopleList.SelectedItem).Name) {
                director = LightInjectContainer.GetInstance<IPerson>();
                director.Name = addPerson.PersonName.Text;
                director.Thumb = addPerson.PersonThumb.Text;
            }
            else {
                director = (IPerson) addPerson.PeopleList.SelectedItem;
            }

            IPerson addDirector = SelectedMovie.AddDirector(director);
            if (addDirector == null) {
                UIHelper.ProviderCouldNotAdd();
            }
        }

        #endregion

        #region Genre Handlers

        private void AddGenreClick() {
            AddGenre ag = new AddGenre { Owner = ParentWindow };
            try {
                ag.Genres = _service.Genres;
            }
            catch (Exception) {
                
            }

            ag.ShowDialog();

            if (ag.DialogResult != true) {
                return;
            }

            if (string.IsNullOrEmpty(ag.NewGenre.Text)) {
                foreach (IGenre genre in ag.GenreList.SelectedItems) {
                    SelectedMovie.AddGenre(genre);
                }
            }
            else {
                IGenre genre = _service.Genres.FirstOrDefault(g => g.Name.Equals(ag.NewGenre.Text, StringComparison.CurrentCultureIgnoreCase));
                if (genre == null) {
                    genre = LightInjectContainer.CanGetInstance<IGenre>()
                                ? LightInjectContainer.GetInstance<IGenre>()
                                : new DesignGenre();

                    genre.Name = ag.NewGenre.Text;
                }
                SelectedMovie.AddGenre(genre);
            }
        }

        private void EditGenreOnClick(IGenre genre) {
            if (MessageBox.Show(ParentWindow, Gettext.T(
                "Please note this will edit this genre name in all the movies in the collection.\n" +
                "If you want to edit this genre only for this movie ucheck it and add a new genre.\n\n" +
                "Do you really want to edit this genre?"),
                Gettext.T("Genre edit"),
                MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
                return;
            }

            string genreName = InputBox.Show(ParentWindow, Gettext.T("Genre name:"), Gettext.T("Edit genre"), Gettext.T("Edit"), genre.Name);

            if (!genre.Name.Equals(genreName, StringComparison.CurrentCultureIgnoreCase)) {
                genre.Name = genreName;
            }
        }

        #endregion

        #region Plot Handlers

        private void SetPlotLanguageClick(MoviePlot moviePlot) {
            SelectLanguage sc = new SelectLanguage { Owner = ParentWindow, Languages = _service.Languages.Select(l => new MovieLanguage(l)) };

            if (sc.ShowDialog() == true) {
                ILanguage selectedLang = ((MovieLanguage) sc.SelectedLanguage.SelectedItem).ObservedEntity;
                moviePlot.Language = selectedLang.Name;
            }
        }

        private void RemovePotOnClick(MoviePlot moviePlot) {
            if (MessageBox.Show(ParentWindow,
                Gettext.T("Do you really want to remove plot: \"{0}\"?", moviePlot),
                Gettext.T("Confrim remove"), MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                SelectedMovie.RemovePlot(moviePlot);
            }
        }

        private void AddPlot() {
            IsAddingPlot = true;

            if (SelectedMovie == null) {
                return;
            }

            IPlot plot = LightInjectContainer.CanGetInstance<IPlot>()
                ? LightInjectContainer.GetInstance<IPlot>()
                : new DesignPlot();

            plot.Full = Gettext.T("Enter full description");

            SelectedMovie.AddPlot(plot);

            IsAddingPlot = false;
        }

        #endregion

        #region Actors Handlers

        private void AddActorOnClick() {
            AddPerson addPerson = new AddPerson(true) { Owner = ParentWindow, People = _service.People };

            if (addPerson.ShowDialog() != true) {
                return;
            }

            IPerson person = (IPerson) addPerson.PeopleList.SelectedItem;
            string addActorCharacter = addPerson.ActorCharacter.Text;

            IActor movieActor;
            if (person == null || addPerson.PersonName.Text != person.Name) {
                //the person/actor is not in the database yet

                movieActor = LightInjectContainer.CanGetInstance<IActor>()
                    ? LightInjectContainer.GetInstance<IActor>()
                    : new DesignActor();

                movieActor.Name = addPerson.PersonName.Text;
                movieActor.Thumb = addPerson.PersonThumb.Text;

                if (!string.IsNullOrEmpty(addPerson.ActorCharacter.Text)) {
                    movieActor.Character = addPerson.ActorCharacter.Text;
                }
            }
            else {
                //the person/actor is already in the database

                if (string.IsNullOrEmpty(addActorCharacter)) {
                    //check if they have already been added as an actor without a character
                    MovieActor ma = SelectedMovie.Actors.FirstOrDefault(a => a.Name == person.Name && a.Character == null);
                    if (ma != null) {
                        MessageBox.Show(ParentWindow, Gettext.T("This actor with unspecified character already exists in the list. To add another role please specify a character."));
                        return;
                    }

                    //else add it as actor with unspecified character
                    movieActor = LightInjectContainer.CanGetInstance<IActor>()
                        ? LightInjectContainer.GetInstance<IActor>()
                        : new DesignActor();

                    movieActor.Name = person.Name;
                    movieActor.Thumb = person.Thumb;
                    movieActor.ImdbID = person.ImdbID;
                }
                else {
                    //check if they have already been added as an actor with this character
                    MovieActor ma = SelectedMovie.Actors.FirstOrDefault(a => a.Name == person.Name && a.Character == addActorCharacter);
                    if (ma != null) {
                        MessageBox.Show(ParentWindow, Gettext.T("This actor already exists in the list. To add another actor's role specify a diffrent character."));
                        return;
                    }

                    //else add them as an actor with the specified character
                    movieActor = LightInjectContainer.CanGetInstance<IActor>()
                        ? LightInjectContainer.GetInstance<IActor>()
                        : new DesignActor();

                    movieActor.Name = person.Name;
                    movieActor.Thumb = person.Thumb;
                    movieActor.ImdbID = person.ImdbID;
                    movieActor.Character = addActorCharacter;
                }
            }

            SelectedMovie.AddActor(movieActor);
        }

        #endregion

        #region Studio Handlers

        private void AddStudioOnClick() {
            IEnumerable<IStudio> studios = _service.Studios ?? UIHelper.GetStudios();

            AddStudios addStudios = new AddStudios {
                Owner = ParentWindow,
                Studios = new ObservableCollection<MovieStudio>(studios.Select(s => new MovieStudio(s)))
            };

            if (addStudios.ShowDialog() != true) {
                return;
            }

            //new studio has been added
            if (addStudios.StudiosList.SelectedIndex == -1 && !string.IsNullOrEmpty(addStudios.NewStudioName.Text)) {
                IStudio studio = LightInjectContainer.CanGetInstance<IStudio>()
                                     ? LightInjectContainer.GetInstance<IStudio>()
                                     : new DesignStudio();

                studio.Name = addStudios.NewStudioName.Text;

                SelectedMovie.AddStudio(studio);
                return;
            }

            foreach (MovieStudio studio in addStudios.StudiosList.SelectedItems) {
                SelectedMovie.AddStudio(studio.ObservedEntity);
            }
        }

        private void EditStudioOnClick(MovieStudio studio) {
            if (MessageBox.Show(ParentWindow,
                Gettext.T("Please note this will edit this studio name in all the movies in the collection.\n" +
                          "If you want to edit studio name only for this movie remove this studio add a new one with desired name.\n\n" +
                          "Do you really want to edit this studio?"),
                Gettext.T("Studio edit"),
                MessageBoxButton.YesNo) != MessageBoxResult.Yes) 
            {

                return;
            }

            string studioName = InputBox.Show(ParentWindow, Gettext.T("Studio name:"), Gettext.T("Edit studio"), Gettext.T("Edit"), studio.Name);
            if (!studio.Name.Equals(studioName, StringComparison.CurrentCultureIgnoreCase)) {
                studio.Name = studioName;
            }
        }

        #endregion

        private void TrailerSearchOnClick() {
            OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Multiselect = false };

            if (ofd.ShowDialog() == true) {
                SelectedMovie.Trailer = ofd.FileName;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}