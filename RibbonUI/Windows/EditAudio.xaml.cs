using System.Windows;
using Frost.Common.Models;
using RibbonUI.ViewModels.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for EditAudio.xaml</summary>
    public partial class EditAudio : Window {
        public static readonly DependencyProperty SelectedAudioProperty = DependencyProperty.Register("SelectedAudio", typeof(IAudio), typeof(EditAudio), new PropertyMetadata(default(IAudio), AudioChanged));

        public EditAudio() {
            InitializeComponent();
        }

        private static void AudioChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((EditAudioViewModel) ((EditAudio) d).DataContext).SelectedAudio = (IAudio) args.NewValue;
        }

        public IAudio SelectedAudio {
            get { return (IAudio) GetValue(SelectedAudioProperty); }
            set { SetValue(SelectedAudioProperty, value); }
        }

    }
}
