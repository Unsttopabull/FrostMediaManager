using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Frost.Common.Models.DB.MovieVo;

namespace RibbonUI {

    /// <summary>Interaction logic for ContentGrid.xaml</summary>
    public partial class ContentGrid : UserControl, IDisposable {
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(Movie), typeof(ContentGrid),
            new FrameworkPropertyMetadata(default(Movie), FrameworkPropertyMetadataOptions.AffectsRender));
        private string _filter;
        private ICollectionView _collectionView;
        private MovieVoContainer _container;

        public ContentGrid() {
            _filter = "";
            InitializeComponent();
            Movies = new ObservableCollection<Movie>();
        }

        public List<Language> Languages {
            get { return _container.Languages.ToList(); }
        }

        public ObservableCollection<Movie> Movies { get; private set; }

        public Movie SelectedMovie {
            get { return (Movie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }

        public double MinRequiredWidth {
            get {
                double flagsWidth = MovieFlags != null ? MovieFlags.MinRequiredWidth : 0;
                return (MovieList != null ? MovieList.ActualWidth : 0) + (double.IsNaN(flagsWidth) ? 0 : flagsWidth);
            }
        }

        internal void GetMovies() {
            _container = new MovieVoContainer(false, "movieVo.db3");
            Movies = new ObservableCollection<Movie>(_container.Movies
                                                               .Include("Studios")
                                                               .Include("Arts")
                                                               .Include("Genres")
                                                               .Include("Awards")
                                                               .Include("ActorsLink")
                                                               .Include("Audios")
                                                               .ToList());
            _container.Languages.ToList();

            MovieList.ItemsSource = Movies;

            _collectionView = CollectionViewSource.GetDefaultView(Movies);
            _collectionView.Filter = mv => ((Movie) mv).Title.IndexOf(_filter, StringComparison.OrdinalIgnoreCase) != -1;

            MovieList.Items.Refresh();
        }

        internal void Save() {
            if (!_container.HasUnsavedChanges()) {
                return;
            }

            if (MessageBox.Show("There are unsaved changes, save?", "Unsaved changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                _container.SaveChanges();
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e) {
            _filter = ListFilter.Text;
            _collectionView.Refresh();
        }

        private void ListFilterKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                SearchClick(null, null);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            if (ContentGridControl != null) {
                ContentGridControl.Dispose();
            }

            if (_container != null) {
                _container.Dispose();
            }
        }

        private void MovieSubtitlesGotFocus(object sender, RoutedEventArgs e) {
            ((MainWindow) ((Grid) Parent).Parent).Ribbon.SubtitlesTab.IsSelected = true;
        }

        private void MovieSubtitlesOnLostFocus(object sender, RoutedEventArgs e) {
            ((MainWindow) ((Grid) Parent).Parent).Ribbon.Search.IsSelected = true;
        }

        private void SubtitlesList_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            if (e.EditAction == DataGridEditAction.Commit) {
                if (e.Column.Header as string == "Language") {

                }
            }
        }
    }

}