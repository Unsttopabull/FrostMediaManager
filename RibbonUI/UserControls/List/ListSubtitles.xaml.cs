using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;
using RibbonUI.ViewModels.UserControls.List;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditSubtitles.xaml</summary>
    public partial class ListSubtitles : UserControl {
        public static readonly DependencyProperty SubtitlesProperty = DependencyProperty.Register("Subtitles", typeof(ObservableCollection<ISubtitle>), typeof(ListSubtitles), new FrameworkPropertyMetadata(default(ObservableCollection<ISubtitle>), FrameworkPropertyMetadataOptions.AffectsRender, OnSubtitlesChanged));

        public ListSubtitles() {
            InitializeComponent();
        }

        public ObservableCollection<ISubtitle> Subtitles {
            get { return (ObservableCollection<ISubtitle>) GetValue(SubtitlesProperty); }
            set { SetValue(SubtitlesProperty, value); }
        }

        private static void OnSubtitlesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListSubtitlesViewModel) ((ListSubtitles) d).DataContext).Subtitles = (ObservableCollection<ISubtitle>) e.NewValue;
        }

        private void ListSubtitlesOnLoaded(object sender, RoutedEventArgs e) {
            ((ListSubtitlesViewModel) (DataContext)).ParentWindow = Window.GetWindow(this);
        }
    }
}
