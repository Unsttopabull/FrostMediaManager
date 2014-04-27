using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Frost.Common;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using Frost.MovieInfoProviders;
using Frost.MovieInfoProviders.Omdb;
using FuzzySearch;
using Newtonsoft.Json;
using RibbonUI.Annotations;
using RibbonUI.Design.Models;
using RibbonUI.UserControls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows {

    public enum ErrorType {
        Warning,
        Error
    }

    /// <summary>Interaction logic for WebUpdater.xaml</summary>
    public partial class WebUpdater : Window, INotifyPropertyChanged {
        private static readonly TimeSpan StaleTime = new TimeSpan(30, 0, 0);
        private static readonly Regex IMDBIdExtract = new Regex(@"(tt[0-9]+)", RegexOptions.IgnoreCase);
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Dictionary<string, ParsedMovieInfos> _movieInfos;
        private readonly WebUpdateSite _site;
        private readonly ObservableMovie _movie;
        private string _labelText;
        private string _progressText;
        private bool _shown;
        private Visibility _closeButtonVisibility;

        public WebUpdater(WebUpdateSite site, ObservableMovie movie) {
            _movieInfos = new Dictionary<string, ParsedMovieInfos>();
            LoadCaches();

            Errors = new ObservableCollection<ErrorInfo>();
            CloseButtonVisibility = Visibility.Collapsed;

            _site = site;
            _movie = movie;
            ContentRendered += OnContentRendered;

            InitializeComponent();
        }

        public string LabelText {
            get { return _labelText; }
            set {
                if (value == _labelText) {
                    return;
                }
                _labelText = value;
                OnPropertyChanged();
            }
        }

        public string ProgressText {
            get { return _progressText; }
            set {
                if (value == _progressText) {
                    return;
                }
                _progressText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ErrorInfo> Errors { get; set; }

        public Visibility CloseButtonVisibility {
            get { return _closeButtonVisibility; }
            private set {
                if (value == _closeButtonVisibility) {
                    return;
                }
                _closeButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        private void LoadCaches() {
            LabelText = "Loading caches ...";

            if (!Directory.Exists("Scrappers")) {
                try {
                    Directory.CreateDirectory("Scrappers");
                }
                catch (Exception e) {
                    return;
                }
            }

            foreach (string cache in Directory.EnumerateFiles("Scrappers", "*.cache")) {
                string scrapper = Path.GetFileNameWithoutExtension(cache);
                ProgressText = scrapper;

                JsonSerializer jser = new JsonSerializer();

                ParsedMovieInfos info = null;
                using (JsonReader tr = new JsonTextReader(File.OpenText(cache))) {
                    try {
                        info = jser.Deserialize<ParsedMovieInfos>(tr);
                    }
                    catch (Exception e) {
                    }
                }

                if (info != null && scrapper != null) {
                    info.FuzzySearch = new FuzzySearchService(info.Movies.Select(m => m.OriginalName));

                    _movieInfos.Add(scrapper, info);
                }
            }
        }

        private void Update() {
            switch (_site) {
                case WebUpdateSite.Kolosej:
                    if (!_movieInfos.ContainsKey("Kolosej")) {
                        _movieInfos.Add("Kolosej", new ParsedMovieInfos());
                    }

                    SearchMovieInfo(_movieInfos["Kolosej"], _site);
                    break;
                case WebUpdateSite.PlanetTus:
                    if (!_movieInfos.ContainsKey("Tus")) {
                        _movieInfos.Add("Tus", new ParsedMovieInfos());
                    }
                    SearchMovieInfo(_movieInfos["Tus"], _site);
                    break;
                case WebUpdateSite.GremoVKino:
                    if (!_movieInfos.ContainsKey("GremoVKino")) {
                        _movieInfos.Add("GremoVKino", new ParsedMovieInfos());
                    }
                    SearchMovieInfo(_movieInfos["GremoVKino"], _site);
                    break;
                case WebUpdateSite.OpenSubtitles:
                    if (!_movieInfos.ContainsKey("OSub")) {
                        _movieInfos.Add("OSub", new ParsedMovieInfos());
                    }
                    SearchMovieInfo(_movieInfos["OSub"], _site);
                    break;
                case WebUpdateSite.Omdb:
                    if (!_movieInfos.ContainsKey("OMDB")) {
                        _movieInfos.Add("OMDB", new ParsedMovieInfos());
                    }
                    SearchMovieInfo(_movieInfos["OMDB"], _site);
                    break;
                case WebUpdateSite.TraktTv:
                    if (!_movieInfos.ContainsKey("TraktTV")) {
                        _movieInfos.Add("TraktTV", new ParsedMovieInfos());
                    }
                    SearchMovieInfo(_movieInfos["TraktTV"], _site);
                    break;
                default:
                    Close();
                    break;
            }
        }

        private async void SearchMovieInfo(ParsedMovieInfos info, WebUpdateSite site) {
            ParsingClient cli = null;
            switch (site) {
                case WebUpdateSite.Kolosej:
                    cli = new KolosejClient();
                    break;
                case WebUpdateSite.OpenSubtitles:
                    break;
                case WebUpdateSite.GremoVKino:
                    cli = new GremoVKinoClient();
                    break;
                case WebUpdateSite.Omdb:
                    cli = new OmdbClient();
                    break;
                case WebUpdateSite.TraktTv:
                    cli = new TraktTvClient();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("site");
            }

            await Task.Run(() => UpdateAvailable(info, cli));

            if (cli == null) {
                MessageBox.Show("An error has occured accesing the parser/scrapper.");
                Close();
                return;
            }

            LabelText = "Searching for available movie information.";
            ProgressText = "Searching for movie in index.";

            ParsedMovie match = null;
            if (cli.CanIndex) {
                if (info.Movies == null || !info.Movies.Any()) {
                    MessageBox.Show("An error has occured updating available movie information.");
                    Close();
                    return;
                }

                match = info.Movies.FirstOrDefault(m => string.Equals(m.OriginalName, _movie.Title, StringComparison.InvariantCultureIgnoreCase));
                if (match == null) {
                    List<Result> matches = info.FuzzySearch.Search(_movie.Title).OrderByDescending(r => r.Score).ToList();

                    var names = matches.Select(r => r.Result1);

                    Result bestMatch = matches.FirstOrDefault();
                    if (bestMatch != null) {
                        match = info.Movies.FirstOrDefault(m => m.OriginalName == bestMatch.Result1);
                    }
                }
            }
            else {
                List<ParsedMovie> movies;
                try {
                    movies = GetMatchingMovies(cli).ToList();
                }
                catch (Exception e) {
                    MessageBox.Show("An error has occured updating available movie information.");
                    Close();
                    return;
                }

                FuzzySearchService fuzzySearch = new FuzzySearchService(movies.Select(m => m.OriginalName));
                List<Result> matches = fuzzySearch.Search(_movie.Title).OrderByDescending(r => r.Score).ToList();

                var names = matches.Select(r => r.Result1);

                Result bestMatch = matches.FirstOrDefault();
                if (bestMatch != null) {
                    match = movies.FirstOrDefault(m => m.OriginalName == bestMatch.Result1);
                }
            }

            if (match != null) {
                string message = string.Format("Found {0} {1}", match.OriginalName, string.IsNullOrEmpty(match.SloveneName) ? null : "(" + match.SloveneName + ")");
                MessageBoxResult result = MessageBox.Show(message, "Match found", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes) {
                    _movie.Title = match.OriginalName;
                    DownloadAndUpdate(match, cli);
                }
                else {
                    Close();
                }
            }
            else {
                MessageBox.Show(TranslationManager.T("Movie information not found."));
                Close();
            }
        }

        private IEnumerable<ParsedMovie> GetMatchingMovies(ParsingClient cli) {
            if (cli.SupportsMovieHash) {
                if (_movie["Videos"]) {
                    List<string> hashes = _movie.Videos
                                                .Where(v => v != null && !string.IsNullOrEmpty(v.MovieHash))
                                                .Select(m => m.MovieHash)
                                                .ToList();
                    if (hashes.Count > 0) {
                        return cli.GetByMovieHash(hashes);
                    }
                }
            }

            return !string.IsNullOrEmpty(_movie.ImdbID)
                       ? cli.GetByImdbId(_movie.ImdbID).ToList()
                       : cli.GetByTitle(_movie.Title, (int) (_movie.ReleaseYear.HasValue ? _movie.ReleaseYear.Value : 0));
        }

        private void UpdateAvailable(ParsedMovieInfos info, ParsingClient client) {
            if (info.ParsedTime != default(DateTime) && (DateTime.Now - info.ParsedTime) <= StaleTime) {
                return;
            }

            if (client != null && client.CanIndex) {
                Dispatcher.Invoke(() => {
                    LabelText = TranslationManager.T("Preforming a one-time indexing operation.");
                    ProgressText = TranslationManager.T("Indexing available movies");
                });

                try {
                    client.Index();
                }
                catch (Exception e) {
                    return;
                }

                info.FuzzySearch = new FuzzySearchService(client.AvailableMovies.Select(m => m.OriginalName));
                info.Movies = client.AvailableMovies;
                info.ParsedTime = DateTime.Now;

                JsonSerializer js = new JsonSerializer();
                using (TextWriter tw = File.CreateText("Scrappers/" + client.Name + ".cache")) {
                    try {
                        js.Serialize(tw, info);
                    }
                    catch (Exception e) {
                    }
                }
            }
        }


        private void DownloadAndUpdate(ParsedMovie movie, ParsingClient cli) {
            LabelText = "Downloading...";
            ProgressText = "Getting movie information.";

            if (cli == null) {
                return;
            }

            Task.Run(() => cli.ParseMovieInfo(movie))
                .ContinueWith(t => {
                    if (t.IsFaulted || t.Result == null) {
                        Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Error, TranslationManager.T("There was an error downloading movie information"))));
                        Dispatcher.Invoke(() => {
                            LabelText = "Errors have occured ...";
                            ProgressText = "";

                            ProgressBar.IsIndeterminate = false;
                            ProgressBar.Value = 100;
                            CloseButtonVisibility = Visibility.Visible;
                        });
                        return;
                    }

                    if (t.IsCanceled) {
                        Dispatcher.Invoke(Close);
                        return;
                    }

                    if (t.IsCompleted) {
                        UpdateMovie(t.Result);
                    }
                });
        }

        private void UpdateMovie(ParsedMovieInfo movie) {
            LabelText = "Finished downloading...";
            ProgressText = "Updating movie information.";

            Task.Run(() => {
                if (movie.Genres != null) {
                    foreach (string genre in movie.Genres) {
                        string g = genre;
                        try {
                            Dispatcher.Invoke(() => _movie.AddGenre(new DesignGenre(g), true));
                        }
                        catch (Exception e) {
                            Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message)));
                        }
                    }
                }

                if (movie.Writers != null) {
                    foreach (IParsedPerson writer in movie.Writers) {
                        IParsedPerson w = writer;
                        try {
                            Dispatcher.Invoke(() => _movie.AddWriter(new DesignPerson(w), true));
                        }
                        catch (Exception e) {
                            Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message)));
                        }
                    }
                }

                if (movie.Directors != null) {
                    foreach (IParsedPerson director in movie.Directors) {
                        IParsedPerson d = director;
                        try {
                            Dispatcher.Invoke(() => _movie.AddDirector(new DesignPerson(d), true));
                        }
                        catch (Exception e) {
                            Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message)));
                        }
                    }
                }

                if (movie.Actors != null) {
                    foreach (IParsedActor actor in movie.Actors) {
                        IParsedActor a = actor;
                        try {
                            Dispatcher.Invoke(() => _movie.AddActor(new DesignActor(a), true));
                        }
                        catch (Exception e) {
                            Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message)));
                        }
                    }
                }

                if (_movie["Trailer"] && !string.IsNullOrEmpty(movie.TrailerUrl)) {
                    _movie.Trailer = movie.TrailerUrl.Trim('/');
                }

                bool summaryAvailable = !string.IsNullOrEmpty(movie.Plot);
                if (_movie["Plots"] && summaryAvailable) {
                    try {
                        Dispatcher.Invoke(() => _movie.AddPlot(new DesignPlot { Full = movie.Plot, Tagline = movie.Tagline }, true));
                    }
                    catch (Exception e) {
                        Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message)));
                    }
                }
                else if (_movie.MainPlot != null && summaryAvailable) {
                    MessageBoxResult result = MessageBox.Show("Could not add a new plot. Override current?", "", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes) {
                        _movie.MainPlot.Full = movie.Plot;
                    }
                }

                if (_movie["ReleaseYear"] && movie.ReleaseYear.HasValue) {
                    _movie.ReleaseYear = movie.ReleaseYear.Value;
                }

                if (_movie["RatingAverage"] && !string.IsNullOrEmpty(movie.Rating)) {
                    double rating;
                    if (double.TryParse(movie.Rating, NumberStyles.Float, CultureInfo.InvariantCulture, out rating)) {
                        _movie.RatingAverage = rating;
                    }
                }

                if (_movie["ImdbID"] && !string.IsNullOrEmpty(movie.ImdbLink)) {
                    Match match = IMDBIdExtract.Match(movie.ImdbLink);
                    if (match.Groups.Count > 0) {
                        _movie.ImdbID = match.Groups[0].Value;
                    }
                }

                if (!string.IsNullOrEmpty(movie.Country)) {
                    try {
                        Dispatcher.Invoke(() => _movie.AddCountry(new DesignCountry(movie.Country), true));
                    }
                    catch (Exception e) {
                        Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message)));
                    }
                }

                if (_movie["PromotionalVideos"] && movie.Videos != null) {
                    foreach (IParsedVideo video in movie.Videos) {
                        IParsedVideo v = video;
                        Dispatcher.Invoke(() => _movie.AddPromotionalVideo(new DesignPromotionalVideo(v), true));
                    }
                }

                if (_movie["Ratings"] && !string.IsNullOrEmpty(movie.MPAA)) {
                    Dispatcher.Invoke(() => _movie.AddCertification(new DesingCertification(movie.MPAA, new DesignCountry("United States")), true));
                }

                if (_movie["Awards"] && movie.Awards != null) {
                    foreach (IParsedAward award in movie.Awards) {
                        IParsedAward aw = award;
                        Dispatcher.Invoke(() => _movie.AddAward(new DesignAward(aw), true));
                    }
                }

                if (_movie["Art"]) {
                    if (!string.IsNullOrEmpty(movie.Cover)) {
                        Dispatcher.Invoke(() => _movie.AddArt(new DesignArt(ArtType.Cover, movie.Cover), true));
                    }

                    if (!string.IsNullOrEmpty(movie.Fanart)) {
                        Dispatcher.Invoke(() => _movie.AddArt(new DesignArt(ArtType.Fanart, movie.Fanart), true));
                    }
                }
            }).ContinueWith(t => Dispatcher.Invoke(() => {
                if (Errors.Count > 0) {
                    LabelText = "Finished with warnings...";
                    ProgressText = "";

                    ProgressBar.IsIndeterminate = false;
                    ProgressBar.Value = 100;
                    CloseButtonVisibility = Visibility.Visible;
                }
                else {
                    Close();
                }
            }));
        }

        private void OnCloseButtonClicked(object sender, RoutedEventArgs e) {
            Close();
        }

        private void OnContentRendered(object sender, EventArgs e) {
            if (_shown) {
                return;
            }

            _shown = true;
            Update();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private class ParsedMovieInfos {
            public DateTime ParsedTime { get; set; }

            [JsonIgnore]
            public FuzzySearchService FuzzySearch { get; set; }

            public IEnumerable<ParsedMovie> Movies { get; set; }
        }

        public class ErrorInfo {
            public ErrorInfo(ErrorType type, string message) {
                Type = type;
                Message = message;
            }

            public ErrorType Type { get; private set; }
            public string Message { get; private set; }
        }
    }

}