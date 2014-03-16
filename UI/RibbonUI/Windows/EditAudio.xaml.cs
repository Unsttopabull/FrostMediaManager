using System.Windows;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for EditAudio.xaml</summary>
    public partial class EditAudio : Window {
        public static readonly DependencyProperty SelectedAudioProperty = DependencyProperty.Register("SelectedAudio", typeof(MovieAudio), typeof(EditAudio), new PropertyMetadata(default(MovieAudio), AudioChanged));

        public EditAudio() {
            InitializeComponent();
        }

        private static void AudioChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((EditAudioViewModel) ((EditAudio) d).DataContext).SelectedAudio = (MovieAudio) args.NewValue;
        }

        public MovieAudio SelectedAudio {
            get { return (MovieAudio) GetValue(SelectedAudioProperty); }
            set { SetValue(SelectedAudioProperty, value); }
        }

    }
}
