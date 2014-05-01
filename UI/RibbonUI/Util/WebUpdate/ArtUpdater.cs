using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Frost.Common.Models;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models;
using RibbonUI.Annotations;
using RibbonUI.Design.Models;

namespace RibbonUI.Util.WebUpdate {

    public class ArtUpdater : INotifyPropertyChanged {
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

        public async Task Update(bool silent = false) {
            IParsedArts parsedArts = null;

            try {
                if (_client.IsImdbSupported && !string.IsNullOrEmpty(_movie.ImdbID)) {
                    ProgressText = "Searching for art by Imdb ID";
                    parsedArts = await Task.Run(() => _client.GetMovieArtFromImdbId(_movie.ImdbID));
                }
                else if (_client.IsTmdbSupported && !string.IsNullOrEmpty(_movie.TmdbID)) {
                    ProgressText = "Searching for art by Tmdb ID";
                    parsedArts = await Task.Run(() => _client.GetMovieArtFromTmdbId(_movie.TmdbID));
                }
                else if (_client.IsTitleSupported) {
                    ProgressText = "Searching for art by Title";
                    parsedArts = await Task.Run(() => _client.GetMovieArtFromTitle(_movie.Title, (int) (_movie.ReleaseYear.HasValue ? _movie.ReleaseYear.Value : 0)));
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
                return;
            }

            if (parsedArts != null) {
                UpdateMovie(parsedArts);
            }
            else {
                MessageBox.Show(TranslationManager.T("No art found"));
            }            
        }

        private void UpdateMovie(IParsedArts parsedArts) {
            LabelText = "Updating movie with art....";
            ProgressText = "Updating ...";

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