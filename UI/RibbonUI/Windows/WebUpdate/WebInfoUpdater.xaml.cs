using System;
using System.ComponentModel;
using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models.Info;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Util.WebUpdate;

namespace RibbonUI.Windows.WebUpdate {

    public enum ErrorType {
        Warning,
        Error
    }

    /// <summary>Interaction logic for WebUpdater.xaml</summary>
    public partial class WebUpdater : Window {

        public WebUpdater(string downloader, ObservableMovie movie) {
            IParsingClient cli = LightInjectContainer.TryGetInstance<IParsingClient>(downloader);
            if (cli == null) {
                MessageBox.Show(Gettext.T("Error accessing movie info provider."));
                return;
            }

            DataContext = new MovieInfoUpdater(cli, movie);
            Loaded += OnWindowLoaded;

            InitializeComponent();
        }


        private void OnCloseButtonClicked(object sender, RoutedEventArgs e) {
            Close();
        }

        private void OnWindowLoaded(object sender, EventArgs e) {
            Update();
        }

        private async void Update() {
            bool updateSuccess;
            try {
                updateSuccess = await ((MovieInfoUpdater) DataContext).Update();
            }
            catch (Exception e) {
                updateSuccess = false;
            }

            if (updateSuccess) {
                try {
                    await ((MovieInfoUpdater) DataContext).UpdateMovie();
                }
                catch (Exception e) {
                }
            }

            Close();
        }

        private void WebUpdaterOnClosed(object sender, EventArgs e) {
        }
    }
}