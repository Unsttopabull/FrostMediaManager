using System.Windows;
using System.Windows.Input;
using Frost.Common;
using Frost.XamlControls.Commands;

namespace RibbonUI {
    class MainWindowViewModel {
        private readonly IMoviesDataService _service;

        public MainWindowViewModel(IMoviesDataService service) {
            _service = service;
            OnCloseCommand = new RelayCommand(OnWindowClose);
        }

        public ICommand OnCloseCommand { get; private set; }

        private void OnWindowClose() {
            if (_service == null) {
                return;
            }

            if (!_service.HasUnsavedChanges()) {
                return;
            }

            if (MessageBox.Show("There are unsaved changes, save?", "Unsaved changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                _service.SaveChanges();
            }  

            _service.Dispose();
        }
    }
}
