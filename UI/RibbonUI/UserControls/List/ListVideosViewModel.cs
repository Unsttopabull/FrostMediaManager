using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Frost.Common;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Design;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows.Edit;

namespace RibbonUI.UserControls.List {
    class ListVideosViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IMoviesDataService _service;

        private ICollectionView _collectionView;
        private ObservableMovie _selectedMovie;

        public ListVideosViewModel() {
            _service = TranslationManager.IsInDesignMode
                ? new DesignMoviesDataService()
                : LightInjectContainer.GetInstance<IMoviesDataService>();


            EditVideoCommand = new RelayCommand<MovieVideo>(OnEditClicked);
            RemoveVideoCommand = new RelayCommand<MovieVideo>(OnRemoveClicked);
        }

        public Window ParentWindow { get; set; }

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                if (_selectedMovie != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_selectedMovie.Videos);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
                    if (_collectionView.GroupDescriptions != null) {
                        _collectionView.GroupDescriptions.Add(groupDescription);
                    }
                }

                OnPropertyChanged("SelectedMovie");
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
            SelectedMovie.RemoveVideo(selectedVideo);
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
