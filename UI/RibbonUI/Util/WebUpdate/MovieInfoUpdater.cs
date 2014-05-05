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
using Frost.Common.Models;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Util;
using Frost.Common.Util.ISO;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers;
using Frost.InfoParsers.Models.Info;
using FuzzySearch;
using Newtonsoft.Json;
using RibbonUI.Annotations;
using RibbonUI.Design.Models;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows.WebUpdate;

namespace RibbonUI.Util.WebUpdate {

    public class MovieInfoUpdater : INotifyPropertyChanged {
        private static readonly TimeSpan StaleTime = new TimeSpan(30, 0, 0);
        private static readonly Regex IMDBIdExtract = new Regex(@"(tt[0-9]+)", RegexOptions.IgnoreCase);
        private static readonly Dictionary<string, ParsedMovieInfos> MovieInfos;
        private readonly IParsingClient _cli;
        private readonly IMovieInfo _movieInfo;
        private string _labelText;
        private string _progressText;
        private Visibility _closeButtonVisibility;
        private bool _isProgressbarIndeterminate;
        private double _progressValue;
        private ParsedMovieInfo _parsedInfo;
        public event PropertyChangedEventHandler PropertyChanged;

        static MovieInfoUpdater() {
            MovieInfos = new Dictionary<string, ParsedMovieInfos>();
            LoadCaches();
        }

        public MovieInfoUpdater(IParsingClient cli, IMovieInfo movieInfo) {
            _cli = cli;
            _movieInfo = movieInfo;

            Errors = new ThreadSafeObservableCollection<ErrorInfo>();
            IsProgressbarIndeterminate = true;
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

        public double ProgressValue {
            get { return _progressValue; }
            set {
                if (value.Equals(_progressValue)) {
                    return;
                }
                _progressValue = value;
                OnPropertyChanged();
            }
        }

        public bool IsProgressbarIndeterminate {
            get { return _isProgressbarIndeterminate; }
            set {
                if (value.Equals(_isProgressbarIndeterminate)) {
                    return;
                }
                _isProgressbarIndeterminate = value;
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

        private static void LoadCaches() {
            //LabelText = "Loading caches ...";

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
                //ProgressText = scrapper;

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

                    MovieInfos.Add(scrapper, info);
                }
            }
        }

        public async Task<bool> Update(bool silent = false) {
            if (!MovieInfos.ContainsKey(_cli.Name)) {
                MovieInfos.Add(_cli.Name, new ParsedMovieInfos());
            }

            return await SearchMovieInfo(MovieInfos[_cli.Name], _cli, silent);
        }

        private async Task<bool> SearchMovieInfo(ParsedMovieInfos info, IParsingClient cli, bool silent) {
            await Task.Run(() => UpdateAvailable(info, cli, silent));

            if (cli == null) {
                if (!silent) {
                    MessageBox.Show("An error has occured accessing the parser/scrapper.");
                }
                return false;
            }

            if (!silent) {
                LabelText = "Searching for available movie information.";
                ProgressText = "Searching for movie in index.";
            }

            ParsedMovie match = null;
            if (cli.CanIndex) {
                if (info.Movies == null || !info.Movies.Any()) {
                    if (!silent) {
                        MessageBox.Show("An error has occured updating available movie information.");
                    }
                    return false;
                }

                match = info.Movies.FirstOrDefault(m => string.Equals(m.OriginalName, _movieInfo.Title, StringComparison.InvariantCultureIgnoreCase));
                if (match == null) {
                    List<Result> matches = info.FuzzySearch.Search(_movieInfo.Title).OrderByDescending(r => r.Score).ToList();

                    Result bestMatch = matches.FirstOrDefault();
                    if (bestMatch != null) {
                        match = info.Movies.FirstOrDefault(m => m.OriginalName == bestMatch.Result1);
                    }
                }
            }
            else {
                List<ParsedMovie> movies;
                try {
                    IEnumerable<ParsedMovie> matchingMovies = GetMatchingMovies(cli);
                    if (matchingMovies != null) {
                        movies = matchingMovies.ToList();
                    }
                    else {
                        movies = null;
                    }
                }
                catch (Exception e) {
                    if (!silent) {
                        MessageBox.Show("An error has occured updating available movie information.");
                    }
                    return false;
                }

                FuzzySearchService fuzzySearch = new FuzzySearchService(movies.Select(m => m.OriginalName));
                List<Result> matches = fuzzySearch.Search(_movieInfo.Title).OrderByDescending(r => r.Score).ToList();

                Result bestMatch = matches.FirstOrDefault();
                if (bestMatch != null) {
                    match = movies.FirstOrDefault(m => m.OriginalName == bestMatch.Result1);
                }
            }

            if (match != null) {
                MessageBoxResult result;
                if (!silent) {
                    string message = string.Format("Found {0} {1}", match.OriginalName, string.IsNullOrEmpty(match.TranslatedName) ? null : "(" + match.TranslatedName + ")");
                    result = MessageBox.Show(message, "Match found", MessageBoxButton.YesNo);
                }
                else {
                    result = MessageBoxResult.Yes;
                }

                if (result == MessageBoxResult.Yes) {
                    _movieInfo.OriginalTitle = match.OriginalName;
                    await DownloadAndUpdate(match, cli, silent);
                    return true;
                }
            }
            else if (!silent) {
                MessageBox.Show(TranslationManager.T("Movie information not found."));
            }
            return false;
        }

        private IEnumerable<ParsedMovie> GetMatchingMovies(IParsingClient cli) {
            if (cli.SupportsMovieHash) {
                if (_movieInfo.MovieHashes != null && _movieInfo.MovieHashes.Any()) {
                    return cli.GetByMovieHash(_movieInfo.MovieHashes);
                }
            }

            if (!string.IsNullOrEmpty(_movieInfo.ImdbID) && cli.IsImdbSupported) {
                return cli.GetByImdbId(_movieInfo.ImdbID).ToList();
            }
            return cli.GetByTitle(_movieInfo.Title, (int) (_movieInfo.ReleaseYear.HasValue ? _movieInfo.ReleaseYear.Value : 0));
        }

        private void UpdateAvailable(ParsedMovieInfos info, IParsingClient client, bool silent) {
            if (info.ParsedTime != default(DateTime) && (DateTime.Now - info.ParsedTime) <= StaleTime) {
                return;
            }

            if (client != null && client.CanIndex) {
                if (!silent) {
                    LabelText = TranslationManager.T("Preforming a one-time indexing operation.");
                    ProgressText = TranslationManager.T("Indexing available movies");
                }

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
                        MovieInfos[client.Name] = info;
                    }
                    catch (Exception e) {
                    }
                }
            }
        }


