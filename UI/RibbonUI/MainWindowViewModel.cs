using System;
using System.Windows;
using System.Windows.Input;
using Frost.Common;
using Frost.XamlControls.Commands;
using RibbonUI.Util;

namespace RibbonUI {
    class MainWindowViewModel {
        private readonly IMoviesDataService _service;

        public MainWindowViewModel() {
            _service = LightInjectContainer.GetInstance<IMoviesDataService>();

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
                try {
                    _service.SaveChanges();
                }
                catch (Exception e) {
                    UIHelper.HandleProviderException(e);
                }
            }  

            LightInjectContainer.Dispose();
        }
    }
}
