using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.DetectFeatures;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Design.Fakes;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    public class ArtAndPlotViewModel : INotifyPropertyChanged {
        private const string IMDB_PERSON_URI = "http://www.imdb.com/name/{0}";
        private const string IMDB_TITLE_URI = "http://www.imdb.com/title/{0}";
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableMovie _selectedMovie;

        public ArtAndPlotViewModel() {
            if (TranslationManager.IsInDesignMode) {
                SelectedMovie = new ObservableMovie(new FakeMovie());
            }

            GoToImdbCommand = new RelayCommand<string>(GoToIMDB);

            ActorImdbClickedCommand = new RelayCommand<string>(
                imdbId => Process.Start(String.Format(IMDB_PERSON_URI, imdbId)),
                s => !String.IsNullOrEmpty(s)
                );

            GoToTrailerCommand = new RelayCommand<string>(GoToTrailer);
        }

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                if (_selectedMovie != null && _selectedMovie.Countries != null) {
                    Countries = _selectedMovie.Countries.Select(c => new MovieCountry(c)).ToList();
                }
                else {
                    Countries = new List<MovieCountry>();
                }

                OnPropertyChanged("Countries");
                OnPropertyChanged("NumberOfOscarsWon");
                OnPropertyChanged("NumberOfOscarNominations");
                OnPropertyChanged("NumberOfCannesAwards");
                OnPropertyChanged("NumberOfCannesNominations");
                OnPropertyChanged("NumberOfGoldenGlobeNominations");
                OnPropertyChanged("NumberOfGoldenGlobesWon");
                OnPropertyChanged("MPAARatingImage");
                OnPropertyChanged("FirstFanart");
                OnPropertyChanged("FirstCoverOrPoster");
                OnPropertyChanged("FirstPlot");
                OnPropertyChanged("BoxImage");
                OnPropertyChanged("GenreNames");

                OnPropertyChanged();
            }
        }

        #region Utility properties

        //public IEnumerable<IActor> Actors { get; set; }

        public List<MovieCountry> Countries { get; set; }

        public string BoxImage {
            get {
                string path;
                if (SelectedMovie == null || (SelectedMovie != null && SelectedMovie.Type == MovieType.Unknown)) {
                    path = "Images/Boxes/generic.png";
                }
                else {
                    MovieType movieType = SelectedMovie.Type;
                    if (movieType == MovieType.ISO) {
                        movieType = MovieType.DVD;
                    }

                    path = "Images/Boxes/" + movieType + ".png";
                }
                return Path.Combine(Directory.GetCurrentDirectory(), path);
            }
        }

        #region Awards

        public int NumberOfOscarsWon {
            get {
                return SelectedMovie != null && SelectedMovie.Awards != null
                           ? SelectedMovie.Awards.Count(a => a.Organization == "Oscar" && !a.IsNomination)
                           : 0;
            }
        }

        public int NumberOfGoldenGlobesWon {
            get {
                return SelectedMovie != null && SelectedMovie.Awards != null
                           ? SelectedMovie.Awards.Count(a => a.Organization == "Golden Globe" && !a.IsNomination)
                           : 0;
            }
        }

        public int NumberOfGoldenGlobeNominations {
            get {
                return SelectedMovie != null && SelectedMovie.Awards != null
                           ? SelectedMovie.Awards.Count(a => a.Organization == "Golden Globe" && a.IsNomination)
                           : 0;
            }
        }

        public int NumberOfCannesAwards {
            get {
                return SelectedMovie != null && SelectedMovie.Awards != null
                           ? SelectedMovie.Awards.Count(a => a.Organization == "Cannes" && !a.IsNomination)
                           : 0;
            }
        }

        public int NumberOfCannesNominations {
            get {
                return SelectedMovie != null && SelectedMovie.Awards != null
                           ? SelectedMovie.Awards.Count(a => a.Organization == "Cannes" && a.IsNomination)
                           : 0;
            }
        }

        public int NumberOfOscarNominations {
            get {
                return SelectedMovie != null && SelectedMovie.Awards != null
                           ? SelectedMovie.Awards.Count(a => a.Organization == "Oscar" && a.IsNomination)
                           : 0;
            }
        }

        #endregion

        /// <summary>Gets the US MPAA movie rating.</summary>
        /// <value>A string with the MPAA movie rating</value>
        public string MPAARating {
            get {
                ICertification mpaa = null;

                if (SelectedMovie != null && SelectedMovie.Certifications != null) {
                    try {
                        mpaa = SelectedMovie.Certifications.FirstOrDefault(c => c.Country.ISO3166.Alpha3.Equals("usa", StringComparison.OrdinalIgnoreCase));
                    }
                    catch (Exception e) {
                        Console.WriteLine(e);
                    }
                }

                return mpaa != null
                           ? mpaa.Rating
                           : null;
            }
        }

        public string MPAARatingImage {
            get {
                if (SelectedMovie == null) {
                    return null;
                }

                string rating = MPAARating;
                if (!String.IsNullOrEmpty(rating)) {
                    rating = rating.Replace("Rated ", "").ToUpper();
                    string mpaa = null;
                    switch (rating) {
                        case "G":
                            mpaa = "Images/RatingsE/usa/mpaag.png";
                            break;
                        case "NC-17":
                            mpaa = "Images/RatingsE/usa/mpaanc17.png";
                            break;
                        case "PG":
                            mpaa = "Images/RatingsE/usa/mpaapg.png";
                            break;
                        case "PG-13":
                            mpaa = "Images/RatingsE/usa/mpaapg13.png";
                            break;
                        case "R":
                            mpaa = "Images/RatingsE/usa/mpaar.png";
                            break;
                        default:
                            return null;
                    }
                    return String.Format("file://{0}/{1}", Directory.GetCurrentDirectory(), mpaa);
                }
                return null;
            }
        }

        public string FirstFanart {
            get {
                if (SelectedMovie == null) {
                    return null;
                }

                return CheckArtIfValid(SelectedMovie.DefaultFanart);
            }
        }

        //public IArt FirstFanart {
        //    get {
        //        if (SelectedMovie == null) {
        //            return null;
        //        }

        //        return CheckArtIfValid2(SelectedMovie.DefaultFanart);
        //    }
        //}

        public string FirstCoverOrPoster {
            get {
                if (SelectedMovie == null) {
                    return null;
                }
                return CheckArtIfValid(SelectedMovie.DefaultCover);
            }
        }

        public IPlot FirstPlot {
            get {
                if (SelectedMovie == null) {
                    return null;
                }

                return SelectedMovie.MainPlot;
            }
        }

        public string GenreNames {
            get {
                if (SelectedMovie == null || SelectedMovie.Genres == null) {
                    return null;
                }

                return string.Join(", ", SelectedMovie.Genres.Select(g => g.Name));
            }
        }

        #endregion

        #region ICommands

        public ICommand GoToImdbCommand { get; private set; }
        public ICommand GoToTrailerCommand { get; private set; }
        public ICommand ActorImdbClickedCommand { get; private set; }

        #endregion

        private void GoToIMDB(string imdbId) {
            if (!String.IsNullOrEmpty(imdbId)) {
                Process.Start(String.Format(IMDB_TITLE_URI, imdbId));
            }
        }

        private void GoToTrailer(string trailer) {
            if (String.IsNullOrEmpty(trailer)) {
                return;
            }

            if (trailer.StartsWith("http") || trailer.StartsWith("www")) {
                if (Path.HasExtension(trailer)) {
                    string extension = Path.GetExtension(trailer).Trim('.');
                    if (FeatureDetector.VideoExtensions.Contains(extension)) {
                        try {
                            Process.Start("wmplayer", String.Format("\"{0}\"", trailer));
                            return;
                        }
                        catch {
                        }
                    }
                }
            }
            Process.Start(trailer);
        }

        //private static IArt CheckArtIfValid2(IArt art) {
        //    if (art == null) {
        //        return null;
        //    }

        //    string path = art.PreviewOrPath;
        //    if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute)) {
        //        return art;
        //    }

        //    if (!File.Exists(path)) {
        //        return null;
        //    }
        //    return art;
        //}

        private static string CheckArtIfValid(IArt art) {
            if (art == null) {
                return null;
            }

            string path = art.PreviewOrPath;
            if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute)) {
                return art.PreviewOrPath;
            }

            if (!File.Exists(path)) {
                return null;
            }
            return art.PreviewOrPath;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}