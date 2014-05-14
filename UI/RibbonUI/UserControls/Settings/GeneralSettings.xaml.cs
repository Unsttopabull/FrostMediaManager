using System.Windows;
using System.Windows.Controls;

namespace Frost.RibbonUI.UserControls.Settings {

    /// <summary>Interaction logic for GeneralSettings.xaml</summary>
    public partial class GeneralSettings : UserControl {
        public GeneralSettings() {
            InitializeComponent();
        }


        private void GeneralSettingsOnLoaded(object sender, RoutedEventArgs e) {
            ((GeneralSettingsViewModel) DataContext).ParentWindow = Window.GetWindow(this);
        }
    }
}
