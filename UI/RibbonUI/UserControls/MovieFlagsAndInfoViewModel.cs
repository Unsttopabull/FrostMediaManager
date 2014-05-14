using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frost.RibbonUI.Properties;
using Frost.RibbonUI.Util.ObservableWrappers;
using Frost.XamlControls.Commands;

namespace Frost.RibbonUI.UserControls {

    public class MovieFlagsAndInfoViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableMovie _selectedMovie;

        public MovieFlagsAndInfoViewModel() {
            OnSizeChangedCommand = new RelayCommand<MovieFlagsAndInfo>(
                flags => { flags.MinRequiredWidth = flags.MovieFlags.RenderSize.Width + flags.MovieInfo.RenderSize.Width; }
            );
        }

        public ICommand OnSizeChangedCommand { get; private set; }

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                OnPropertyChanged();
            }
        }

        public string OriginalTitle {
            get {
                if (SelectedMovie != null &&
                    SelectedMovie.OriginalTitle != null &&
                    !SelectedMovie.OriginalTitle.Equals(SelectedMovie.Title))
                {
                    return SelectedMovie.OriginalTitle;
                }
                return null;
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