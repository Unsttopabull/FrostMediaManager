using System.Windows;
using RibbonUI.Util;

namespace RibbonUI.Windows.Search {

    /// <summary>Interaction logic for SearchMovies.xaml</summary>
    public partial class SearchMovies : Window {

        public SearchMovies(bool searchInfo, bool searchArt, bool searchVideos, Plugin info = null, Plugin art = null, Plugin videos = null) {
            InitializeComponent();

            SearchMoviesViewModel viewModel = ViewModel;
            if (viewModel != null) {
                viewModel.SearchInfo = searchInfo;
                viewModel.SearchArt = searchArt;
                viewModel.SearchVideos = searchVideos;

                if (info != null) {
                    viewModel.InfoPlugin = info;
                }

                if (art != null) {
                    viewModel.ArtPlugin = art;
                }

                if (videos != null) {
                    viewModel.VideoPlugin = videos;
                }
            }

            Loaded += SearchMoviesLoaded;
        }

        public SearchMoviesViewModel ViewModel {
            get { return DataContext as SearchMoviesViewModel; }
        }

        public void SearchMoviesLoaded(object sender, RoutedEventArgs e) {

            if (ViewModel != null) {
                ViewModel.ParentWindow = this;
                ViewModel.Search();
            }
        }
    }
}
