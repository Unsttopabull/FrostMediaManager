using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using RibbonUI.Translation;
using WPFLocalizeExtension.Engine;

namespace RibbonUI {

    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application {
        public App() {
            DispatcherUnhandledException += UnhandledExeption;
            LocalizeDictionary.Instance.DefaultProvider = new GettextProvider();
            LocalizeDictionary.Instance.PropertyChanged += LocalizeCultureChanged;
            LocalizeDictionary.Instance.Culture = CultureInfo.CurrentUICulture;
        }

        private void UnhandledExeption(object sender, DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }

        private void LocalizeCultureChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName != "Culture") {
                return;
            }

            CultureInfo currentCulture = LocalizeDictionary.CurrentCulture;
            if (!LocalizeDictionary.Instance.DefaultProvider.AvailableCultures.Contains(currentCulture)) {
                currentCulture = !currentCulture.IsNeutralCulture && LocalizeDictionary.Instance.DefaultProvider.AvailableCultures.Contains(currentCulture.Parent)
                                     ? currentCulture.Parent
                                     : CultureInfo.GetCultureInfo("en-GB");

                LocalizeDictionary.Instance.Culture = currentCulture;
                return;
            }

            Thread.CurrentThread.CurrentUICulture = currentCulture;
        }
    }

}