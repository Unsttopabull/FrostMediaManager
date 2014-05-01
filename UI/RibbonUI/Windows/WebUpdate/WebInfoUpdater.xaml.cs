using System;
using System.ComponentModel;
using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Util.WebUpdate;

namespace RibbonUI.Windows.WebUpdate {

    public enum ErrorType {
        Warning,
        Error
    }

    /// <summary>Interaction logic for WebUpdater.xaml</summary>
    public partial class WebUpdater : Window, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        public WebUpdater(string downloader, ObservableMovie movie) {
            IParsingClient cli = LightInjectContainer.TryGetInstance<IParsingClient>(downloader);
            if (cli == null) {
                MessageBox.Show(TranslationManager.T("Error accessing movie info provider."));
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
            await ((MovieInfoUpdater) DataContext).Update();

            Close();
        }

        private void WebUpdaterOnClosed(object sender, EventArgs e) {
        }
    }
}