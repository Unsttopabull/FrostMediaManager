using System.Windows;
using Frost.XamlControls.Commands;

namespace RibbonUI.ViewModels.Windows {
    class AddCountriesViewModel {

        public AddCountriesViewModel() {
            AddCommand = new RelayCommand<Window>(window => {
                window.DialogResult = true;
                window.Close();
            });

            CancelCommand = new RelayCommand<Window>(window1 => {
                window1.DialogResult = false;
                window1.Close();
            });
        }

        public ICommand<Window> AddCommand { get; private set; }
        public ICommand<Window> CancelCommand { get; private set; }
    }
}