        private async Task DownloadAndUpdate(ParsedMovie movie, IParsingClient cli, bool silent) {
            if (!silent) {
                LabelText = "Downloading...";
                ProgressText = "Getting movie information.";
            }

            if (cli == null) {
                return;
            }

            await Task.Run(() => cli.ParseMovieInfo(movie))
                      .ContinueWith(t => {
                          if (t.IsFaulted || t.Result == null) {
                              Errors.Add(new ErrorInfo(ErrorType.Error, TranslationManager.T("There was an error downloading movie information")));

                              if (!silent) {
                                  LabelText = "Errors have occured ...";
                                  ProgressText = "";
                              }

                              IsProgressbarIndeterminate = false;
                              ProgressValue = 100;
                              CloseButtonVisibility = Visibility.Visible;
                              return;
                          }

                          if (t.IsCanceled) {
                              return;
                          }

                          if (t.IsCompleted) {
                              _parsedInfo = t.Result;
                          }
                      });
        }

        public Task UpdateMovieInfo() {
            MovieInfo movie = _movieInfo as MovieInfo;
            if (movie == null || _parsedInfo == null) {
                return Task.FromResult<object>(null);
            }

            return Task.Run(() => {
                if (_parsedInfo.Genres != null) {
                    foreach (string genre in _parsedInfo.Genres) {
                        if (!movie.Genres.Contains(genre)) {
                            movie.Genres.Add(genre);
                        }
                    }
                }

                UpdatePeople(movie);

                if (!string.IsNullOrEmpty(_parsedInfo.TrailerUrl)) {
                    movie.Trailer = _parsedInfo.TrailerUrl.Trim('/');
                }

                bool summaryAvailable = !string.IsNullOrEmpty(_parsedInfo.Plot);
                if (summaryAvailable) {
                    movie.Plots.Add(new PlotInfo(_parsedInfo.Plot, null, _parsedInfo.Tagline, null));
                }

                if (_parsedInfo.ReleaseYear.HasValue) {
                    movie.ReleaseYear = _parsedInfo.ReleaseYear.Value;
                }

                if (!string.IsNullOrEmpty(_parsedInfo.Rating)) {
                    double rating;
                    if (double.TryParse(_parsedInfo.Rating, NumberStyles.Float, CultureInfo.InvariantCulture, out rating)) {
                        movie.RatingAverage = rating;
                    }
                }

                if (!string.IsNullOrEmpty(_parsedInfo.ImdbLink)) {
                    Match match = IMDBIdExtract.Match(_parsedInfo.ImdbLink);
                    if (match.Groups.Count > 0) {
                        movie.ImdbID = match.Groups[0].Value;
                    }
                }

                if (_parsedInfo.Countries != null) {
                    foreach (string country in _parsedInfo.Countries) {
                        ISOCountryCode countryCode = ISOCountryCodes.Instance.GetByEnglishName(country);
                        if (countryCode != null && !movie.Countries.Contains(countryCode)) {
                            movie.Countries.Add(countryCode);
                        }                        
                    }
                }

                if (_parsedInfo.Videos != null) {
                    foreach (IParsedVideo video in _parsedInfo.Videos) {
                        IParsedVideo v = video;
                        movie.AddPromotionalVideo(new DesignPromotionalVideo(v), true);
                    }
                }

                if (!string.IsNullOrEmpty(_parsedInfo.MPAA) && !movie.Certifications.Any(c => c.Country != null && string.Equals(c.Country.Alpha3, "usa", StringComparison.OrdinalIgnoreCase))) {
                    movie.Certifications.Add(new CertificationInfo(ISOCountryCodes.Instance.GetByISOCode("usa"), _parsedInfo.MPAA));
                }

                if (_parsedInfo.Awards != null) {
                    foreach (IParsedAward award in _parsedInfo.Awards) {
                        movie.Awards.Add(new AwardInfo(award.Award, award.Organization, award.IsNomination));
                    }
                }

                if (!string.IsNullOrEmpty(_parsedInfo.Cover)) {
                    movie.AddArt(new DesignArt(ArtType.Cover, _parsedInfo.Cover, _parsedInfo.CoverPreview), true);
                }

                if (!string.IsNullOrEmpty(_parsedInfo.Fanart)) {
                    movie.AddArt(new DesignArt(ArtType.Fanart, _parsedInfo.Fanart, _parsedInfo.FanartPreview), true);
                }
            });
        }

