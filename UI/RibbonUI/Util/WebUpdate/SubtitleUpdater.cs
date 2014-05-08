using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models.Subtitles;
using log4net;
using RibbonUI.Annotations;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Util.WebUpdate {

    public class SubtitleUpdater : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SubtitleUpdater));
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
                HandleException(e, "");
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

        private void HandleException(Exception e, string arg, bool silent = false) {
            if (e is WebException) {
                if (Log.IsWarnEnabled) {
                    Log.Warn(string.Format("Error downloading subtitle data {0} with plugin {1}.", arg, _cli.Name), e);
                }
                if (!silent) {
                    MessageBox.Show("An error has occured downloading subtitle information.");
                }

                return;
            }

            if (Log.IsErrorEnabled) {
                Log.Error(string.Format("Unknown error has occured while getting subtitle info {0} from plugin \"{1}\".", arg, _cli.Name), e);

                if (!silent) {
                    MessageBox.Show("An error has occured getting subtitle information.");
                }
            }
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
                        HandleException(e, "by MovieHash", true);
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
                    HandleException(e, string.Format("by Imdb [{0}]", _movie.ImdbID), true);
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
                    HandleException(e, string.Format("by movie Title [{0}]", _movie.ImdbID), true);
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