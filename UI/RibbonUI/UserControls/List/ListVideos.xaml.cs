using System.Windows;
using System.Windows.Controls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditVideos.xaml</summary>
    public partial class ListVideos : UserControl {
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(ObservableMovie), typeof(ListVideos), new PropertyMetadata(default(ObservableMovie), OnMovieChanged));

        public ListVideos() {
            InitializeComponent();
        }

        public ObservableMovie Movie {
            get { return (ObservableMovie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        private static void OnMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((ListVideosViewModel) (((ListVideos) d).DataContext)).SelectedMovie = (ObservableMovie) args.NewValue;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
            ((ListVideosViewModel) DataContext).ParentWindow = Window.GetWindow(this);
        }
    }

}