using System.Linq;
using System.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for ProviderSelect.xaml</summary>
    public partial class ProviderSelect : Window {
        public ProviderSelect() {
            InitializeComponent();
        }


        private void ProviderSelectOnLoaded(object sender, RoutedEventArgs e) {
            ProviderSelectViewModel vm = (ProviderSelectViewModel) DataContext;
            vm.Window = this;

            if (vm.Providers != null && vm.Providers.Count == 1) {
                vm.SelectProviderCommand.Execute(vm.Providers.FirstOrDefault());
            }
        }
    }

}