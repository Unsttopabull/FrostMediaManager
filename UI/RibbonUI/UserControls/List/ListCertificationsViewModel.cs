using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {
    public class ListCertificationsViewModel : INotifyPropertyChanged {
        private ObservableCollection<MovieCertification> _certifications;
        public event PropertyChangedEventHandler PropertyChanged;

        public ListCertificationsViewModel() {
            RemoveCommand = new RelayCommand<MovieCertification>(OnCertificationRemove, cert => cert != null);
        }

        public ObservableCollection<MovieCertification> Certifications {
            get { return _certifications; }
            set {
                if (Equals(value, _certifications)) {
                    return;
                }
                _certifications = value;
                OnPropertyChanged();
            }
        }

        public ICommand<MovieCertification> RemoveCommand { get; private set; }

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
