﻿using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models.Art;
using Frost.RibbonUI.Util;
using Frost.RibbonUI.Util.ObservableWrappers;
using Frost.RibbonUI.Util.WebUpdate;

namespace Frost.RibbonUI.Windows.WebUpdate {

    /// <summary>Interaction logic for WebArtUpdater.xaml</summary>
    public partial class WebArtUpdater : Window {

        public WebArtUpdater(string downloader, ObservableMovie movie) {
            IFanartClient cli = LightInjectContainer.TryGetInstance<IFanartClient>(downloader);
            if (cli == null) {
                MessageBox.Show(Gettext.T("Error: Downloader plugin could not be instantied."));
                return;
            }
            DataContext = new ArtUpdater(cli, movie);

            Loaded += WebArtUpdaterLoaded;

            InitializeComponent();
        }

        public Visibility CloseButtonVisibility { get; private set; }

        void WebArtUpdaterLoaded(object sender, RoutedEventArgs e) {
            NativeMethods.HideWindowBorderButtons(this);
            Update();
        }

        private async void Update() {
            await ((ArtUpdater) DataContext).Update();

            Close();
        }
    }

}