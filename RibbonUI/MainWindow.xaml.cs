using System.ComponentModel;
using System.Data.Entity;
using System.Windows;
using System.Windows.Data;
using Frost.Common.Models.DB.MovieVo;

namespace RibbonUI {

    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow {
        internal readonly MovieVoContainer Container = new MovieVoContainer();

        public MainWindow() {
            InitializeComponent();
        }

        private void RibbonWindowLoaded(object sender, RoutedEventArgs e) {
            Container.Movies
                      .Include("Studios")
                      .Include("Arts")
                      .Include("Genres")
                      .Include("Awards")
                      .Include("ActorsLink")
                      .Include("Plots")
                      .Include("Directors")
                      .Include("Countries")
                      .Include("Audios")
                      .Include("Videos").Load();

            Container.Genres.Load();
            Container.Countries.Load();
            Container.Languages.Load();
            Container.People.Load();
            Container.Studios.Load();
            
            CollectionViewSource movieSource = (CollectionViewSource) (FindResource("MoviesSource"));
            if (movieSource != null) {
                movieSource.Source = Container.Movies.Local;
            }

            CollectionViewSource genreSurce = (CollectionViewSource) (FindResource("GenreSource"));
            if (genreSurce != null) {
                genreSurce.Source = Container.Genres.Local;
            }

            CollectionViewSource countriesSource = (CollectionViewSource) (FindResource("CountriesSource"));
            if (countriesSource != null) {
                countriesSource.Source = Container.Countries.Local;
            }

            CollectionViewSource languageSource = (CollectionViewSource) (FindResource("LanguagesSource"));
            if (languageSource != null) {
                languageSource.Source = Container.Languages.Local;
            }

            CollectionViewSource peopleSource = (CollectionViewSource) (FindResource("PeopleSource"));
            if (peopleSource != null) {
                peopleSource.Source = Container.People.Local;
            }

            CollectionViewSource studiosSource = (CollectionViewSource) (FindResource("StudiosSource"));
            if (studiosSource != null) {
                studiosSource.Source = Container.Studios.Local;
            }
        }

        /// <summary>Raises the <see cref="E:System.Windows.Window.Closing"/> event.</summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);

            if (!Container.HasUnsavedChanges()) {
                return;
            }

            if (MessageBox.Show("There are unsaved changes, save?", "Unsaved changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                Container.SaveChanges();
            }

            Container.Dispose();
        }
    }

}