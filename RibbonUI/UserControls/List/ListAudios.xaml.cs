using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;
using GalaSoft.MvvmLight.Ioc;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditAudios.xaml</summary>
    public partial class ListAudios : UserControl {
        public static readonly DependencyProperty AudiosProperty = DependencyProperty.Register("Audios", typeof(ObservableCollection<MovieAudio>), typeof(ListAudios), new PropertyMetadata(default(ObservableCollection<MovieAudio>), AudiosChanged));

        public ListAudios() {
            InitializeComponent();
            DataContext = SimpleIoc.Default.GetInstance<ListAudiosViewModel>();
        }

        public ObservableCollection<MovieAudio> Audios {
            get { return (ObservableCollection<MovieAudio>) GetValue(AudiosProperty); }
            set { SetValue(AudiosProperty, value); }
        }

        private static void AudiosChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListAudiosViewModel) ((ListAudios) d).DataContext).Audios = (ObservableCollection<MovieAudio>) e.NewValue;
        }

        private void ListAudiosOnLoaded(object sender, RoutedEventArgs e) {
            ((ListAudiosViewModel) DataContext).ParentWindow = Window.GetWindow(this);
        }
    }
}
