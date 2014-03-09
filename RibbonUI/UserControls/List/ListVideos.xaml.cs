using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;
using RibbonUI.ViewModels.UserControls.List;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditVideos.xaml</summary>
    public partial class ListVideos : UserControl {
        public static readonly DependencyProperty VideosProperty = DependencyProperty.Register("Videos", typeof(ObservableCollection<IVideo>), typeof(ListVideos), new PropertyMetadata(default(ObservableCollection<IVideo>), OnVideoListChanged));

        public ListVideos() {
            InitializeComponent();
        }

        private static void OnVideoListChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((ListVideosViewModel) (((ListVideos) d).DataContext)).Videos = (ObservableCollection<IVideo>) args.NewValue;
        }

        public ObservableCollection<IVideo> Videos {
            get { return (ObservableCollection<IVideo>) GetValue(VideosProperty); }
            set { SetValue(VideosProperty, value); }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
            ((ListVideosViewModel) DataContext).ParentWindow = Window.GetWindow(this);
        }
    }

}