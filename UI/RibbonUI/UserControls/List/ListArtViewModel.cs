using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {
    public class ListArtViewModel : INotifyPropertyChanged {
        private ObservableCollection<MovieArt> _art;
        public event PropertyChangedEventHandler PropertyChanged;

        public ListArtViewModel() {
            RemoveCommand = new RelayCommand<MovieArt>(RemoveOnClick, art => art != null);
            AddCommand = new RelayCommand<MovieArt>(AddOnClick, art => art != null);
        }

        public ObservableCollection<MovieArt> Art {
            get { return _art; }
            set {
                if (Equals(value, _art)) {
                    return;
                }
                _art = value;
                OnPropertyChanged();
            }
        }

        public ICommand<MovieArt> RemoveCommand { get; private set; }

        public ICommand<MovieArt> AddCommand { get; private set; }

        private void RemoveOnClick(MovieArt art) {
            
        }

        private void AddOnClick(MovieArt art) {
            
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
