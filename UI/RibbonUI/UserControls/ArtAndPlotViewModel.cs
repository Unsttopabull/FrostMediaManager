﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Frost.Common;
using Frost.DetectFeatures;
using Frost.GettextMarkupExtension;
using Frost.RibbonUI.Design.Fakes;
using Frost.RibbonUI.Properties;
using Frost.RibbonUI.Util.ObservableWrappers;
using Frost.XamlControls.Commands;
using log4net;

namespace Frost.RibbonUI.UserControls {

    public class ArtAndPlotViewModel : INotifyPropertyChanged {
        private const string IMDB_PERSON_URI = "http://www.imdb.com/name/{0}";
        private const string IMDB_TITLE_URI = "http://www.imdb.com/title/{0}";
        private static readonly ILog Log = LogManager.GetLogger(typeof(ArtAndPlotViewModel));
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableMovie _selectedMovie;

        public ArtAndPlotViewModel() {
            if (Gettext.IsInDesignMode) {
                SelectedMovie = new ObservableMovie(new FakeMovie());
            }

            GoToImdbCommand = new RelayCommand<string>(GoToIMDB);

            ActorImdbClickedCommand = new RelayCommand<string>(
                imdbId => {
                    string uri = string.Format(IMDB_PERSON_URI, imdbId);
                    try {
                        Process.Start(uri);
                    }
                    catch (Exception e) {
                        if (Log.IsWarnEnabled) {
                            Log.Warn(string.Format("Failed to open an IMDB person website. Person ImdbID \"{0}\".", imdbId), e);
                        }

                        MessageBox.Show(Gettext.T("Error opening IMDB page with address: ") + uri);
                    }
                },
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

                OnPropertyChanged("MPAARatingImage");
                OnPropertyChanged("BoxImage");

                OnPropertyChanged();
            }
        }

        #region Utility properties

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


        public string MPAARatingImage {
            get {
                if (SelectedMovie == null) {
                    return null;
                }

                string rating = SelectedMovie.MPAARating;
                if (!String.IsNullOrEmpty(rating)) {
                    rating = rating.Replace("Rated ", "").ToUpper();
                    string mpaa;
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

        #endregion

        #region ICommands

        public ICommand GoToImdbCommand { get; private set; }
        public ICommand GoToTrailerCommand { get; private set; }
        public ICommand ActorImdbClickedCommand { get; private set; }

        #endregion

        private void GoToIMDB(string imdbId) {
            if (!string.IsNullOrEmpty(imdbId)) {
                string uri = string.Format(IMDB_TITLE_URI, imdbId);
                try {
                    Process.Start(uri);
                }
                catch (Exception e) {
                    if (Log.IsWarnEnabled) {
                        Log.Warn(string.Format("Failed to open a movie IMDB website with ImdbId \"{0}\".", imdbId), e);
                    }

                    MessageBox.Show(Gettext.T("Error opening IMDB page with address: ") + uri);
                }
            }
        }

        private void GoToTrailer(string trailer) {
            if (string.IsNullOrEmpty(trailer)) {
                return;
            }

            if (trailer.StartsWith("http") || trailer.StartsWith("www")) {
                if (Path.HasExtension(trailer)) {
                    string extension = Path.GetExtension(trailer).Trim('.');
                    if (FeatureDetector.VideoExtensions.Contains(extension)) {
                        try {
                            Process.Start("wmplayer", string.Format("\"{0}\"", trailer));
                            return;
                        }
                        catch (Exception e) {
                            if (Log.IsWarnEnabled) {
                                Log.Warn(string.Format("Failed to open a movie trailer with Windows Media Player and path \"{0}\".", trailer), e);
                            }

                            MessageBox.Show(Gettext.T("Error opening trailer in Windows Media Player"));
                        }
                    }
                }
            }
            try {
                Process.Start(trailer);
            }
            catch (Exception e) {
                if (Log.IsWarnEnabled) {
                    Log.Warn(string.Format("Failed to open a movie trailer with path \"{0}\".", trailer), e);
                }

                MessageBox.Show(Gettext.T("Error opening trailer with path: ") + trailer);
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