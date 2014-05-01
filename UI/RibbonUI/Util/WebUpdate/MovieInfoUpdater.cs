﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Util;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
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

        public async Task Update(bool silent = false) {
            if (!MovieInfos.ContainsKey(_cli.Name)) {
                MovieInfos.Add(_cli.Name, new ParsedMovieInfos());
            }

            await SearchMovieInfo(MovieInfos[_cli.Name], _cli);
        }

        private async Task SearchMovieInfo(ParsedMovieInfos info, IParsingClient cli) {
            await Task.Run(() => UpdateAvailable(info, cli));

            if (cli == null) {
                MessageBox.Show("An error has occured accesing the parser/scrapper.");
                return;
            }

            LabelText = "Searching for available movie information.";
            ProgressText = "Searching for movie in index.";

            ParsedMovie match = null;
            if (cli.CanIndex) {
                if (info.Movies == null || !info.Movies.Any()) {
                    MessageBox.Show("An error has occured updating available movie information.");
                    return;
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
                    movies = GetMatchingMovies(cli).ToList();
                }
                catch (Exception e) {
                    MessageBox.Show("An error has occured updating available movie information.");
                    return;
                }

                FuzzySearchService fuzzySearch = new FuzzySearchService(movies.Select(m => m.OriginalName));
                List<Result> matches = fuzzySearch.Search(_movieInfo.Title).OrderByDescending(r => r.Score).ToList();

                Result bestMatch = matches.FirstOrDefault();
                if (bestMatch != null) {
                    match = movies.FirstOrDefault(m => m.OriginalName == bestMatch.Result1);
                }
            }

            if (match != null) {
                string message = string.Format("Found {0} {1}", match.OriginalName, string.IsNullOrEmpty(match.TranslatedName) ? null : "(" + match.TranslatedName + ")");
                MessageBoxResult result = MessageBox.Show(message, "Match found", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes) {
                    _movieInfo.Title = match.OriginalName;
                    DownloadAndUpdate(match, cli);
                }
            }
            else {
                MessageBox.Show(TranslationManager.T("Movie information not found."));
            }
        }

        private IEnumerable<ParsedMovie> GetMatchingMovies(IParsingClient cli) {
            if (cli.SupportsMovieHash) {


                if (_movieInfo.MovieHashes != null && _movieInfo.MovieHashes.Any()) {
                    return cli.GetByMovieHash(_movieInfo.MovieHashes);
                }
            }

            return !string.IsNullOrEmpty(_movieInfo.ImdbID) && cli.IsImdbSupported
                       ? cli.GetByImdbId(_movieInfo.ImdbID).ToList()
                       : cli.GetByTitle(_movieInfo.Title, (int) (_movieInfo.ReleaseYear.HasValue ? _movieInfo.ReleaseYear.Value : 0));
        }

        private void UpdateAvailable(ParsedMovieInfos info, IParsingClient client) {
            if (info.ParsedTime != default(DateTime) && (DateTime.Now - info.ParsedTime) <= StaleTime) {
                return;
            }

            if (client != null && client.CanIndex) {
                LabelText = TranslationManager.T("Preforming a one-time indexing operation.");
                ProgressText = TranslationManager.T("Indexing available movies");

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


        private void DownloadAndUpdate(ParsedMovie movie, IParsingClient cli) {
            LabelText = "Downloading...";
            ProgressText = "Getting movie information.";

            if (cli == null) {
                return;
            }

            Task.Run(() => cli.ParseMovieInfo(movie))
                .ContinueWith(t => {
                    if (t.IsFaulted || t.Result == null) {
                        Errors.Add(new ErrorInfo(ErrorType.Error, TranslationManager.T("There was an error downloading movie information")));
                        LabelText = "Errors have occured ...";
                        ProgressText = "";

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
            if (movie == null) {
                return null;
            }

            return null;
        }

        public Task UpdateMovie() {
            ObservableMovie movie = _movieInfo as ObservableMovie;
            if (movie == null) {
                return null;
            }

            LabelText = "Finished downloading...";
            ProgressText = "Updating movie information.";

            return Task.Run(() => {
                if (_parsedInfo.Genres != null) {
                    foreach (string genre in this._parsedInfo.Genres) {
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
                    MessageBoxResult result = MessageBox.Show("Could not add a new plot. Override current?", "", MessageBoxButton.YesNo);
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

                if (!string.IsNullOrEmpty(_parsedInfo.Country)) {
                    try {
                        movie.AddCountry(new DesignCountry(_parsedInfo.Country), true);
                    }
                    catch (Exception e) {
                        Errors.Add(new ErrorInfo(ErrorType.Warning, e.Message));
                    }
                }

                if (movie["PromotionalVideos"] && _parsedInfo.Videos != null) {
                    foreach (IParsedVideo video in _parsedInfo.Videos) {
                        IParsedVideo v = video;
                        movie.AddPromotionalVideo(new DesignPromotionalVideo(v), true);
                    }
                }

                if (movie["Ratings"] && !string.IsNullOrEmpty(_parsedInfo.MPAA)) {
                    movie.AddCertification(new DesingCertification(_parsedInfo.MPAA, new DesignCountry("United States")), true);
                }

                if (movie["Awards"] && _parsedInfo.Awards != null) {
                    foreach (IParsedAward award in _parsedInfo.Awards) {
                        IParsedAward aw = award;
                        movie.AddAward(new DesignAward(aw), true);
                    }
                }

                if (movie["Art"]) {
                    if (!string.IsNullOrEmpty(_parsedInfo.Cover)) {
                         movie.AddArt(new DesignArt(ArtType.Cover, _parsedInfo.Cover), true);
                    }

                    if (!string.IsNullOrEmpty(_parsedInfo.Fanart)) {
                        movie.AddArt(new DesignArt(ArtType.Fanart, _parsedInfo.Fanart), true);
                    }
                }
            }).ContinueWith(t => {
                if (Errors.Count > 0) {
                    LabelText = "Finished with warnings...";
                    ProgressText = "";

                    IsProgressbarIndeterminate = false;
                    ProgressValue = 100;
                    CloseButtonVisibility = Visibility.Visible;
                }
            });
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