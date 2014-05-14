using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Frost.Common.Models;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models;
using Frost.InfoParsers.Models.Info;
using Frost.RibbonUI.Design.Models;
using Frost.RibbonUI.Properties;
using log4net;

namespace Frost.RibbonUI.Util.WebUpdate {
    public class PromoVideoUpdater : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PromoVideoUpdater));
        private readonly IPromotionalVideoClient _cli;
        private readonly IMovieInfo _movie;
        public event PropertyChangedEventHandler PropertyChanged;
        private string _progressText;
        private string _labelText;

        public PromoVideoUpdater(IPromotionalVideoClient cli, IMovieInfo movie) {
            _cli = cli;
            _movie = movie;
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
            IEnumerable<IParsedVideo> parsedVideos;

            try {
                if (_cli.IsImdbSupported && !string.IsNullOrEmpty(_movie.ImdbID)) {
                    if (!silent) {
                        ProgressText = Gettext.T("Searching for videos by Imdb ID");
                    }
                    parsedVideos = await Task.Run(() => _cli.GetMovieVideosFromImdbId(_movie.ImdbID));
                }
                else if (_cli.IsTmdbSupported && !string.IsNullOrEmpty(_movie.TmdbID)) {
                    if (!silent) {
                        ProgressText = Gettext.T("Searching for videos by Tmdb ID");
                    }
                    parsedVideos = await Task.Run(() => _cli.GetMovieVideosFromTmdbId(_movie.TmdbID));
                }
                else if (_cli.IsTitleSupported) {
                    if (!silent) {
                        ProgressText = Gettext.T("Searching for videos by Title");
                    }
                    parsedVideos = await Task.Run(() => _cli.GetMovieVideosFromTitle(_movie.Title, (int) (_movie.ReleaseYear.HasValue ? _movie.ReleaseYear.Value : 0)));
                }
                else {
                    List<string> lst = new List<string>();
                    if (_cli.IsImdbSupported) {
                        lst.Add("Imdb ID");
                    }

                    if (_cli.IsTmdbSupported) {
                        lst.Add("Tmdb ID");
                    }

                    if (_cli.IsTitleSupported) {
                        lst.Add(Gettext.T("Movie title"));
                    }

                    MessageBox.Show(string.Format(Gettext.T("No required info found. Plugin requires:") + "{0}", string.Join(Gettext.T(" or "), lst)));
                    return false;
                }
            }
            catch (WebException e) {
                if (Log.IsErrorEnabled) {
                    Log.Error(string.Format("Exception occured while downloading promotional videos for movie \"{0}\" with plugin \"{1}\".", _movie.Title, _cli.Name));
                }

                if (!silent) {
                    MessageBox.Show(e.Message);
                }
                return false;                
            }
            catch (Exception e) {
                if (Log.IsErrorEnabled) {
                    Log.Error(string.Format("Unknown exception occured while getting promotional videos for movie \"{0}\" with plugin \"{1}\".", _movie.Title, _cli.Name));
                }

                if (!silent) {
                    MessageBox.Show(e.Message);
                }
                return false;
            }

            if (parsedVideos == null) {
                if (!silent) {
                    MessageBox.Show(Gettext.T("No videos found"));
                }
                return false;
            }

            UpdateMovie(parsedVideos, silent);
            return true;
        }

        private void UpdateMovie(IEnumerable<IParsedVideo> videos, bool silent) {
            if (!silent) {
                LabelText = "Updating movie with promotional videos....";
                ProgressText = "Updating ...";
            }

            foreach (IParsedVideo parsedVideo in videos) {
                _movie.AddPromotionalVideo(new DesignPromotionalVideo(parsedVideo), true);
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
