using System;
using System.Windows;
using RibbonUI.UserControls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for WebUpdater.xaml</summary>
    public partial class WebUpdater : Window {
        private readonly ObservableMovie _movie;

        public WebUpdater(WebUpdateSite site, ObservableMovie movie) {
            InitializeComponent();

            _movie = movie;
            Update(site);
        }

        private void Update(WebUpdateSite site) {
            switch (site) {
                case WebUpdateSite.Kolosej:
                    UpdateFromKolosej();
                    break;
                case WebUpdateSite.PlanetTus:
                    UpdateFromTus();
                    break;
                case WebUpdateSite.OpenSubtitles:
                    UpdateFromOpenSubtitles();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("site");
            }
        }

        private void UpdateFromOpenSubtitles() {
            
        }

        private void UpdateFromTus() {
            
        }

        private void UpdateFromKolosej() {
            
        }
    }
}
