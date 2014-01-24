using System.Windows;

namespace RibbonUI {

    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow {

        public MainWindow() {
            InitializeComponent();
        }

        private void RibbonWindowLoaded(object sender, RoutedEventArgs e) {
            ContentGrid.GetMovies();
        }

        private void RibbonWindowClosing(object sender, System.ComponentModel.CancelEventArgs e) {
            ContentGrid.Save();
        }
    }

}