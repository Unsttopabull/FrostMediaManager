using System.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SearchMovies.xaml</summary>
    public partial class SearchMovies : Window {
        public SearchMovies() {
            InitializeComponent();

            Loaded += SearchMoviesLoaded;
        }

        public void SearchMoviesLoaded(object sender, RoutedEventArgs e) {
            ((SearchMoviesViewModel) DataContext).ParentWindow = this;
        }
    }
}
