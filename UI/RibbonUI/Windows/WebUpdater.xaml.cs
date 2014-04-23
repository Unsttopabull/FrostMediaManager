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
using Frost.GettextMarkupExtension;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using Frost.MovieInfoParsers.GremoVKino;
using Frost.MovieInfoParsers.Kolosej;
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
                default:
                    return;
            }
        }

        private async void SearchMovieInfo(ParsedMovieInfos info, WebUpdateSite site) {
            await Task.Run(() => UpdateAvailable(info, site));

            LabelText = "Searching for available movie information.";
            ProgressText = "Searching for movie in index.";

            if (info.Movies == null || !info.Movies.Any()) {
                MessageBox.Show("An error has occured updating available movie information.");
                Close();
                return;
            }

            ParsedMovie match = info.Movies.FirstOrDefault(m => string.Equals(m.OriginalName, _movie.Title, StringComparison.InvariantCultureIgnoreCase));
            if (match == null) {
                List<Result> matches = info.FuzzySearch.Search(_movie.Title).OrderByDescending(r => r.Score).ToList();

                var names = matches.Select(r => r.Result1);

                Result bestMatch = matches.FirstOrDefault();
                if (bestMatch != null) {
                    match = info.Movies.FirstOrDefault(m => m.OriginalName == bestMatch.Result1);
                }
            }

            if (match != null) {
                MessageBoxResult result = MessageBox.Show(string.Format("Found {0} ({1})", match.OriginalName, match.SloveneName), "Match found", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes) {
                    _movie.Title = match.OriginalName;
                    DownloadAndUpdate(match, site);
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

        private void UpdateAvailable(ParsedMovieInfos info, WebUpdateSite site) {
            if (info.ParsedTime != default(DateTime) && (DateTime.Now - info.ParsedTime) <= StaleTime) {
                return;
            }

            Dispatcher.Invoke(() => LabelText = TranslationManager.T("Preforming a one-time indexing operation."));
            Dispatcher.Invoke(() => ProgressText = TranslationManager.T("Indexing available movies"));

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
                default:
                    throw new ArgumentOutOfRangeException("site");
            }

            if (cli != null) {
                try {
                    cli.Parse();
                }
                catch (Exception e) {
                    return;
                }

                info.FuzzySearch = new FuzzySearchService(cli.AvailableMovies.Select(m => m.OriginalName));
                info.Movies = cli.AvailableMovies;
                info.ParsedTime = DateTime.Now;

                JsonSerializer js = new JsonSerializer();
                using (TextWriter tw =  File.CreateText("Scrappers/" + cli.Name + ".cache")) {
                    try {
                        js.Serialize(tw, info);
                    }
                    catch (Exception e) {
                        
                    }
                }
            }
        }


        private void DownloadAndUpdate(ParsedMovie movie, WebUpdateSite site) {
            LabelText = "Downloading...";
            ProgressText = "Getting movie information.";

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
                default:
                    throw new ArgumentOutOfRangeException("site");
            }

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
                    foreach (string writer in movie.Writers) {
                        string w = writer;
                        try {
                            Dispatcher.Invoke(() =>  _movie.AddWriter(new DesignPerson(w, null, null), true));
                        }
                        catch (Exception e) {
                            Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message)));
                        }
                    }
                }

                if (movie.Directors != null) {
                    foreach (string director in movie.Directors) {
                        string d = director;
                        try {
                            Dispatcher.Invoke(() => _movie.AddDirector(new DesignPerson(d, null, null), true));
                        }
                        catch (Exception e) {
                            Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message)));
                        }
                    }
                }

                if (movie.Actors != null) {
                    foreach (string actor in movie.Actors) {
                        string a = actor;
                        try {
                            Dispatcher.Invoke(() => _movie.AddActor(new DesignActor(a, null), true));
                        }
                        catch (Exception e) {
                            Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message)));
                        }
                    }
                }

                if (_movie["Trailer"] && !string.IsNullOrEmpty(movie.TrailerUrl)) {
                    _movie.Trailer = movie.TrailerUrl.Trim('/');
                }

                bool summaryAvailable = !string.IsNullOrEmpty(movie.Summary);
                if (_movie["Plots"] && summaryAvailable) {
                    try {
                        Dispatcher.Invoke(() => _movie.AddPlot(new DesignPlot { Full = movie.Summary }, true));
                    }
                    catch (Exception e) {
                        Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message)));
                    }
                }
                else if (_movie.MainPlot != null && summaryAvailable) {
                    MessageBoxResult result = MessageBox.Show("Could not add a new plot. Override current?", "", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes) {
                        _movie.MainPlot.Full = movie.Summary;
                    }
                }

                if (_movie["ReleaseYear"] && !string.IsNullOrEmpty(movie.ReleaseYear)) {
                    long relase;
                    if (long.TryParse(movie.ReleaseYear, NumberStyles.Integer, CultureInfo.InvariantCulture, out relase)) {
                        _movie.ReleaseYear = relase;
                    }
                }

                if (_movie["RatingAverage"] && !string.IsNullOrEmpty(movie.ImdbRating)) {
                    double rating;
                    if (double.TryParse(movie.ImdbRating, NumberStyles.Float, CultureInfo.InvariantCulture, out rating)) {
                        _movie.RatingAverage = rating;
                    }
                }

                if (_movie["ImdbId"] && !string.IsNullOrEmpty(movie.ImdbLink)) {
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
                        Dispatcher.Invoke(() => _movie.AddPromotionalVideo(new DesignPromotionalVideo(v)));
                    }
                }

                if (_movie["Awards"] && movie.Awards != null) {
                    foreach (ParsedAward award in movie.Awards) {
                        ParsedAward aw = award;
                        Dispatcher.Invoke(() => _movie.AddAward(new DesignAward(aw)));
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