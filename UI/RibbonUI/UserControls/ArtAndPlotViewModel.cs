using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Frost.Common;
using Frost.Common.Models;
using Frost.XamlControls.Commands;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using RibbonUI.Annotations;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    public class ArtAndPlotViewModel : INotifyPropertyChanged {
        private const string IMDB_PERSON_URI = "http://www.imdb.com/name/nm{0}";
        private const string IMDB_TITLE_URI = "http://www.imdb.com/title/{0}";
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableMovie _selectedMovie;

        public ArtAndPlotViewModel() {
            GoToImdbCommand = new RelayCommand<string>(GoToIMDB);

            ActorImdbClickedCommand = new RelayCommand<string>(
                imdbId => Process.Start(string.Format(IMDB_PERSON_URI, imdbId)),
                s => !string.IsNullOrEmpty(s)
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

                //if (_selectedMovie != null) {
                //    Actors = _selectedMovie.Actors;
                //}

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

                OnPropertyChanged();
            }
        }

        #region Utility properties

        //public IEnumerable<IActor> Actors { get; set; }

        public string BoxImage {
            get {
                string path;
                if (SelectedMovie == null || (SelectedMovie != null && SelectedMovie.Type == MovieType.Unknown)) {
                    path = "Images/Boxes/generic.png";
                }
                else {
                    path = "Images/Boxes/" + SelectedMovie.Type + ".png";
                }
                return Path.Combine(Directory.GetCurrentDirectory(), path);
                ;
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
                if (!string.IsNullOrEmpty(rating)) {
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
                    return string.Format("file://{0}/{1}", Directory.GetCurrentDirectory(), mpaa);
                }
                return null;
            }
        }

        public string FirstFanart {
            get {
                if (SelectedMovie == null || SelectedMovie.Art == null) {
                    return null;
                }

                if (SelectedMovie.Art.Any(a => a.Type == ArtType.Fanart)) {
                    IArt v = SelectedMovie.Art.FirstOrDefault(a => a.Type == ArtType.Fanart && !string.IsNullOrEmpty(a.PreviewOrPath));
                    if (v != null) {
                        return CheckIfValid(v);
                    }
                }

                return null;
            }
        }

        public string FirstCoverOrPoster {
            get {
                if (SelectedMovie == null || SelectedMovie.Art == null) {
                    return null;
                }

                IArt art;
                if (SelectedMovie.Art.Any(a => a.Type == ArtType.Cover)) {
                    art = SelectedMovie.Art.FirstOrDefault(a => a.Type == ArtType.Cover && !string.IsNullOrEmpty(a.PreviewOrPath));
                    if (art != null) {
                        return CheckIfValid(art);
                    }
                }

                if (SelectedMovie.Art.All(a => a.Type != ArtType.Poster)) {
                    return null;
                }

                art = SelectedMovie.Art.FirstOrDefault(a => a.Type == ArtType.Poster);
                if (art != null) {
                    return CheckIfValid(art);
                }
                return null;
            }
        }

        public IPlot FirstPlot {
            get {
                if (SelectedMovie == null || SelectedMovie.Plots == null) {
                    return null;
                }

                return SelectedMovie.Plots.Any()
                           ? SelectedMovie.Plots.FirstOrDefault()
                           : null;
            }
        }

        #endregion

        #region ICommands

        public ICommand GoToImdbCommand { get; private set; }
        public ICommand GoToTrailerCommand { get; private set; }
        public ICommand ActorImdbClickedCommand { get; private set; }

        #endregion

        private void GoToIMDB(string imdbId) {
            if (!string.IsNullOrEmpty(imdbId)) {
                Process.Start(string.Format(IMDB_TITLE_URI, imdbId));
            }
        }

        private void GoToTrailer(string trailer) {
            if (!string.IsNullOrEmpty(trailer)) {
                Process.Start(trailer);
            }
        }

        private static string CheckIfValid(IArt art) {
            string path = art.PreviewOrPath;
            if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute)) {
                return art.PreviewOrPath;
            }

            if (!File.Exists(path)) {
                return null;
            }
            else {
                return art.PreviewOrPath;
            }
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