using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Frost.Common;
using Frost.Common.Annotations;
using Frost.Common.Models;
using Frost.GettextMarkupExtension;
using Frost.Models.Frost.DB;
using Frost.Models.Frost.DB.People;
using Frost.XamlControls.Commands;
using Microsoft.Win32;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows;

namespace RibbonUI.ViewModels.UserControls {
    public class EditMovieViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private IMovie _selectedMovie;
        private ObservableHashSet2<MoviePlot> _moviePlots;
        private ObservableHashSet2<ObservableActor> _actors;

        public EditMovieViewModel() {
            MoviePlots = new ObservableHashSet2<MoviePlot>();

            #region Commands

            AddStudioCommand = new RelayCommand(AddStudioOnClick);
            RemoveStudioCommand = new RelayCommand<Studio>(
                studio => SelectedMovie.Studios.Remove(studio),
                studio => studio != null
            );
            EditStudioCommand = new RelayCommand<Studio>(EditStudioOnClick, studio => studio != null);

            AddMovieGenreCommand = new RelayCommand<Genre>(
                genre => SelectedMovie.Genres.Add(genre),
                genre => genre != null
            );
            RemoveMovieGenreCommand = new RelayCommand<Genre>(
                genre => SelectedMovie.Genres.Remove(genre),
                genre => genre != null
            );

            AddGenreCommand = new RelayCommand<ListCollectionView>(AddGenreClick);
            RemoveGenreCommand = new RelayCommand<Genre>(RemoveGenreClick, genre => genre != null);
            EditGenreCommand = new RelayCommand<Genre>(EditGenreOnClick, genre => genre != null);

            AddActorCommand = new RelayCommand(AddActorOnClick);
            RemoveActorCommand = new RelayCommand<ObservableActor>(
                actor => Actors.Remove(actor),
                actor => actor != null
            );
            EditActorCommand = new RelayCommand<ObservableActor>(
                actor => new EditPerson { Owner = ParentWindow, SelectedPerson = actor }.ShowDialog(),
                actor => actor != null
            );

            SetPlotLanguageCommand = new RelayCommand<MoviePlot>(SetPlotLanguageClick, plot => plot != null);
            AddPlotCommand = new RelayCommand(() => SelectedMovie.Plots.Add(new Plot("Enter full description", null, null)));
            RemovePlotCommand = new RelayCommand<MoviePlot>(RemovePotOnClick, plot => plot != null);

            AddCountryCommand = new RelayCommand(AddCountryOnClick);
            RemoveCountryCommand = new RelayCommand<Country>(
                country => SelectedMovie.Countries.Remove(country),
                country => country != null
            );

            AddDirectorCommand = new RelayCommand(AddDirectorOnClick);
            RemoveDirectorCommand = new RelayCommand<Person>(
                director => SelectedMovie.Directors.Remove(director),
                director => director != null
            );
            EditDirectorCommand = new RelayCommand<Person>(
                director => new EditPerson { Owner = ParentWindow, SelectedPerson = director }.ShowDialog(),
                director => director != null
            );

            TrailerSearchCommand = new RelayCommand(TrailerSearchOnClick);

            #endregion
        }

        public Window ParentWindow { get; set; }

        public IMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                if (_selectedMovie != null) {
                    MoviePlots = new ObservableHashSet2<MoviePlot>(_selectedMovie.Plots.Select(p => new MoviePlot(p)));
                    Actors = new ObservableHashSet2<ObservableActor>(_selectedMovie.Actors.Select(a => new ObservableActor(a)));
                }

