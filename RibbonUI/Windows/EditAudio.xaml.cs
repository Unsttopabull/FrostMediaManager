using System.Windows;
using Frost.Models.Frost.DB.Files;
using RibbonUI.ViewModels.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for EditAudio.xaml</summary>
    public partial class EditAudio : Window {
        public static readonly DependencyProperty SelectedAudioProperty = DependencyProperty.Register("SelectedAudio", typeof(Audio), typeof(EditAudio), new PropertyMetadata(default(Audio), AudioChanged));

        public EditAudio() {
            InitializeComponent();
        }

        private static void AudioChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((EditAudioViewModel) ((EditAudio) d).DataContext).SelectedAudio = (Audio) args.NewValue;
        }

        public Audio SelectedAudio {
            get { return (Audio) GetValue(SelectedAudioProperty); }
            set { SetValue(SelectedAudioProperty, value); }
        }

    }
}
