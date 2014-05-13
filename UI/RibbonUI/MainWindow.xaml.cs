using System;
using System.Threading;
using System.Windows;
using Frost.Common;
using Frost.GettextMarkupExtension;
using log4net;
using RibbonUI.Properties;
using RibbonUI.Util;
using RibbonUI.Windows;

namespace RibbonUI {

    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));
        private Loading _loading;

        public MainWindow(string assemblyPath) {
            Loaded += MainWindowLoaded;
            Closed += (sender, args) => Application.Current.Shutdown();

            Thread t = new Thread(() => {
                _loading = new Loading(10);
                _loading.Show();

                _loading.Closed += (sender, args) => _loading.Dispatcher.InvokeShutdown();
                System.Windows.Threading.Dispatcher.Run();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            while (_loading == null) {
                //spinloop wait to create loading splash
            }

            _loading.LabelText = "Loading settings ...";

            Settings.Load();

            _loading.ProgressMax = 30;
            _loading.ProgressValue = 10;
            _loading.LabelText = "Registering provider models ...";

            try {
                LightInjectContainer.RegisterAssembly(assemblyPath);
            }
            catch (Exception e) {
                MessageBox.Show(Gettext.T("There was an error loading the provider, see log for more info. Program will now exit."));

                if (Log.IsFatalEnabled) {
                    Log.Fatal(string.Format("There was an error loading the provider assembly file \"{0}\"", assemblyPath), e);
                }

                Application.Current.Shutdown();
                return;
            }

            if (!LightInjectContainer.CanGetInstance<IMoviesDataService>()) {
                if (Log.IsFatalEnabled) {
                    Log.Fatal("Provider did not register a service, the program can not continue");
                }

                MessageBox.Show(Gettext.T("Provider did not register a service, the program can not continue."), Gettext.T("No service registered"), MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }

            try {
                LightInjectContainer.GetInstance<IMoviesDataService>();
            }
            catch(Exception e) {
                _loading.Dispatcher.InvokeShutdown();
                MessageBox.Show(Gettext.T("Could not create provider service, the program can not continue. See log for more info."), Gettext.T("Error creating provider service"), MessageBoxButton.OK, MessageBoxImage.Error);

                if (Log.IsFatalEnabled) {
                    Log.Fatal("Could not create provider service, the program can not continue.", e);
                }

                Application.Current.Shutdown();
                return;
            }

            _loading.ProgressMax = 95;
            _loading.ProgressValue = 40;
            _loading.LabelText = "Loading provider database ...";

            DataContext = new MainWindowViewModel();

            InitializeComponent();

            _loading.ProgressValue = 100;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e) {
            _loading.Dispatcher.InvokeShutdown();
        }
    }

}