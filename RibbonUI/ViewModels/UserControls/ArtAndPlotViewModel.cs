using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;

namespace RibbonUI.ViewModels.UserControls {
    public class ArtAndPlotViewModel : INotifyPropertyChanged {
        private const string IMDB_PERSON_URI = "http://www.imdb.com/name/nm{0}";
        private const string IMDB_TITLE_URI = "http://www.imdb.com/title/{0}";
        public event PropertyChangedEventHandler PropertyChanged;
        private IMovie _selectedMovie;

        public ArtAndPlotViewModel() {
            GoToImdbCommand = new RelayCommand<string>(GoToIMDB);

            ActorImdbClickedCommand = new RelayCommand<string>(
                imdbId => Process.Start(string.Format(IMDB_PERSON_URI, imdbId)),
                s => !string.IsNullOrEmpty(s)
            );

            GoToTrailerCommand = new RelayCommand<string>(GoToTrailer);
        }

        public IMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

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

                OnPropertyChanged();
            }
        }

        #region Utility properties

        #region Awards

        public int NumberOfOscarsWon {
            get {
                if (SelectedMovie == null) {
                    return 0;
                }
                return SelectedMovie.Awards.Count(a => a.Organization == "Oscar" && !a.IsNomination);
            }
        }

        public int NumberOfGoldenGlobesWon {
            get {
                if (SelectedMovie == null) {
                    return 0;
                }
                return SelectedMovie.Awards.Count(a => a.Organization == "Golden Globe" && !a.IsNomination);
            }
        }

        public int NumberOfGoldenGlobeNominations {
            get {
                if (SelectedMovie == null) {
                    return 0;
                }
                return SelectedMovie.Awards.Count(a => a.Organization == "Golden Globe" && a.IsNomination);
            }
        }

        public int NumberOfCannesAwards {
            get {
                if (SelectedMovie == null) {
                    return 0;
                }
                return SelectedMovie.Awards.Count(a => a.Organization == "Cannes" && !a.IsNomination);
            }
        }

        public int NumberOfCannesNominations {
            get {
                if (SelectedMovie == null) {
                    return 0;
                }
                return SelectedMovie.Awards.Count(a => a.Organization == "Cannes" && a.IsNomination);
            }
        }

        public int NumberOfOscarNominations {
            get {
                if (SelectedMovie == null) {
                    return 0;
                }
                return SelectedMovie.Awards.Count(a => a.Organization == "Oscar" && a.IsNomination);
            }
        }

        #endregion

        /// <summary>Gets the US MPAA movie rating.</summary>
        /// <value>A string with the MPAA movie rating</value>
        public string MPAARating {
            get {
                ICertification mpaa = null;
                try {
                    mpaa = mpaa = SelectedMovie.Certifications.FirstOrDefault(c => c.Country.Name.Equals("United States", StringComparison.OrdinalIgnoreCase));
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }

                if (mpaa != null) {
                    return mpaa.Rating;
                }
                return null;
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
                    switch (rating) {
                        case "G":
                            return "Images/RatingsE/usa/mpaag.png";
                        case "NC-17":
                            return "Images/RatingsE/usa/mpaanc17.png";
                        case "PG":
                            return "Images/RatingsE/usa/mpaapg.png";
                        case "PG-13":
                            return "Images/RatingsE/usa/mpaapg13.png";
                        case "R":
                            return "Images/RatingsE/usa/mpaar.png";
                    }
                }
                return null;
            }
        }

        public IArt FirstFanart {
            get {
                if (SelectedMovie == null) {
                    return null;
                }

                return SelectedMovie.Art.Any(a => a.Type == ArtType.Fanart)
                           ? SelectedMovie.Art.FirstOrDefault(a => a.Type == ArtType.Fanart)
                           : null;
            }
        }

        public IArt FirstCoverOrPoster {
            get {
                if (SelectedMovie == null) {
                    return null;
                }

                if (SelectedMovie.Art.Any(a => a.Type == ArtType.Cover)) {
                    return SelectedMovie.Art.FirstOrDefault(a => a.Type == ArtType.Cover);
                }

                if (SelectedMovie.Art.Any(a => a.Type == ArtType.Poster)) {
                    return SelectedMovie.Art.FirstOrDefault(a => a.Type == ArtType.Poster);
                }
                return null;
            }
        }

        public IPlot FirstPlot {
            get {
                if (SelectedMovie == null) {
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
