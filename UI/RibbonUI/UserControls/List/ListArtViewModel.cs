using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {
    public class ListArtViewModel : INotifyPropertyChanged {
        private ObservableCollection<MovieArt> _art;
        private ICollectionView _collectionView;
        public event PropertyChangedEventHandler PropertyChanged;

        public ListArtViewModel() {
            RemoveCommand = new RelayCommand<MovieArt>(RemoveOnClick, art => art != null);
            SetAsDefaultCommand = new RelayCommand<MovieArt>(SetAsDefaultOnClick, art => art != null);
        }

        public ObservableCollection<MovieArt> Art {
            get { return _art; }
            set {
                if (Equals(value, _art)) {
                    return;
                }
                _art = value;

                _collectionView = CollectionViewSource.GetDefaultView(_art);
                if (_collectionView == null) {
                    OnPropertyChanged();
                    return;
                }

                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Type");
                if (_collectionView.GroupDescriptions != null) {
                    _collectionView.GroupDescriptions.Add(groupDescription);
                }

                OnPropertyChanged();
            }
        }

        public ICommand<MovieArt> RemoveCommand { get; private set; }

        public ICommand<MovieArt> SetAsDefaultCommand { get; private set; }

        private void RemoveOnClick(MovieArt art) {
            
        }

        private void SetAsDefaultOnClick(MovieArt art) {
            
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
