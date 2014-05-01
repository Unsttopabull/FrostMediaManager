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
using FuzzySearch;
using Newtonsoft.Json;
using RibbonUI.Annotations;
using RibbonUI.Design.Models;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows.WebUpdate {

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
        private readonly string _downloader;
        private readonly ObservableMovie _movie;
        private string _labelText;
        private string _progressText;
        private bool _shown;
        private Visibility _closeButtonVisibility;

        public WebUpdater(string downloader, ObservableMovie movie) {
            _movieInfos = new Dictionary<string, ParsedMovieInfos>();
            LoadCaches();

            Errors = new ObservableCollection<ErrorInfo>();
            CloseButtonVisibility = Visibility.Collapsed;

            _downloader = downloader;
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

            if (!Directory.Exists("Downloaders")) {
                try {
                    Directory.CreateDirectory("Downloaders");
                }
                catch (Exception e) {
                    return;
                }
            }

            foreach (string cache in Directory.EnumerateFiles("Downloaders", "*.cache")) {
                string scrapper = Path.GetFileNameWithoutExtension(cache);
                ProgressText = scrapper;

                JsonSerializer jser = new JsonSerializer();

                ParsedMovieInfos info = null;
                using (JsonReader tr = new JsonTextReader(File.OpenText(cache))) {
                    try {
                        info = jser.Deserialize<ParsedMovieInfos>(tr);
                    }
                    catch (JsonException ex) {
                        string msg = ex.Message;
                    }
                    catch (Exception e) {
                        string msg = e.Message;
                    }
                }

                if (info != null && info.Movies != null && scrapper != null) {
                    //IEnumerable<string> valuesToIndex = info.Movies.Where(m => !string.IsNullOrEmpty(m.OriginalName)).Select(m => m.OriginalName);

                    List<string> movieNames = new List<string>();
                    foreach (ParsedMovie mov in info.Movies.Where(mov => mov != null)) {
                        if (string.IsNullOrEmpty(mov.OriginalName)) {
                            if (string.IsNullOrEmpty(mov.TranslatedName)) {
                                continue;
                            }
                            movieNames.Add(mov.TranslatedName);
                        }
                        else {
                            movieNames.Add(mov.OriginalName);
                        }
                    }


                    info.FuzzySearch = new FuzzySearchService(movieNames);

                    _movieInfos.Add(scrapper, info);
                }
            }
        }

        private void Update() {
            IParsingClient cli = LightInjectContainer.TryGetInstance<IParsingClient>(_downloader);
            if (cli == null) {
                MessageBox.Show(TranslationManager.T("Error accessing movie info provider."));
                return;
            }

            if (!_movieInfos.ContainsKey(cli.Name)) {
                _movieInfos.Add(cli.Name, new ParsedMovieInfos());
            }

            SearchMovieInfo(_movieInfos[cli.Name], cli);
        }

        private async void SearchMovieInfo(ParsedMovieInfos info, IParsingClient cli) {
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
                string message = string.Format("Found {0} {1}", match.OriginalName, string.IsNullOrEmpty(match.TranslatedName) ? null : "(" + match.TranslatedName + ")");
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

        private IEnumerable<ParsedMovie> GetMatchingMovies(IParsingClient cli) {
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

            return !string.IsNullOrEmpty(_movie.ImdbID) && cli.IsImdbSupported
                       ? cli.GetByImdbId(_movie.ImdbID).ToList()
                       : cli.GetByTitle(_movie.Title, (int) (_movie.ReleaseYear.HasValue ? _movie.ReleaseYear.Value : 0));
        }

        private void UpdateAvailable(ParsedMovieInfos info, IParsingClient client) {
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
                using (TextWriter tw = File.CreateText("Downloaders/" + client.Name + ".cache")) {
                    try {
                        js.Serialize(tw, info);
                        _movieInfos[client.Name] = info;
                    }
                    catch (Exception e) {
                    }
                }
            }
        }


        private void DownloadAndUpdate(ParsedMovie movie, IParsingClient cli) {
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

        private void WebUpdaterOnClosed(object sender, EventArgs e) {
        }
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