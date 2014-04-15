﻿using System.Threading;
using System.Windows;
using System.Windows.Media.Media3D;
using Frost.Common;
using RibbonUI.Properties;
using RibbonUI.Util;
using RibbonUI.Windows;

namespace RibbonUI {

    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow {
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

            LightInjectContainer.RegisterAssembly(assemblyPath);

            if (!LightInjectContainer.CanGetInstance<IMoviesDataService>()) {
                try {
                    _loading.Dispatcher.InvokeShutdown();
                }
                catch { }

                MessageBox.Show("Provider did not register a service, the program can not continue", "No service registered", MessageBoxButton.OK, MessageBoxImage.Error);

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