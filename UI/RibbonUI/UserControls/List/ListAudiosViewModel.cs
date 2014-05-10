using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.Common;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Design;
using RibbonUI.Design.Fakes;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows.Edit;

namespace RibbonUI.UserControls.List {

    internal class ListAudiosViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICollectionView _collectionView;
        private readonly IMoviesDataService _service;
        private ObservableMovie _selectedMovie;

        public ListAudiosViewModel() {
            if (Gettext.IsInDesignMode) {
                _service = new DesignMoviesDataService();
                SelectedMovie = new ObservableMovie(new FakeMovie());
            }
            else {
                _service = LightInjectContainer.GetInstance<IMoviesDataService>();
            }

            EditCommand = new RelayCommand<MovieAudio>(OnEditClicked, a => a != null);
            RemoveCommand = new RelayCommand<MovieAudio>(OnRemoveClicked, a => a != null);
        }

        public Window ParentWindow { get; set; }

        public ICommand<MovieAudio> EditCommand { get; private set; }
        public ICommand<MovieAudio> RemoveCommand { get; private set; }

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }

                _selectedMovie = value;

                if (_selectedMovie != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_selectedMovie.Audios);
                    if (_collectionView == null) {
                        OnPropertyChanged();
                        return;
                    }

                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
                    if (_collectionView.GroupDescriptions != null) {
                        _collectionView.GroupDescriptions.Add(groupDescription);
                    }
                }
                OnPropertyChanged();
            }
        }


        private void OnEditClicked(MovieAudio audio) {
            EditAudio editAudio = new EditAudio {
                Owner = ParentWindow,
                SelectedAudio = audio,
            };

            if (_service.Languages != null && _service.Languages.Any()) {
                editAudio.SelectedLanguage.ItemsSource = _service.Languages;
            }
            else {
                editAudio.SelectedLanguage.ItemsSource = UIHelper.LanguagesWithImages;
            }

            editAudio.ShowDialog();

            _collectionView.Refresh();
        }

        private void OnRemoveClicked(MovieAudio audio) {
            SelectedMovie.RemoveAudio(audio);
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