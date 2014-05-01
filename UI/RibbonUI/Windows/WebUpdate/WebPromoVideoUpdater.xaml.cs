using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models;
using RibbonUI.Design.Models;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows.WebUpdate {

    /// <summary>Interaction logic for WebPromoVideoUpdater.xaml</summary>
    public partial class WebPromoVideoUpdater : Window {
        private readonly string _downloader;
        private readonly ObservableMovie _movie;
        private bool _shown;

        public WebPromoVideoUpdater(string downloader, ObservableMovie movie) {
            _downloader = downloader;
            _movie = movie;
            ContentRendered += OnContentRendered;
            Loaded += WebArtUpdaterLoaded;
            LabelText = TranslationManager.T("Searching for videos...");

            InitializeComponent();
        }

        public Visibility CloseButtonVisibility { get; private set; }

        public string ProgressText { get; set; }

        public string LabelText { get; set; }

        private void WebArtUpdaterLoaded(object sender, RoutedEventArgs e) {
            NativeMethods.HideWindowBorderButtons(this);
        }

        private void OnContentRendered(object sender, EventArgs e) {
            if (_shown) {
                return;
            }

            _shown = true;
            Update();
        }

        private async void Update() {
            IPromotionalVideoClient cli = LightInjectContainer.TryGetInstance<IPromotionalVideoClient>(_downloader);
            if (cli == null) {
                MessageBox.Show(TranslationManager.T("Error: Downloader plugin could not be instantied."));
                return;
            }

            IEnumerable<IParsedVideo> parsedVideos = null;

            try {
                if (cli.IsImdbSupported && !string.IsNullOrEmpty(_movie.ImdbID)) {
                    ProgressText = "Searching for videos by Imdb ID";
                    parsedVideos = await Task.Run(() => cli.GetMovieArtFromImdbId(_movie.ImdbID));
                }
                else if (cli.IsTmdbSupported && !string.IsNullOrEmpty(_movie.TmdbID)) {
                    ProgressText = "Searching for videos by Tmdb ID";
                    parsedVideos = await Task.Run(() => cli.GetMovieArtFromTmdbId(_movie.TmdbID));
                }
                else if (cli.IsTitleSupported) {
                    ProgressText = "Searching for videos by Title";
                    parsedVideos = await Task.Run(() => cli.GetMovieArtFromTitle(_movie.Title, (int) (_movie.ReleaseYear.HasValue ? _movie.ReleaseYear.Value : 0)));
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
                Close();
            }

            if (parsedVideos != null) {
                UpdateMovie(parsedVideos);
            }
            else {
                MessageBox.Show(TranslationManager.T("No videos found"));
            }

            Close();
        }

        private void UpdateMovie(IEnumerable<IParsedVideo> videos) {
            LabelText = "Updating movie with promotional videos....";
            ProgressText = "Updating ...";

            foreach (IParsedVideo parsedVideo in videos) {
                _movie.AddPromotionalVideo(new DesignPromotionalVideo(parsedVideo), true);
            }
        }
    }
}
