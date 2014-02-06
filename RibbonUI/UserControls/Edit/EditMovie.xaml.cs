using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.People;
using Frost.Common.Util;
using RibbonUI.Windows;

namespace RibbonUI.UserControls.Edit {

    /// <summary>Interaction logic for EditMovie.xaml</summary>
    public partial class EditMovie : UserControl {
        private Movie _movie;
        private Window _window;

        public EditMovie() {
            InitializeComponent();
        }

        #region Country Handlers

        private void RemoveCountryOnClick(object sender, RoutedEventArgs e) {
            if (MovieCountries.SelectedIndex != -1) {
                Country selectedItem = (Country) MovieCountries.SelectedItem;
                ObservableHashSet<Country> itemsSource = (ObservableHashSet<Country>) MovieCountries.ItemsSource;
                itemsSource.Remove(selectedItem);
            }
        }

        private void AddCountryOnClick(object sender, RoutedEventArgs e) {
            AddCountries sc = new AddCountries { Owner = Window.GetWindow(this), DataContext = Window.GetWindow(this).Resources["CountriesSource"] };

            if (sc.ShowDialog() == true) {
                foreach (Country country in sc.SelectedCountry.SelectedItems) {
                    if (!_movie.Countries.Contains(country)) {
                        _movie.Countries.Add(country);
                        MovieCountries.Items.Refresh();
                    }
                }
            }
        }

        #endregion

        #region Directors Handlers

        private void RemoveDirectorOnClick(object sender, RoutedEventArgs e) {
            if (MovieDirectors.SelectedIndex != -1) {
                Person selectedDirector = (Person) MovieDirectors.SelectedItem;
                ObservableHashSet<Person> directors = (ObservableHashSet<Person>) MovieDirectors.ItemsSource;
                directors.Remove(selectedDirector);
            }
        }

        private void AddDirectorOnClick(object sender, RoutedEventArgs e) {
            Window window = Window.GetWindow(this);
            AddPerson addPerson = new AddPerson { Owner = window, DataContext = window.Resources["PeopleSource"] };

            if (addPerson.ShowDialog() != true) {
                return;
            }

            Person director = (addPerson.PersonName.Text != ((Person) addPerson.PeopleList.SelectedItem).Name)
                                  ? new Person(addPerson.PersonName.Text, addPerson.PersonThumb.Text)
                                  : (Person) addPerson.PeopleList.SelectedItem;

            _movie.Directors.Add(director);
            MovieDirectors.Items.Refresh();
        }

        #endregion

        #region Genre Handlers

        private void GenreOnChecked(object sender, RoutedEventArgs e) {
            CheckBox check = (CheckBox) sender;
            Genre g = (Genre) check.DataContext;


            if (!_movie.Genres.Contains(g)) {
                _movie.Genres.Add(g);
            }
        }

        private void GenreOnUnchecked(object sender, RoutedEventArgs e) {
            CheckBox check = (CheckBox) sender;
            Genre g = (Genre) check.DataContext;

            if (_movie.Genres.Contains(g)) {
                _movie.Genres.Remove(g);
            }
        }

        private void EditMovieOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            var mov = e.NewValue as Movie;
            if (mov != null) {
                _movie = mov;
            }
        }

        private void CbMovieGenreCheckBoxLoaded(object sender, RoutedEventArgs e) {
            CheckBox cb = (CheckBox) sender;
            if (cb != null) {
                Genre g = (Genre) cb.DataContext;
                if (_movie.Genres.Contains(g)) {
                    cb.IsChecked = true;
                }
            }
        }

        private void AddGenreClick(object sender, RoutedEventArgs e) {
            string genreName = InputBox.Show(_window, "Genre name:", "Add genre", "Add");

            if (!string.IsNullOrEmpty(genreName)) {
                ListCollectionView itemsSource = (ListCollectionView) MovieGenres.ItemsSource;
                if (itemsSource.Cast<Genre>().All(genre => !genre.Name.Equals(genreName, StringComparison.CurrentCultureIgnoreCase))) {
                    Genre newGenre = new Genre(genreName);
                    newGenre = (Genre) itemsSource.AddNewItem(newGenre);
                    itemsSource.CommitNew();

                    _movie.Genres.Add(newGenre);

                    itemsSource.Refresh();
                    MovieGenres.Items.Refresh();
                }
                else {
                    MessageBox.Show(_window, "Genre with specified name already exists.");
                }
            }
        }

