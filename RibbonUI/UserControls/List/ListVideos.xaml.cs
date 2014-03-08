using System.Windows;
using System.Windows.Controls;
using Frost.Models.Frost.DB.Files;
using RibbonUI.Util;
using RibbonUI.ViewModels.UserControls.List;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditVideos.xaml</summary>
    public partial class ListVideos : UserControl {
        public static readonly DependencyProperty VideosProperty = DependencyProperty.Register("Videos", typeof(ObservableHashSet2<Video>), typeof(ListVideos), new PropertyMetadata(default(ObservableHashSet2<Video>), OnVideoListChanged));

        public ListVideos() {
            InitializeComponent();
        }

        private static void OnVideoListChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((ListVideosViewModel) (((ListVideos) d).DataContext)).Videos = (ObservableHashSet2<Video>) args.NewValue;
        }

        public ObservableHashSet2<Video> Videos {
            get { return (ObservableHashSet2<Video>) GetValue(VideosProperty); }
            set { SetValue(VideosProperty, value); }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
            ((ListVideosViewModel) DataContext).ParentWindow = Window.GetWindow(this);
        }
    }

}