using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models;
using RibbonUI.Annotations;
using RibbonUI.Design.Models;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Util.WebUpdate {
    public class PromoVideoUpdater : INotifyPropertyChanged {
        private readonly IPromotionalVideoClient _cli;
        private readonly ObservableMovie _movie;
        public event PropertyChangedEventHandler PropertyChanged;
        private string _progressText;
        private string _labelText;

        public PromoVideoUpdater(IPromotionalVideoClient cli, ObservableMovie movie) {
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

        public async Task Update(bool silent = false) {
            IEnumerable<IParsedVideo> parsedVideos = null;

            try {
                if (_cli.IsImdbSupported && !string.IsNullOrEmpty(_movie.ImdbID)) {
                    ProgressText = "Searching for videos by Imdb ID";
                    parsedVideos = await Task.Run(() => _cli.GetMovieArtFromImdbId(_movie.ImdbID));
                }
                else if (_cli.IsTmdbSupported && !string.IsNullOrEmpty(_movie.TmdbID)) {
                    ProgressText = "Searching for videos by Tmdb ID";
                    parsedVideos = await Task.Run(() => _cli.GetMovieArtFromTmdbId(_movie.TmdbID));
                }
                else if (_cli.IsTitleSupported) {
                    ProgressText = "Searching for videos by Title";
                    parsedVideos = await Task.Run(() => _cli.GetMovieArtFromTitle(_movie.Title, (int) (_movie.ReleaseYear.HasValue ? _movie.ReleaseYear.Value : 0)));
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
                return;
            }

            if (parsedVideos != null) {
                UpdateMovie(parsedVideos);
            }
            else {
                MessageBox.Show(TranslationManager.T("No videos found"));
            }            
        }

        private void UpdateMovie(IEnumerable<IParsedVideo> videos) {
            LabelText = "Updating movie with promotional videos....";
            ProgressText = "Updating ...";

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
