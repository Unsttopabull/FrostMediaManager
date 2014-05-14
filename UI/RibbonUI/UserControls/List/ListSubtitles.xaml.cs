using System.Windows;
using System.Windows.Controls;
using Frost.RibbonUI.Util.ObservableWrappers;

namespace Frost.RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditSubtitles.xaml</summary>
    public partial class ListSubtitles : UserControl {
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(ObservableMovie), typeof(ListSubtitles), new FrameworkPropertyMetadata(default(ObservableMovie), FrameworkPropertyMetadataOptions.AffectsRender, OnMovieChanged));

        public ListSubtitles() {
            InitializeComponent();
        }

        public ObservableMovie Movie {
            get { return (ObservableMovie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        private static void OnMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListSubtitlesViewModel) ((ListSubtitles) d).DataContext).SelectedMovie = (ObservableMovie) e.NewValue;
        }

        private void ListSubtitlesOnLoaded(object sender, RoutedEventArgs e) {
            ((ListSubtitlesViewModel) (DataContext)).ParentWindow = Window.GetWindow(this);
        }
    }
}
