using System;
using System.Windows;
using System.Windows.Input;
using Frost.Common;
using Frost.XamlControls.Commands;
using log4net;
using RibbonUI.Util;

namespace RibbonUI {
    class MainWindowViewModel {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindowViewModel));
        private readonly IMoviesDataService _service;

        public MainWindowViewModel() {
            try {
                _service = LightInjectContainer.GetInstance<IMoviesDataService>();
            }
            catch (Exception e) {
                if (Log.IsFatalEnabled) {
                    Log.Fatal("Failed to create a service to the provider.", e);
                }
                Application.Current.Shutdown();
            }

            OnCloseCommand = new RelayCommand(OnWindowClose);
        }

        public ICommand OnCloseCommand { get; private set; }

        private void OnWindowClose() {
            if (_service == null) {
                return;
            }

            try {
                if (!_service.HasUnsavedChanges()) {
                    return;
                }
            }
            catch (Exception e) {
                UIHelper.HandleProviderException(Log, e);
            }

            if (MessageBox.Show("There are unsaved changes, save?", "Unsaved changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                try {
                    _service.SaveChanges();
                }
                catch (Exception e) {
                    UIHelper.HandleProviderException(Log, e);
                }
            }  

            LightInjectContainer.Dispose();
        }
    }
}
