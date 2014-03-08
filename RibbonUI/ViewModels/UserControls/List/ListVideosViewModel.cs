using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.Common.Annotations;
using Frost.Models.Frost.DB.Files;
using Frost.XamlControls.Commands;
using RibbonUI.Util;
using RibbonUI.Windows;

namespace RibbonUI.ViewModels.UserControls.List {
    class ListVideosViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICollectionView _collectionView;
        private ObservableHashSet2<Video> _videos;

        public ListVideosViewModel() {
            EditVideoCommand = new RelayCommand<Video>(OnEditClicked, v => v != null);
            RemoveVideoCommand = new RelayCommand<Video>(OnRemoveClicked, v => v != null);
        }

        public Window ParentWindow { get; set; }

        public ObservableHashSet2<Video> Videos {
            get { return _videos; }
            set {
                _videos = value;

                if (_videos == null) {
                    return;
                }

                _collectionView = CollectionViewSource.GetDefaultView(_videos);

                PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
                if (_collectionView.GroupDescriptions != null) {
                    _collectionView.GroupDescriptions.Add(groupDescription);
                }
            }
        }

        public ICommand<Video> EditVideoCommand { get; private set; }
        public ICommand<Video> RemoveVideoCommand { get; private set; }

        private void OnEditClicked(Video selectedVideo) {

            EditVideo editVideo = new EditVideo {
                Owner = ParentWindow,
                DataContext = selectedVideo,
                SelectedLanguage = {
                    ItemsSource = ((CollectionViewSource)ParentWindow.Resources["LanguagesSource"]).View
                }
            };

            editVideo.ShowDialog();

            _collectionView.Refresh();
        }

        private void OnRemoveClicked(Video selectedVideo) {
            
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
