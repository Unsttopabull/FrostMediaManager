using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Ioc;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditSubtitles.xaml</summary>
    public partial class ListSubtitles : UserControl {
        public static readonly DependencyProperty SubtitlesProperty = DependencyProperty.Register("Subtitles", typeof(ObservableCollection<MovieSubtitle>), typeof(ListSubtitles), new FrameworkPropertyMetadata(default(ObservableCollection<MovieSubtitle>), FrameworkPropertyMetadataOptions.AffectsRender, OnSubtitlesChanged));

        public ListSubtitles() {
            InitializeComponent();
            DataContext = SimpleIoc.Default.GetInstance<ListSubtitlesViewModel>();
        }

        public ObservableCollection<MovieSubtitle> Subtitles {
            get { return (ObservableCollection<MovieSubtitle>) GetValue(SubtitlesProperty); }
            set { SetValue(SubtitlesProperty, value); }
        }

        private static void OnSubtitlesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListSubtitlesViewModel) ((ListSubtitles) d).DataContext).Subtitles = (ObservableCollection<MovieSubtitle>) e.NewValue;
        }

        private void ListSubtitlesOnLoaded(object sender, RoutedEventArgs e) {
            ((ListSubtitlesViewModel) (DataContext)).ParentWindow = Window.GetWindow(this);
        }
    }
}
