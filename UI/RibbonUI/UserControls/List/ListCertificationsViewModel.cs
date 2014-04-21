using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {
    public class ListCertificationsViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableMovie _selectedMovie;

        public ListCertificationsViewModel() {
            RemoveCommand = new RelayCommand<MovieCertification>(OnCertificationRemove, cert => cert != null);
        }

        public ICommand<MovieCertification> RemoveCommand { get; private set; }

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

        private void OnCertificationRemove(MovieCertification obj) {
            
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
