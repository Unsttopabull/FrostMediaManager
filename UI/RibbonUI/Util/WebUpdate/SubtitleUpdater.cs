using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models.Subtitles;
using RibbonUI.Annotations;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Util.WebUpdate {
    public class SubtitleUpdater : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ISubtitleClient _cli;
        private readonly ObservableMovie _movie;
        private string _progressText;
        private string _labelText;

        public SubtitleUpdater(ISubtitleClient cli, ObservableMovie movie) {
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

        public async Task<IEnumerable<ISubtitleInfo>> Update(IEnumerable<string> languages, bool silent = false) {
            IEnumerable<ISubtitleInfo> subtitles;
            try {
                subtitles = await GetSubtitleInfo(languages, silent);
            }
            catch (Exception e) {
                if (!silent) {
                    MessageBox.Show(e.Message);
                }
                return null;
            }

            if (subtitles == null) {
                if (!silent) {
                    MessageBox.Show(Gettext.T("No subtitles found"));
                }
                return null;
            }

            return subtitles;
        }

        private async Task<IEnumerable<ISubtitleInfo>> GetSubtitleInfo(IEnumerable<string> languages, bool silent) {
            if (_cli.IsMovieHashSupported) {
                if (_movie.MovieHashes != null && _movie.MovieHashes.Any()) {
                    try {
                        var subs = await Task.Run(() => _cli.GetSubtitlesByMovieHash(_movie.MovieHashesInfo, languages));
                        if (subs != null) {
                            return subs;
                        }
                    }
                    catch (Exception e) {
                    }
                }
            }

            if (_cli.IsImdbSupported && !string.IsNullOrEmpty(_movie.ImdbID)) {
                if (!silent) {
                    ProgressText = "Searching for videos by Imdb ID";
                }

                try {
                    return await Task.Run(() => _cli.GetMovieSubtitlesFromImdbId(_movie.ImdbID, languages));
                }
                catch (Exception e) {
                    return null;
                }
            }

            if (_cli.IsTitleSupported) {
                if (!silent) {
                    ProgressText = "Searching for videos by Title";
                }
                try {
                    return await Task.Run(() => _cli.GetMovieSubtitlesFromTitle(_movie.Title, (int) (_movie.ReleaseYear.HasValue ? _movie.ReleaseYear.Value : 0), languages));
                }
                catch (Exception e) {
                    return null;
                }
            }

            List<string> lst = new List<string>();
            if (_cli.IsMovieHashSupported) {
                lst.Add("Movie Hash");
            }

            if (_cli.IsImdbSupported) {
                lst.Add("Imdb ID");
            }

            if (_cli.IsTitleSupported) {
                lst.Add("Movie title");
            }

            MessageBox.Show(string.Format("No required info found. Plugin requires : {0}", string.Join(" or ", lst)));
            return null;
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
