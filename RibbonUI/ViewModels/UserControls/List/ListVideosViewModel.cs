using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.Common.Annotations;
using Frost.Common.Models;
using Frost.XamlControls.Commands;
using RibbonUI.Windows;

namespace RibbonUI.ViewModels.UserControls.List {
    class ListVideosViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICollectionView _collectionView;
        private ObservableCollection<IVideo> _videos;

        public ListVideosViewModel() {
            EditVideoCommand = new RelayCommand<IVideo>(OnEditClicked, v => v != null);
            RemoveVideoCommand = new RelayCommand<IVideo>(OnRemoveClicked, v => v != null);
        }

        public Window ParentWindow { get; set; }

        public ObservableCollection<IVideo> Videos {
            get { return _videos; }
            set {
                _videos = value;

                _collectionView = CollectionViewSource.GetDefaultView(_videos);
                if (_videos == null) {
                    OnPropertyChanged();
                    return;
                }

                PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
                if (_collectionView.GroupDescriptions != null) {
                    _collectionView.GroupDescriptions.Add(groupDescription);
                }
                OnPropertyChanged();
            }
        }

        public ICommand<IVideo> EditVideoCommand { get; private set; }
        public ICommand<IVideo> RemoveVideoCommand { get; private set; }

        private void OnEditClicked(IVideo selectedVideo) {

            EditVideo editVideo = new EditVideo {
                Owner = ParentWindow,
                Video = selectedVideo,
                SelectedLanguage = {
                    ItemsSource = ((CollectionViewSource)ParentWindow.Resources["LanguagesSource"]).View
                }
            };

            editVideo.ShowDialog();

            _collectionView.Refresh();
        }

        private void OnRemoveClicked(IVideo selectedVideo) {
            
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
