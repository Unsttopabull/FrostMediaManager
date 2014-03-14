using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditVideos.xaml</summary>
    public partial class ListVideos : UserControl {
        public static readonly DependencyProperty VideosProperty = DependencyProperty.Register("Videos", typeof(ObservableCollection<MovieVideo>), typeof(ListVideos), new PropertyMetadata(default(ObservableCollection<MovieVideo>), OnVideoListChanged));

        public ListVideos() {
            InitializeComponent();
            DataContext = LightInjectContainer.GetInstance<ListVideosViewModel>();
        }

        private static void OnVideoListChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((ListVideosViewModel) (((ListVideos) d).DataContext)).Videos = (ObservableCollection<MovieVideo>) args.NewValue;
        }

        public ObservableCollection<MovieVideo> Videos {
            get { return (ObservableCollection<MovieVideo>) GetValue(VideosProperty); }
            set { SetValue(VideosProperty, value); }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
            ((ListVideosViewModel) DataContext).ParentWindow = Window.GetWindow(this);
        }
    }

}