using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Frost.Common.Models;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models.Art;
using Frost.InfoParsers.Models.Info;
using log4net;
using RibbonUI.Annotations;
using RibbonUI.Design.Models;

namespace RibbonUI.Util.WebUpdate {

    public class ArtUpdater : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ArtUpdater));
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IFanartClient _client;
        private readonly IMovieInfo _movie;
        private string _progressText;
        private string _labelText;

        public ArtUpdater(IFanartClient client, IMovieInfo movie) {
            _movie = movie;
            _client = client;
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

        public async Task<bool> Update(bool silent = false) {
            IParsedArts parsedArts = null;

            try {
                if (_client.IsImdbSupported && !string.IsNullOrEmpty(_movie.ImdbID)) {
                    if (!silent) {
                        ProgressText = "Searching for art by Imdb ID";
                    }
                    parsedArts = await Task.Run(() => _client.GetMovieArtFromImdbId(_movie.ImdbID));
                }
                else if (_client.IsTmdbSupported && !string.IsNullOrEmpty(_movie.TmdbID)) {
                    if (!silent) {
                        ProgressText = "Searching for art by Tmdb ID";
                    }
                    parsedArts = await Task.Run(() => _client.GetMovieArtFromTmdbId(_movie.TmdbID));
                }
                else if (_client.IsTitleSupported) {
                    if (!silent) {
                        ProgressText = "Searching for art by Title";
                    }
                    parsedArts = await Task.Run(() => _client.GetMovieArtFromTitle(_movie.Title, (int) (_movie.ReleaseYear.HasValue ? _movie.ReleaseYear.Value : 0)));
                }
                else {
                    List<string> lst = new List<string>();
                    if (_client.IsImdbSupported) {
                        lst.Add("Imdb ID");
                    }

                    if (_client.IsTmdbSupported) {
                        lst.Add("Tmdb ID");
                    }

                    if (_client.IsTitleSupported) {
                        lst.Add(Gettext.T("Movie title"));
                    }

                    MessageBox.Show(string.Format(Gettext.T("No required info found. Plugin requires:")+ "{0}", string.Join(Gettext.T(" or "), lst)));
                    return false;
                }
            }
            catch (Exception e) {
                if (Log.IsErrorEnabled) {
                    Log.Error("Error has occured wile getting art info.", e);
                }

                if (!silent) {
                    MessageBox.Show(e.Message);
                }
                return false;
            }

            if (parsedArts == null) {
                if (!silent) {
                    MessageBox.Show(Gettext.T("No art found"));
                }
                return false;
            }

            UpdateMovie(parsedArts, silent);
            return true;
        }

        private void UpdateMovie(IParsedArts parsedArts, bool silent) {
            if (!silent) {
                LabelText = Gettext.T("Updating movie with art....");
                ProgressText = Gettext.T("Updating ...");
            }

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}