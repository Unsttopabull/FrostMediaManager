using System;
using System.Threading.Tasks;
using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models;
using RibbonUI.Design.Models;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows.WebUpdate {

    /// <summary>Interaction logic for WebArtUpdater.xaml</summary>
    public partial class WebArtUpdater : Window {
        private readonly string _downloader;
        private readonly ObservableMovie _movie;
        private bool _shown;

        public WebArtUpdater(string downloader, ObservableMovie movie) {
            _downloader = downloader;
            _movie = movie;
            ContentRendered += OnContentRendered;
            Loaded += WebArtUpdaterLoaded;
            LabelText = TranslationManager.T("Searching for art...");

            InitializeComponent();
        }

        public Visibility CloseButtonVisibility { get; private set; }

        public string ProgressText { get; set; }

        public string LabelText { get; set; }

        void WebArtUpdaterLoaded(object sender, RoutedEventArgs e) {
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
            IFanartClient cli = LightInjectContainer.TryGetInstance<IFanartClient>(_downloader);
            if (cli == null) {
                MessageBox.Show(TranslationManager.T("Error: Downloader plugin could not be instantied."));
                return;
            }

            IParsedArts parsedArts = null;

            try {
                if (cli.IsImdbSupported && !string.IsNullOrEmpty(_movie.ImdbID)) {
                    ProgressText = "Searching for art by Imdb ID";
                    parsedArts = await Task.Run(() => cli.GetMovieArtFromImdbId(_movie.ImdbID));
                }
                else if (cli.IsTmdbSupported && !string.IsNullOrEmpty(_movie.TmdbID)) {
                    ProgressText = "Searching for art by Tmdb ID";
                    parsedArts = await Task.Run(() => cli.GetMovieArtFromTmdbId(_movie.TmdbID));
                }
                else if (cli.IsTitleSupported) {
                    ProgressText = "Searching for art by Title";
                    parsedArts = await Task.Run(() => cli.GetMovieArtFromTitle(_movie.Title, (int) (_movie.ReleaseYear.HasValue ? _movie.ReleaseYear.Value : 0)));
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
                Close();
            }

            if (parsedArts != null) {
                UpdateMovie(parsedArts);
            }
            else {
                MessageBox.Show(TranslationManager.T("No art found"));
            }

            Close();
        }

        private void UpdateMovie(IParsedArts parsedArts) {
            LabelText = "Updating movie with art....";
            ProgressText = "Updating ...";

            if (parsedArts.Covers != null) {
                foreach (IParsedArt cover in parsedArts.Covers) {
                    _movie.AddArt(new DesignArt(cover), true);
                }
            }

            if (parsedArts.Fanart != null) {
                foreach (IParsedArt cover in parsedArts.Fanart) {
                    _movie.AddArt(new DesignArt(cover), true);
                }
            }
        }
    }

}