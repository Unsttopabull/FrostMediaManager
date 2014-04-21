using System;
using System.Collections.Generic;
using System.Windows;
using Frost.MovieInfoParsers.Kolosej;
using RibbonUI.UserControls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for WebUpdater.xaml</summary>
    public partial class WebUpdater : Window {
        private readonly ObservableMovie _movie;
        private static readonly Dictionary<string, MovieInfos> _movieInfos;

        static WebUpdater() {
            _movieInfos = new Dictionary<string, MovieInfos>();
        }

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


            KolosejMovie km = new KolosejMovie();
        }

        private class MovieInfos {
            public DateTime ParsedTime { get; set; }
            public ICollection<KolosejMovie> Movies { get; set; }
        }
    }
}
