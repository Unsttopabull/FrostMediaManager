using System.Windows;
using System.Windows.Controls;

namespace RibbonUI.UserControls.Settings {

    /// <summary>Interaction logic for EditFeatureDetector.xaml</summary>
    public partial class FeatureDetectorSettings : UserControl {

        public FeatureDetectorSettings() {
            InitializeComponent();
        }

        private void FeatureDetectorSettingsLoaded(object sender, RoutedEventArgs e) {
            ((FeatureDetectorSettingsViewModel) DataContext).ParentWindow = Window.GetWindow(this);
        }
    }
}
