using System.Windows;
using RibbonUI.ViewModels.UserControls.Settings;
using UserControl = System.Windows.Controls.UserControl;

namespace RibbonUI.UserControls.Settings {

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
