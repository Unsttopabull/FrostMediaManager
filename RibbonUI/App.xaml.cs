using System.Windows;
using System.Windows.Threading;
using Frost.GettextMarkupExtension;

namespace RibbonUI {

    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application {
        public App() {
            TranslationManager.CurrentTranslationProvider = new SecondLanguageTranslationProvider("Languages");

            DispatcherUnhandledException += UnhandledExeption;
        }

        private void UnhandledExeption(object sender, DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }
    }

}