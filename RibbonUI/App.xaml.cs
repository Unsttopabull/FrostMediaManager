using System.Windows;
using System.Windows.Threading;

namespace RibbonUI {

    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application {

        public App() {
            DispatcherUnhandledException += UnhandledExeption;
        }

        private void UnhandledExeption(object sender, DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }
    }

}