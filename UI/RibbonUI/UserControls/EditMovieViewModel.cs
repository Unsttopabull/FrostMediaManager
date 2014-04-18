using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Frost.Common;
using Frost.Common.Comparers;
using Frost.Common.Models.Provider;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using Microsoft.Win32;
using RibbonUI.Annotations;
using RibbonUI.Design;
using RibbonUI.Design.Fakes;
using RibbonUI.Design.Models;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows;

namespace RibbonUI.UserControls {

    public class EditMovieViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IMoviesDataService _service;
        private ObservableMovie _selectedMovie;
        private ObservableCollection<MoviePlot> _plots;
        private ObservableCollection<MovieActor> _actors;
        private ObservableCollection<MovieCountry> _countries;
        private ObservableCollection<MovieStudio> _studios;
        private ObservableCollection<IGenre> _genres;
        private ObservableCollection<MoviePerson> _directors;
        private ObservableCollection<IMovieSet> _sets;
        private MoviePlot _selectedPlot;

        public EditMovieViewModel() {
            _service = TranslationManager.IsInDesignMode
                ? new DesignMoviesDataService()
                : LightInjectContainer.GetInstance<IMoviesDataService>();

            if (TranslationManager.IsInDesignMode) {
                SelectedMovie = new ObservableMovie(new FakeMovie());
            }

            Plots = new ObservableCollection<MoviePlot>();

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

                if (_selectedMovie != null) {
                    Plots = _selectedMovie.Plots == null
                                ? new ObservableCollection<MoviePlot>()
                                : new ObservableCollection<MoviePlot>(_selectedMovie.Plots.Select(p => new MoviePlot(p)));
                    Actors = _selectedMovie.Actors == null
                                 ? new ObservableCollection<MovieActor>()
                                 : new ObservableCollection<MovieActor>(_selectedMovie.Actors.Select(a => new MovieActor(a)));
                    Countries = _selectedMovie.Countries == null
                                    ? new ObservableCollection<MovieCountry>()
                                    : new ObservableCollection<MovieCountry>(_selectedMovie.Countries.Select(c => new MovieCountry(c)));
                    Studios = _selectedMovie.Studios == null
                                  ? new ObservableCollection<MovieStudio>()
                                  : new ObservableCollection<MovieStudio>(_selectedMovie.Studios.Select(s => new MovieStudio(s)));
                    Genres = _selectedMovie.Genres == null
                                 ? new ObservableCollection<IGenre>()
                                 : new ObservableCollection<IGenre>(_selectedMovie.Genres);
                    Directors = _selectedMovie.Directors == null
                                    ? new ObservableCollection<MoviePerson>()
                                    : new ObservableCollection<MoviePerson>(_selectedMovie.Directors.Select(p => new MoviePerson(p)));
                }

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
                OnPropertyChanged("IsPlotFullEditable");
                OnPropertyChanged("IsPlotSummaryEditable");
                OnPropertyChanged("IsPlotTaglineEditable");
            }
        }

        public bool IsPlotTaglineEditable {
            get { return SelectedPlot != null && SelectedPlot["Tagline"]; }
        }

        public bool IsPlotSummaryEditable {
            get { return SelectedPlot != null && SelectedPlot["Summary"]; }
        }

        public bool IsPlotFullEditable {
            get { return SelectedPlot != null && SelectedPlot["Full"]; }
        }

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

        public ObservableCollection<MoviePlot> Plots {
            get { return _plots; }
            private set {
                if (Equals(value, _plots)) {
                    return;
                }
                _plots = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MovieActor> Actors {
            get { return _actors; }
            set {
                if (Equals(value, _actors)) {
                    return;
                }
                _actors = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MovieCountry> Countries {
            get { return _countries; }
            set {
                if (Equals(value, _countries)) {
                    return;
                }
                _countries = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MovieStudio> Studios {
            get { return _studios; }
            set {
                if (Equals(value, _studios)) {
                    return;
                }
                _studios = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IGenre> Genres {
            get { return _genres; }
            set {
                if (Equals(value, _genres)) {
                    return;
                }
                _genres = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MoviePerson> Directors {
            get { return _directors; }
            set {
                if (Equals(value, _directors)) {
                    return;
                }
                _directors = value;
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
                p => { SelectedMovie.MainPlot = p.ObservedEntity; },
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
                studio => {
                    if (SelectedMovie.RemoveStudio(studio.ObservedEntity)) {
                        Studios.Remove(studio);
                    }
                },
                studio => studio != null
            );

            EditStudioCommand = new RelayCommand<MovieStudio>(EditStudioOnClick, studio => studio != null);

            AddMovieGenreCommand = new RelayCommand<IGenre>(
                AddGenre,
                genre => genre != null
            );

            RemoveMovieGenreCommand = new RelayCommand<IGenre>(
                RemoveGenre,
                genre => genre != null
            );

            AddGenreCommand = new RelayCommand(AddGenreClick);
            EditGenreCommand = new RelayCommand<IGenre>(EditGenreOnClick, genre => genre != null);

            AddActorCommand = new RelayCommand(AddActorOnClick);
            RemoveActorCommand = new RelayCommand<MovieActor>(
                actor => {
                    if (SelectedMovie.RemoveActor(actor.ObservedEntity as IActor)) {
                        Actors.Remove(actor);
                    }
                },
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
                country => {
                    if (SelectedMovie.RemoveCountry(country.ObservedEntity)) {
                        Countries.Remove(country);
                    }
                },
                country => country != null
            );

            AddDirectorCommand = new RelayCommand(AddDirectorOnClick);
            RemoveDirectorCommand = new RelayCommand<MoviePerson>(
                director => {
                    if (SelectedMovie.RemoveDirector(director.ObservedEntity)) {
                        Directors.Remove(director);
                    }
                },
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
                if (!Countries.Contains(country)) {
                    ICountry addCountry = SelectedMovie.AddCountry(country.ObservedEntity);
                    if (addCountry != null) {
                        Countries.Add(new MovieCountry(addCountry));
                    }
                }
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
                return;
            }

            MoviePerson moviePerson = new MoviePerson(addDirector);
            if (!Directors.Contains(moviePerson)) {
                Directors.Add(moviePerson);
            }
            else {
                MessageBox.Show(TranslationManager.T("This person has already been added to this movie as a Director."));
            }
        }

        #endregion

        #region Genre Handlers

        private void AddGenreClick() {
            AddGenre ag = new AddGenre { Owner = ParentWindow, Genres = _service.Genres };
            ag.ShowDialog();

            if (string.IsNullOrEmpty(ag.NewGenre.Text)) {
                foreach (IGenre genre in ag.GenreList.SelectedItems) {
                    AddGenre(genre);
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
                AddGenre(genre);
            }
        }

        private void EditGenreOnClick(IGenre genre) {
            if (MessageBox.Show(ParentWindow, TranslationManager.T(
                "Please note this will edit this genre name in all the movies in the collection.\n" +
                "If you want to edit this genre only for this movie ucheck it and add a new genre.\n\n" +
                "Do you really want to edit this genre?"),
                TranslationManager.T("Genre edit"),
                MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
                return;
            }

            string genreName = InputBox.Show(ParentWindow, TranslationManager.T("Genre name:"), TranslationManager.T("Edit genre"), TranslationManager.T("Edit"), genre.Name);

            if (!genre.Name.Equals(genreName, StringComparison.CurrentCultureIgnoreCase)) {
                genre.Name = genreName;
            }
        }

        private void RemoveGenre(IGenre genre) {
            if (SelectedMovie.RemoveGenre(genre)) {
                Genres.Remove(genre);
            }
            else {
                UIHelper.ProviderCouldNotRemove();
            }
        }

        private void AddGenre(IGenre genre) {
            IGenre addGenre = SelectedMovie.AddGenre(genre);
            if (addGenre != null) {
                if (!Genres.Contains<IGenre>(addGenre, new HasNameEqualityComparer())) {
                    Genres.Add(addGenre);
                }
                else {
                    MessageBox.Show(TranslationManager.T("This genre has already been added to this movie."));
                }
            }
            else {
                UIHelper.ProviderCouldNotAdd();
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
                TranslationManager.T("Do you really want to remove plot: \"{0}\"?", moviePlot),
                TranslationManager.T("Confrim remove"), MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                if (SelectedMovie.RemovePlot(moviePlot.ObservedEntity)) {
                    Plots.Remove(moviePlot);
                }
            }
        }

        private void AddPlot() {
            if (SelectedMovie == null) {
                return;
            }

            IPlot plot = LightInjectContainer.CanGetInstance<IPlot>()
                ? LightInjectContainer.GetInstance<IPlot>()
                : new DesignPlot();

            plot.Full = TranslationManager.T("Enter full description");

            plot = SelectedMovie.AddPlot(plot);
            if (plot != null) {
                Plots.Add(new MoviePlot(plot));
            }
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
                    MovieActor ma = Actors.FirstOrDefault(a => a.Name == person.Name && a.Character == null);
                    if (ma != null) {
                        MessageBox.Show(ParentWindow, TranslationManager.T("This actor with unspecified character already exists in the list. To add another role specify a character."));
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
                    MovieActor ma = Actors.FirstOrDefault(a => a.Name == person.Name && a.Character == addActorCharacter);
                    if (ma != null) {
                        MessageBox.Show(ParentWindow, TranslationManager.T("This actor already exists in the list. To add another actor's role specify a diffrent character."));
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

            IActor addActor = SelectedMovie.AddActor(movieActor);
            if (addActor != null) {
                Actors.Add(new MovieActor(addActor));
            }
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

                AddStudio(studio);
                return;
            }

            foreach (MovieStudio studio in addStudios.StudiosList.SelectedItems) {
                AddStudio(studio.ObservedEntity);
            }
        }

        private void AddStudio(IStudio studio) {
            IStudio addStudio = SelectedMovie.AddStudio(studio);
            if (addStudio == null) {
                return;
            }

            MovieStudio ms = new MovieStudio(addStudio);
            if (Studios.Contains(ms)) {
                Studios.Add(ms);
            }
        }

        private void EditStudioOnClick(MovieStudio studio) {
            if (MessageBox.Show(ParentWindow,
                "Please note this will edit this studio name in all the movies in the collection.\n" +
                "If you want to edit studio name only for this movie remove this studio add a new one with desired name.\n\n" +
                "Do you really want to edit this studio?",
                "Studio edit", MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
                return;
            }

            string studioName = InputBox.Show(ParentWindow, TranslationManager.T("Studio name:"), TranslationManager.T("Edit studio"), TranslationManager.T("Edit"), studio.Name);
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