        private void UpdatePeople(MovieInfo movie) {
            if (_parsedInfo.Writers != null) {
                foreach (IParsedPerson writer in _parsedInfo.Writers) {
                    if (movie.Writers.Any(w => PersonEquals(writer, w))) {
                        continue;
                    }
                    movie.Writers.Add(new PersonInfo(writer.Name, writer.Thumb, writer.ImdbID));
                }
            }

            if (_parsedInfo.Directors != null) {
                foreach (IParsedPerson director in _parsedInfo.Directors) {
                    if (movie.Directors.Any(d => PersonEquals(director, d))) {
                        continue;
                    }
                    movie.Directors.Add(new PersonInfo(director.Name, director.Thumb, director.ImdbID));
                }
            }

            if (_parsedInfo.Actors != null) {
                foreach (IParsedActor actor in _parsedInfo.Actors) {
                    ActorInfo actorInfo = movie.Actors.FirstOrDefault(a => PersonEquals(actor, a));

                    if (actorInfo != null) {
                        if (string.Equals(actor.Character, actorInfo.Character, StringComparison.CurrentCultureIgnoreCase)) {
                            continue;
                        }

                        if (string.IsNullOrEmpty(actorInfo.Character) && !string.IsNullOrEmpty(actor.Character)) {
                            actorInfo.Character = actor.Character;
                            continue;
                        }
                    }

                    PersonInfo p = new PersonInfo(actor.Name, actor.Thumb, actor.ImdbID);
                    movie.Actors.Add(new ActorInfo(p, actor.Character));
                }
            }
        }

        private bool PersonEquals(IParsedPerson lhs, PersonInfo rhs) {
            if (lhs == null || rhs == null) {
                return false;
            }

            if (!string.IsNullOrEmpty(lhs.ImdbID) && !string.IsNullOrEmpty(rhs.ImdbID)) {
                return string.Equals(lhs.ImdbID, rhs.ImdbID, StringComparison.OrdinalIgnoreCase);
            }

            return string.Equals(lhs.Name, rhs.Name);            
        }

        public Task UpdateMovie(bool silent = false) {
            ObservableMovie movie = _movieInfo as ObservableMovie;
            if (movie == null || _parsedInfo == null) {
                return Task.FromResult<object>(null);
            }

            if (!silent) {
                LabelText = "Finished downloading...";
                ProgressText = "Updating movie information.";
            }

            return Task.Run(() => UpdateMovieInfo(silent, movie))
                       .ContinueWith(t => {
                           if (Errors.Count > 0) {
                               if (!silent) {
                                   LabelText = "Finished with warnings...";
                                   ProgressText = "";
                               }

                               IsProgressbarIndeterminate = false;
                               ProgressValue = 100;
                               CloseButtonVisibility = Visibility.Visible;
                           }
                       });
        }