                OnPropertyChanged();
            }
        }

        public ObservableHashSet2<MoviePlot> MoviePlots {
            get { return _moviePlots; }
            private set {
                if (Equals(value, _moviePlots)) {
                    return;
                }
                _moviePlots = value;
                OnPropertyChanged();
            }
        }

        public ObservableHashSet2<ObservableActor> Actors {
            get { return _actors; }
            set {
                if (Equals(value, _actors)) {
                    return;
                }
                _actors = value;
                OnPropertyChanged();
            }
        }

        #region ICommands

        public ICommand AddStudioCommand { get; private set; }
        public ICommand<Studio> RemoveStudioCommand { get; private set; }
        public ICommand<Studio> EditStudioCommand { get; private set; }

        public ICommand<Genre> AddMovieGenreCommand { get; private set; }
        public ICommand<Genre> RemoveMovieGenreCommand { get; private set; }

        public ICommand<ListCollectionView> AddGenreCommand { get; private set; }
        public ICommand<Genre> RemoveGenreCommand { get; private set; }
        public ICommand<Genre> EditGenreCommand { get; private set; }

        public ICommand AddActorCommand { get; private set; }
        public ICommand<ObservableActor> RemoveActorCommand { get; private set; }
        public ICommand<ObservableActor> EditActorCommand { get; private set; }

        public ICommand<MoviePlot> SetPlotLanguageCommand { get; private set; }
        public ICommand AddPlotCommand { get; private set; }
        public ICommand<MoviePlot> RemovePlotCommand { get; private set; }

        public ICommand AddCountryCommand { get; private set; }
        public ICommand<Country> RemoveCountryCommand { get; private set; }

        public ICommand AddDirectorCommand { get; private set; }
        public ICommand<Person> RemoveDirectorCommand { get; private set; }
        public ICommand<Person> EditDirectorCommand { get; private set; }

        public ICommand TrailerSearchCommand { get; private set; }

        #endregion

        #region Country Handlers

        private void AddCountryOnClick() {
            AddCountries sc = new AddCountries { Owner = ParentWindow, DataContext = ParentWindow.Resources["CountriesSource"] };

            if (sc.ShowDialog() != true) {
                return;
            }

            foreach (Country country in sc.SelectedCountry.SelectedItems) {
                if (!SelectedMovie.Countries.Contains(country)) {
                    SelectedMovie.Countries.Add(country);
                    //MovieCountries.Items.Refresh();
                }
            }
        }

        #endregion

        #region Directors Handlers

        private void AddDirectorOnClick() {
            AddPerson addPerson = new AddPerson { Owner = ParentWindow };

            if (addPerson.ShowDialog() != true) {
                return;
            }

            Person director = (addPerson.PersonName.Text != ((Person) addPerson.PeopleList.SelectedItem).Name)
                                  ? new Person(addPerson.PersonName.Text, addPerson.PersonThumb.Text)
                                  : (Person) addPerson.PeopleList.SelectedItem;

            SelectedMovie.Directors.Add(director);
            //MovieDirectors.Items.Refresh();
        }

        #endregion

        #region Genre Handlers

        private void AddGenreClick(ListCollectionView genres) {
            string genreName = InputBox.Show(ParentWindow, "Genre name:", "Add genre", "Add");

            if (!string.IsNullOrEmpty(genreName)) {
                if (genres.Cast<Genre>().All(genre => !genre.Name.Equals(genreName, StringComparison.CurrentCultureIgnoreCase))) {
                    Genre newGenre = new Genre(genreName);
                    newGenre = (Genre) genres.AddNewItem(newGenre);
                    genres.CommitNew();

                    SelectedMovie.Genres.Add(newGenre);

                    genres.Refresh();
                    //MovieGenres.Items.Refresh();
                }
                else {
                    MessageBox.Show(ParentWindow, "Genre with specified name already exists.");
                }
            }
        }

        private void RemoveGenreClick(Genre genre) {
            if (MessageBox.Show(ParentWindow,
                "Please note this will remove this genre from all the movies in the collection.\n" +
                "To remove the genre from this movie only click \"No\" and just uncheck it\nin the list.\n\n" +
                "Do you really want to remove this genre?",
                "Genre remove", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                CollectionViewSource itemsSource = (CollectionViewSource) ParentWindow.Resources["GenreSource"];
                ListCollectionView view = (ListCollectionView) itemsSource.View;

                view.Remove(genre);
            }
        }

        private void EditGenreOnClick(Genre genre) {
            if (MessageBox.Show(ParentWindow,
                "Please note this will edit this genre name in all the movies in the collection.\n" +
                "If you want to edit this genre only for this movie ucheck it and add a new genre.\n\n" +
                "Do you really want to edit this genre?",
                "Genre edit", MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
                return;
            }

            string genreName = InputBox.Show(ParentWindow, TranslationManager.T("Genre name:"), TranslationManager.T("Edit genre"), TranslationManager.T("Edit"), genre.Name);

            if (!genre.Name.Equals(genreName, StringComparison.CurrentCultureIgnoreCase)) {
                genre.Name = genreName;
                //MovieGenres.Items.Refresh();
            }
        }
        #endregion

        #region Plot Handlers

        private void SetPlotLanguageClick(MoviePlot moviePlot) {
            SelectLanguage sc = new SelectLanguage { Owner = ParentWindow, DataContext = ParentWindow.Resources["LanguagesSource"] };

            if (sc.ShowDialog() == true) {
                Language selectedLang = (Language) sc.SelectedLanguage.SelectedItem;

                moviePlot.Language = selectedLang.Name;

                //int currIdx = MoviePlotCombo.SelectedIndex;
                //MoviePlotCombo.SelectedIndex = -1;
                //MoviePlotCombo.SelectedIndex = currIdx;
            }
        }

        private void RemovePotOnClick(MoviePlot moviePlot) {
            if (MessageBox.Show(ParentWindow, string.Format("Do you really want to remove plot: \"{0}\"?", moviePlot), "Confrim remove", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                SelectedMovie.Plots.Remove((Plot) moviePlot.Plot);
                //MoviePlotCombo.Items.Refresh();

                //if (MoviePlotCombo.HasItems && MoviePlotCombo.SelectedIndex == -1) {
                //    MoviePlotCombo.SelectedIndex = 1;
                //}
            }
        }

        #endregion

        #region Actors Handlers

        private void AddActorOnClick() {
            AddPerson addPerson = new AddPerson(true) { Owner = ParentWindow };

            if (addPerson.ShowDialog() != true) {
                return;
            }

            IPerson person = (IPerson) addPerson.PeopleList.SelectedItem;

            string addActorCharacter = addPerson.ActorCharacter.Text;
            if (addPerson.PersonName.Text != person.Name) {
                //the person/actor is not in the database yet

                IActor actor = ModelCreator.Create<IActor>();
                actor.Name = addPerson.PersonName.Text;
                actor.Thumb = addPerson.PersonThumb.Text;
                actor.Movie = SelectedMovie;

                Actors.Add(new ObservableActor(actor));
            }
            else {
                //the person/actor is already in the database

                if (string.IsNullOrEmpty(addActorCharacter)) {
                    //check if they have already been added as an actor without a character
                    IActor movieActor = Actors.FirstOrDefault(ma => ma.Name == person.Name && ma.Character == null);
                    if (movieActor != null) {
                        MessageBox.Show(ParentWindow, "This actor with unspecified character already exists in the list. To add another role specify a character.");
                        return;
                    }

                    //else add it as actor with unspecified character
                    IActor actor = ModelCreator.Create<IActor>();
                    actor.Name = person.Name;
                    actor.Thumb = person.Thumb;
                    actor.ImdbID = person.ImdbID;
                    actor.Movie = SelectedMovie;

                    Actors.Add(new ObservableActor(actor));
                }
                else {
                    //check if they have already been added as an actor with this character
                    IActor movieActor = Actors.FirstOrDefault(ma => ma.Name == person.Name && ma.Character == addActorCharacter);
                    if (movieActor != null) {
                        MessageBox.Show(ParentWindow, "This actor already exists in the list. To add another actor's role specify a diffrent character.");
                        return;
                    }

                    //else add them as an actor with the specified character
                    IActor actor = ModelCreator.Create<IActor>();//new MovieActor(SelectedMovie, person, addActorCharacter);
                    actor.Name = person.Name;
                    actor.Thumb = person.Thumb;
                    actor.ImdbID = person.ImdbID;
                    actor.Character = addActorCharacter;

                    Actors.Add(new ObservableActor(actor));
                }
            }
            //MovieDirectors.Items.Refresh();
        }

        #endregion

        #region Studio Handlers

        private void AddStudioOnClick() {
            AddStudios addStudios = new AddStudios { Owner = ParentWindow, DataContext = ParentWindow.Resources["StudiosSource"] };

            if (addStudios.ShowDialog() != true) {
                return;
            }

            if (addStudios.StudiosList.SelectedIndex == -1 && !string.IsNullOrEmpty(addStudios.NewStudioName.Text)) {
                SelectedMovie.Studios.Add(new Studio(addStudios.NewStudioName.Text));
                return;
            }

            foreach (Studio studio in addStudios.StudiosList.SelectedItems) {
                if (!SelectedMovie.Studios.Contains(studio)) {
                    SelectedMovie.Studios.Add(studio);
                }
            }
        }

        private void EditStudioOnClick(Studio studio) {
            if (MessageBox.Show(ParentWindow,
                "Please note this will edit this studio name in all the movies in the collection.\n" +
                "If you want to edit studio name only for this movie remove this studio add a new one with desired name.\n\n" +
                "Do you really want to edit this studio?",
                "Studio edit", MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
                return;
            }

            string studioName = InputBox.Show(ParentWindow, "Studio name:", "Edit studio", "Edit", studio.Name);

            if (!studio.Name.Equals(studioName, StringComparison.CurrentCultureIgnoreCase)) {
                studio.Name = studioName;
                //MovieStudios.Items.Refresh();
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
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
