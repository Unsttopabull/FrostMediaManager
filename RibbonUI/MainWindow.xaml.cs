using System.ComponentModel;
using System.Data.Entity;
using System.Windows;
using System.Windows.Data;
using Frost.Common.Models.DB.MovieVo;

namespace RibbonUI {

    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow {
        private readonly MovieVoContainer _container = new MovieVoContainer();

        public MainWindow() {
            InitializeComponent();
        }

        private void RibbonWindowLoaded(object sender, RoutedEventArgs e) {
            _container.Movies
                      .Include("Studios")
                      .Include("Arts")
                      .Include("Genres")
                      .Include("Awards")
                      .Include("ActorsLink")
                      .Include("Plots")
                      .Include("Directors")
                      .Include("Countries")
                      .Include("Audios").Load();

            _container.Genres.Load();
            _container.Countries.Load();
            _container.Languages.Load();
            _container.People.Load();
            _container.Studios.Load();
            
            CollectionViewSource movieSource = (CollectionViewSource) (FindResource("MoviesSource"));
            if (movieSource != null) {
                movieSource.Source = _container.Movies.Local;
            }

            CollectionViewSource genreSurce = (CollectionViewSource) (FindResource("GenreSource"));
            if (genreSurce != null) {
                genreSurce.Source = _container.Genres.Local;
            }

            CollectionViewSource countriesSource = (CollectionViewSource) (FindResource("CountriesSource"));
            if (countriesSource != null) {
                countriesSource.Source = _container.Countries.Local;
            }

            CollectionViewSource languageSource = (CollectionViewSource) (FindResource("LanguagesSource"));
            if (languageSource != null) {
                languageSource.Source = _container.Languages.Local;
            }

            CollectionViewSource peopleSource = (CollectionViewSource) (FindResource("PeopleSource"));
            if (peopleSource != null) {
                peopleSource.Source = _container.People.Local;
            }

            CollectionViewSource studiosSource = (CollectionViewSource) (FindResource("StudiosSource"));
            if (studiosSource != null) {
                studiosSource.Source = _container.Studios.Local;
            }
        }

        /// <summary>Raises the <see cref="E:System.Windows.Window.Closing"/> event.</summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);

            if (!_container.HasUnsavedChanges()) {
                return;
            }

            if (MessageBox.Show("There are unsaved changes, save?", "Unsaved changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                _container.SaveChanges();
            }

            _container.Dispose();
        }
    }

}