﻿using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models;
using Frost.RibbonUI.Util;
using Frost.RibbonUI.Util.ObservableWrappers;
using Frost.RibbonUI.Util.WebUpdate;

namespace Frost.RibbonUI.Windows.WebUpdate {

    /// <summary>Interaction logic for WebPromoVideoUpdater.xaml</summary>
    public partial class WebPromoVideoUpdater : Window {

        public WebPromoVideoUpdater(string downloader, ObservableMovie movie) {
            IPromotionalVideoClient cli = LightInjectContainer.TryGetInstance<IPromotionalVideoClient>(downloader);
            if (cli == null) {
                MessageBox.Show(Gettext.T("Error: Downloader plugin could not be instantied."));
                return;
            }
            DataContext = new PromoVideoUpdater(cli, movie);

            Loaded += WebArtUpdaterLoaded;

            InitializeComponent();
        }

        public Visibility CloseButtonVisibility { get; private set; }

        private void WebArtUpdaterLoaded(object sender, RoutedEventArgs e) {
            NativeMethods.HideWindowBorderButtons(this);
            Update();
        }

        private async void Update() {
            await ((PromoVideoUpdater) DataContext).Update();

            Close();
        }
    }
}
