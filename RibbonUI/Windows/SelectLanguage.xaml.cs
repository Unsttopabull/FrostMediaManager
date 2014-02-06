using System.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectLanguage.xaml</summary>
    public partial class SelectLanguage : Window {
        public SelectLanguage() {
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
