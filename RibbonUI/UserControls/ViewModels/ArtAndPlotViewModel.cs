using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frost.Common.Annotations;
using Frost.Models.Frost.DB;
using Frost.XamlControls.Commands;

namespace RibbonUI.UserControls.ViewModels {
    public class ArtAndPlotViewModel : INotifyPropertyChanged {
        private const string IMDB_PERSON_URI = "http://www.imdb.com/name/nm{0}";
        private const string IMDB_TITLE_URI = "http://www.imdb.com/title/{0}";
        public event PropertyChangedEventHandler PropertyChanged;
        private Movie _selectedMovie;

        public ArtAndPlotViewModel() {
            GoToImdbCommand = new RelayCommand<string>(GoToIMDB);

            ActorImdbClickedCommand = new RelayCommand<string>(
                imdbId => Process.Start(string.Format(IMDB_PERSON_URI, imdbId)),
                s => !string.IsNullOrEmpty(s)
                );

            GoToTrailerCommand = new RelayCommand<string>(GoToTrailer);
        }

        public Movie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoToImdbCommand { get; private set; }
        public ICommand GoToTrailerCommand { get; private set; }
        public ICommand ActorImdbClickedCommand { get; private set; }

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
