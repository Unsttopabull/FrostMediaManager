using System.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectCountry.xaml</summary>
    public partial class AddCountries : Window {
        public AddCountries() {
            InitializeComponent();
        }

        private void AddOnClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void CancelOnClick(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }
    }
}
