using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Frost.Common;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {

    public class ListArtViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICollectionView _collectionView;
        private ObservableMovie _selectedMovie;

        public ListArtViewModel() {
            RemoveCommand = new RelayCommand<MovieArt>(RemoveOnClick, art => art != null);
            SetAsDefaultCommand = new RelayCommand<MovieArt>(SetAsDefaultOnClick, CheckArt);
        }

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                if (_selectedMovie != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_selectedMovie.Art);
                    if (_collectionView != null) {
                        PropertyGroupDescription groupDescription = new PropertyGroupDescription("Type");
                        if (_collectionView.GroupDescriptions != null) {
                            _collectionView.GroupDescriptions.Add(groupDescription);
                        }
                    }
                }

                OnPropertyChanged();
            }
        }

        public ICommand<MovieArt> RemoveCommand { get; private set; }

        public ICommand<MovieArt> SetAsDefaultCommand { get; private set; }


        private bool CheckArt(MovieArt art) {
            bool b = SelectedMovie != null && art != null;

            if (!b || art.ObservedEntity.Id <= 0) {
                return b;
            }

            switch (art.Type) {
                case ArtType.Fanart:
                    if (SelectedMovie.DefaultFanart == null) {
                        return true;
                    }
                    return art.ObservedEntity.Id != SelectedMovie.DefaultFanart.ObservedEntity.Id;
                case ArtType.Cover:
                case ArtType.Poster:
                    if (SelectedMovie.DefaultCover == null) {
                        return true;
                    }
                    return art.ObservedEntity.Id != SelectedMovie.DefaultCover.ObservedEntity.Id;
                default:
                    return true;
            }
        }

        private void RemoveOnClick(MovieArt art) {
            SelectedMovie.RemoveArt(art);
        }

        private void SetAsDefaultOnClick(MovieArt art) {
            switch (art.Type) {
                case ArtType.Fanart:
                    SelectedMovie.DefaultFanart = art;
                    break;
                case ArtType.Poster:
                case ArtType.Cover:
                    SelectedMovie.DefaultCover = art;
                    break;
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