        private void UpdateMovieInfo(bool silent, ObservableMovie movie) {
            if (_parsedInfo.Genres != null) {
                foreach (string genre in _parsedInfo.Genres) {
                    string g = genre;
                    try {
                        movie.AddGenre(new DesignGenre(g), true);
                    }
                    catch (Exception e) {
                        Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                    }
                }
            }

            if (_parsedInfo.Writers != null) {
                foreach (IParsedPerson writer in _parsedInfo.Writers) {
                    IParsedPerson w = writer;
                    try {
                        movie.AddWriter(new DesignPerson(w), true);
                    }
                    catch (Exception e) {
                        Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                    }
                }
            }

            if (_parsedInfo.Directors != null) {
                foreach (IParsedPerson director in _parsedInfo.Directors) {
                    IParsedPerson d = director;
                    try {
                        movie.AddDirector(new DesignPerson(d), true);
                    }
                    catch (Exception e) {
                        Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                    }
                }
            }

            if (_parsedInfo.Actors != null) {
                foreach (IParsedActor actor in _parsedInfo.Actors) {
                    IParsedActor a = actor;
                    try {
                        movie.AddActor(new DesignActor(a), true);
                    }
                    catch (Exception e) {
                        Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                    }
                }
            }

            if (movie["Trailer"] && !string.IsNullOrEmpty(_parsedInfo.TrailerUrl)) {
                movie.Trailer = _parsedInfo.TrailerUrl.Trim('/');
            }

            bool summaryAvailable = !string.IsNullOrEmpty(_parsedInfo.Plot);
            if (movie["Plots"] && summaryAvailable) {
                try {
                    movie.AddPlot(new DesignPlot { Full = _parsedInfo.Plot, Tagline = _parsedInfo.Tagline }, true);
                }
                catch (Exception e) {
                    Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                }
            }
            else if (movie.MainPlot != null && summaryAvailable) {
                MessageBoxResult result = !silent
                                              ? MessageBox.Show("Could not add a new plot. Override current?", "", MessageBoxButton.YesNo)
                                              : MessageBoxResult.Yes;

                if (result == MessageBoxResult.Yes) {
                    movie.MainPlot.Full = _parsedInfo.Plot;
                }
            }

            if (movie["ReleaseYear"] && _parsedInfo.ReleaseYear.HasValue) {
                movie.ReleaseYear = _parsedInfo.ReleaseYear.Value;
            }

            if (movie["RatingAverage"] && !string.IsNullOrEmpty(_parsedInfo.Rating)) {
                double rating;
                if (double.TryParse(_parsedInfo.Rating, NumberStyles.Float, CultureInfo.InvariantCulture, out rating)) {
                    movie.RatingAverage = rating;
                }
            }

            if (movie["ImdbID"] && !string.IsNullOrEmpty(_parsedInfo.ImdbLink)) {
                Match match = IMDBIdExtract.Match(_parsedInfo.ImdbLink);
                if (match.Groups.Count > 0) {
                    movie.ImdbID = match.Groups[0].Value;
                }
            }

            if (_parsedInfo.Countries != null) {
                foreach (string country in _parsedInfo.Countries) {
                    try {
                        movie.AddCountry(new DesignCountry(country), true);
                    }
                    catch (Exception e) {
                        Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                    }                    
                }
            }

            if (movie["PromotionalVideos"] && _parsedInfo.Videos != null) {
                foreach (IParsedVideo video in _parsedInfo.Videos) {
                    IParsedVideo v = video;
                    try {
                        movie.AddPromotionalVideo(new DesignPromotionalVideo(v), true);
                    }
                    catch (Exception e) {
                        Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                    }
                }
            }

            if (movie["Ratings"] && !string.IsNullOrEmpty(_parsedInfo.MPAA)) {
                try {
                    movie.AddCertification(new DesingCertification(_parsedInfo.MPAA, new DesignCountry("United States")), true);
                }
                catch (Exception e) {
                    Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                }
            }

            if (movie["Awards"] && _parsedInfo.Awards != null) {
                foreach (IParsedAward award in _parsedInfo.Awards) {
                    IParsedAward aw = award;
                    try {
                        movie.AddAward(new DesignAward(aw), true);
                    }
                    catch (Exception e) {
                        Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                    }
                }
            }

            if (movie["Art"]) {
                if (!string.IsNullOrEmpty(_parsedInfo.Cover)) {
                    try {
                        movie.AddArt(new DesignArt(ArtType.Cover, _parsedInfo.Cover, _parsedInfo.CoverPreview), true);
                    }
                    catch (Exception e) {
                        Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                    }
                }

                if (!string.IsNullOrEmpty(_parsedInfo.Fanart)) {
                    try {
                        movie.AddArt(new DesignArt(ArtType.Fanart, _parsedInfo.Fanart, _parsedInfo.FanartPreview), true);
                    }
                    catch (Exception e) {
                        Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                    }
                }
            }
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
    }

}