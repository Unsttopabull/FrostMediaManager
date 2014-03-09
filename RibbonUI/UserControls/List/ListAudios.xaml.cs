using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;
using RibbonUI.ViewModels.UserControls.List;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditAudios.xaml</summary>
    public partial class ListAudios : UserControl {
        public static readonly DependencyProperty AudiosProperty = DependencyProperty.Register("Audios", typeof(ObservableCollection<IAudio>), typeof(ListAudios), new PropertyMetadata(default(ObservableCollection<IAudio>), AudiosChanged));

        public ListAudios() {
            InitializeComponent();
        }

        public ObservableCollection<IAudio> Audios {
            get { return (ObservableCollection<IAudio>) GetValue(AudiosProperty); }
            set { SetValue(AudiosProperty, value); }
        }

        private static void AudiosChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListAudiosViewModel) ((ListAudios) d).DataContext).Audios = (ObservableCollection<IAudio>) e.NewValue;
        }

        private void ListAudiosOnLoaded(object sender, RoutedEventArgs e) {
            ((ListAudiosViewModel) DataContext).ParentWindow = Window.GetWindow(this);
        }
    }
}
