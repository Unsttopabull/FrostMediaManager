using System.Windows;
using Frost.XamlControls.Commands;
using RibbonUI.Properties;

namespace RibbonUI.Windows {
    public class SettingsWindowViewModel {

        public SettingsWindowViewModel() {
            CloseCommand = new RelayCommand<Window>(w => {
                Settings.Default.Reload();
                App.LoadSettings();

                w.DialogResult = false;
                w.Close();
            });

            SaveCommand = new RelayCommand<Window>(w => {
                App.SaveSettings();

                w.DialogResult = true;
                w.Close();
            });
        }

        public ICommand<Window>  CloseCommand { get; private set; }
        public ICommand<Window>  SaveCommand { get; private set; }
    }
}
