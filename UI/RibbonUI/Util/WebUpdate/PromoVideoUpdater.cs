using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Frost.Common.Models;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models;
using Frost.InfoParsers.Models.Info;
using RibbonUI.Annotations;
using RibbonUI.Design.Models;

namespace RibbonUI.Util.WebUpdate {
    public class PromoVideoUpdater : INotifyPropertyChanged {
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
            IEnumerable<IParsedVideo> parsedVideos = null;

            try {
                if (_cli.IsImdbSupported && !string.IsNullOrEmpty(_movie.ImdbID)) {
                    if (!silent) {
                        ProgressText = "Searching for videos by Imdb ID";
                    }
                    parsedVideos = await Task.Run(() => _cli.GetMovieVideosFromImdbId(_movie.ImdbID));
                }
                else if (_cli.IsTmdbSupported && !string.IsNullOrEmpty(_movie.TmdbID)) {
                    if (!silent) {
                        ProgressText = "Searching for videos by Tmdb ID";
                    }
                    parsedVideos = await Task.Run(() => _cli.GetMovieVideosFromTmdbId(_movie.TmdbID));
                }
                else if (_cli.IsTitleSupported) {
                    if (!silent) {
                        ProgressText = "Searching for videos by Title";
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
                        lst.Add("Movie title");
                    }

                    MessageBox.Show(string.Format("No required info found. Plugin requires : {0}", string.Join(" or ", lst)));
                    return false;
                }
            }
            catch (Exception e) {
                if (!silent) {
                    MessageBox.Show(e.Message);
                }
                return false;
            }

            if (parsedVideos == null) {
                if (!silent) {
                    MessageBox.Show(TranslationManager.T("No videos found"));
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
