using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Frost.Common;
using Frost.Common.Properties;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Design;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows;

namespace RibbonUI.UserControls.List {
    class ListVideosViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IMoviesDataService _service;

        private ICollectionView _collectionView;
        private ObservableCollection<MovieVideo> _videos;

        public ListVideosViewModel() {
            _service = TranslationManager.IsInDesignMode
                ? new DesignMoviesDataService()
                : LightInjectContainer.GetInstance<IMoviesDataService>();


            EditVideoCommand = new RelayCommand<MovieVideo>(OnEditClicked); //, v => v != null);
            RemoveVideoCommand = new RelayCommand<MovieVideo>(OnRemoveClicked); //, v => v != null);
        }

        public Window ParentWindow { get; set; }

        public ObservableCollection<MovieVideo> Videos {
            get { return _videos; }
            set {
                _videos = value;

                if (_videos == null) {
                    OnPropertyChanged("Videos");
                    return;
                }

                _collectionView = CollectionViewSource.GetDefaultView(_videos);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
                if (_collectionView.GroupDescriptions != null) {
                    _collectionView.GroupDescriptions.Add(groupDescription);
                }
                OnPropertyChanged("Videos");
            }
        }

        public ICommand<MovieVideo> EditVideoCommand { get; private set; }
        public ICommand<MovieVideo> RemoveVideoCommand { get; private set; }

        private void OnEditClicked(MovieVideo selectedVideo) {

            EditVideo editVideo = new EditVideo {
                Owner = ParentWindow,
                Video = selectedVideo,
                SelectedLanguage = {
                    ItemsSource = _service.Languages
                }
            };

            editVideo.ShowDialog();

            OnPropertyChanged("Videos");
        }

        private void OnRemoveClicked(MovieVideo selectedVideo) {
            
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
