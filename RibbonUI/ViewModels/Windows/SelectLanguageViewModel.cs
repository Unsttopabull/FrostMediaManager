using System.Windows;
using System.Windows.Data;
using Frost.XamlControls.Commands;

namespace RibbonUI.ViewModels.Windows {
    class SelectLanguageViewModel {

        public SelectLanguageViewModel() {
            AddCommand = new RelayCommand<Window>(w => {
                w.DialogResult = true;
                w.Close();
            });

            CancelCommand = new RelayCommand<Window>(w => {
                w.DialogResult = false;
                w.Close();
            });
        }

        public ICommand<Window> AddCommand { get; private set; }
        public ICommand<Window> CancelCommand { get; private set; }

        public CollectionViewSource Languages { get; set; }
    }
}
