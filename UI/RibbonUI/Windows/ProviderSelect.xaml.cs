using System;
using System.Linq;
using System.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for ProviderSelect.xaml</summary>
    public partial class ProviderSelect : Window {
        public ProviderSelect() {
            Tag = false;
            InitializeComponent();
        }


        private void ProviderSelectOnLoaded(object sender, RoutedEventArgs e) {
            if (App.IsShutdown) {
                Close();
            }

            ProviderSelectViewModel vm = (ProviderSelectViewModel) DataContext;
            vm.Window = this;

            if (vm.Providers != null && vm.Providers.Count == 1) {
                vm.SelectProviderCommand.Execute(vm.Providers.FirstOrDefault());
            }
        }

        private void ProviderSelectOnClosed(object sender, EventArgs e) {
            if (Tag is bool && (bool) Tag != true) {
                Application.Current.Shutdown();
            }
        }
    }

}