        private void RemoveGenreClick(object sender, RoutedEventArgs e) {
            Window window = Window.GetWindow(this);
            if (MovieGenres.SelectedIndex == -1) {
                MessageBox.Show(window, "No genre selected");
                return;
            }

            if (MessageBox.Show(window,
                "Please note this will remove this genre from all the movies in the collection.\n" +
                "To remove the genre from this movie only click \"No\" and just uncheck it\nin the list.\n\n"+
                "Do you really want to remove this genre?",
                "Genre remove", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                ListCollectionView itemsSource = (ListCollectionView) MovieGenres.ItemsSource;

                itemsSource.Remove(MovieGenres.SelectedItem);
            }
        }

        #endregion

        #region Plot Handlers

        private void SetPlotLanguageClick(object sender, RoutedEventArgs e) {
            SelectLanguage sc = new SelectLanguage { Owner = Window.GetWindow(this), DataContext = Window.GetWindow(this).Resources["LanguagesSource"] };

            if (sc.ShowDialog() == true) {
                Language selectedLang = (Language) sc.SelectedLanguage.SelectedItem;
                Plot selectedPlot = (Plot) MoviePlotCombo.SelectedItem;

                selectedPlot.Language = selectedLang.Name;

                int currIdx = MoviePlotCombo.SelectedIndex;
                MoviePlotCombo.SelectedIndex = -1;
                MoviePlotCombo.SelectedIndex = currIdx;
            }
        }

        private void AddPlotOnClick(object sender, RoutedEventArgs e) {
            _movie.Plots.Add(new Plot("Enter full description", null, null));
            MoviePlotCombo.Items.Refresh();
            if (MoviePlotCombo.SelectedIndex == -1) {
                MoviePlotCombo.SelectedIndex = 1;
            }
        }

        private void RemovePotOnClick(object sender, RoutedEventArgs e) {
            Plot selectedPlot = (Plot) MoviePlotCombo.SelectedItem;

            if (
                MessageBox.Show(Window.GetWindow(this), string.Format("Do you really want to remove plot: \"{0}\"?", selectedPlot), "Confrim remove", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes) {
                _movie.Plots.Remove(selectedPlot);
                MoviePlotCombo.Items.Refresh();

                if (MoviePlotCombo.HasItems && MoviePlotCombo.SelectedIndex == -1) {
                    MoviePlotCombo.SelectedIndex = 1;
                }
            }
        }

        #endregion

        #region Actors Handlers

        private void ActorsListOnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            if (e.EditAction == DataGridEditAction.Commit) {
                TextBox textBox = ((TextBox) e.EditingElement);
                string text = textBox.Text;
                if (string.IsNullOrEmpty(text) || (!string.IsNullOrEmpty(text) && text.OrdinalEquals("Unknown"))) {
                    textBox.Text = null;
                }
            }
        }

        private void RemoveActorOnClick(object sender, RoutedEventArgs e) {
            if (MovieActorsList.SelectedIndex != -1) {
                MovieActor movieActor = (MovieActor) MovieActorsList.SelectedItem;
                _movie.ActorsLink.Remove(movieActor);
            }
            else {
                MessageBox.Show(_window, "No actor selected");
            }
        }

        private void AddActorOnClick(object sender, RoutedEventArgs e) {
            AddPerson addPerson = new AddPerson(true) { Owner = _window, DataContext = _window.Resources["PeopleSource"] };

            if (addPerson.ShowDialog() != true) {
                return;
            }

            Person person = (Person) addPerson.PeopleList.SelectedItem;

            string addActorCharacter = addPerson.ActorCharacter.Text;
            if (addPerson.PersonName.Text != person.Name) {
                //the person/actor is not in the database yet

                Person newPerson = new Person(addPerson.PersonName.Text, addPerson.PersonThumb.Text);
                _movie.ActorsLink.Add(new MovieActor(_movie, newPerson, addActorCharacter));
            }
            else {
                //the person/actor is already in the database

                if (string.IsNullOrEmpty(addActorCharacter)) {
                    //check if they have already been added as an actor without a character
                    MovieActor movieActor = _movie.ActorsLink.FirstOrDefault(ma => ma.Person.Name == person.Name && ma.Character == null);
                    if (movieActor != null) {
                        MessageBox.Show(_window, "This actor with unspecified character already exists in the list. To add another role specify a character.");
                        return;
                    }

                    //else add it as actor with unspecified character
                    _movie.ActorsLink.Add(new MovieActor(_movie, person, null));
                }
                else {
                    //check if they have already been added as an actor with this character
                    MovieActor movieActor = _movie.ActorsLink.FirstOrDefault(ma => ma.Person.Name == person.Name && ma.Character == addActorCharacter);
                    if (movieActor != null) {
                        MessageBox.Show(_window, "This actor already exists in the list. To add another actor's role specify a diffrent character.");
                        return;
                    }

                    //else add them as an actor with the specified character
                    _movie.ActorsLink.Add(new MovieActor(_movie, person, addActorCharacter));
                }
            }
            MovieDirectors.Items.Refresh();
        }

        #endregion

        #region Studio Handlers

        private void RemoveStudioOnClick(object sender, RoutedEventArgs e) {
            if (MovieStudios.SelectedIndex != -1) {
                Studio studio = (Studio) MovieStudios.SelectedItem;
                _movie.Studios.Remove(studio);
            }
            else {
                MessageBox.Show(_window, "No studio selected");
            }
        }

        private void AddStudioOnClick(object sender, RoutedEventArgs e) {
            AddStudios addStudios = new AddStudios { Owner = _window, DataContext = _window.Resources["StudiosSource"] };

            if (addStudios.ShowDialog() != true) {
                return;
            }

            if (addStudios.StudiosList.SelectedIndex == -1 && !string.IsNullOrEmpty(addStudios.NewStudioName.Text)) {
                _movie.Studios.Add(new Studio(addStudios.NewStudioName.Text));
                return;
            }

            foreach (Studio studio in addStudios.StudiosList.SelectedItems) {
                if (!_movie.Studios.Contains(studio)) {
                    _movie.Studios.Add(studio);
                }
            }
        }

        private void EditStudioOnClick(object sender, RoutedEventArgs e) {
        }

        #endregion

        private void EditPersonOnClick(object sender, RoutedEventArgs e) {
            if (MovieActorsList.SelectedIndex == -1) {
                MessageBox.Show(_window, "No actor selected");
            }
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e) {
            _window = Window.GetWindow(this);
        }
    }

}