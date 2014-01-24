using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.Common.Models.DB.MovieVo;

namespace RibbonUI {

    /// <summary>Interaction logic for ContentGrid.xaml</summary>
    public partial class ContentGrid : UserControl, IDisposable {
        private string _filter;
        private ICollectionView _collectionView;
        private MovieVoContainer _mvc;

        public ContentGrid() {
            _filter = "";
            InitializeComponent();
            Movies = new ObservableCollection<Movie>();

            
        }

        internal void GetMovies() {
            _mvc = new MovieVoContainer(false, "movieVo.db3");
            Movies = new ObservableCollection<Movie>(_mvc.Movies.ToList());

            MovieList.ItemsSource = Movies;

            _collectionView = CollectionViewSource.GetDefaultView(Movies);
            _collectionView.Filter = mv => ((Movie) mv).Title.IndexOf(_filter, StringComparison.OrdinalIgnoreCase) != -1;

            MovieList.Items.Refresh();
        }

        internal void Save() {
            if (!_mvc.HasUnsavedChanges()) {
                return;
            }

            if (MessageBox.Show("There are unsaved changes, save?", "Unsaved changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                _mvc.SaveChanges();
            }
        }

        public ObservableCollection<Movie> Movies { get; private set; }

        public double MinRequiredWidth {
            get {
                double flagsWidth = MovieFlags != null ? MovieFlags.MinRequiredWidth : 0;
                return (MovieList != null ? MovieList.ActualWidth : 0) + (double.IsNaN(flagsWidth) ? 0 : flagsWidth);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            if (ContentGridControl != null) {
                ContentGridControl.Dispose();
            }

            if (_mvc != null) {
                _mvc.Dispose();
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e) {
            _filter = "";//ListFilter.Text;
            _collectionView.Refresh();
        }

        private void WatchedCheckChanged(object sender, RoutedEventArgs e) {
            
        }
    }

}