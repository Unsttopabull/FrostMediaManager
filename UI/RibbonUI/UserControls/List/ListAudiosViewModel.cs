using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    class ListAudiosViewModel : INotifyPropertyChanged {
        private ObservableCollection<MovieAudio> _audios;
        public event PropertyChangedEventHandler PropertyChanged;
        private ICollectionView _collectionView;
        private readonly IMoviesDataService _service;

        public ListAudiosViewModel() {
            if (TranslationManager.IsInDesignMode) {
                LightInjectContainer.Register<IMoviesDataService, DesignMoviesDataService>();
            }

            _service = LightInjectContainer.GetInstance<IMoviesDataService>();

            if (TranslationManager.IsInDesignMode) {
                Audios = new ObservableCollection<MovieAudio>(_service.Audios.Select(a => new MovieAudio(a)));
            }

            EditCommand = new RelayCommand<MovieAudio>(OnEditClicked, a => a != null);
            RemoveCommand = new RelayCommand<MovieAudio>(OnRemoveClicked, a => a != null);
        }

        public ObservableCollection<MovieAudio> Audios {
            get { return _audios; }
            set {
                if (Equals(value, _audios)) {
                    return;
                }
                _audios = value;

                _collectionView = CollectionViewSource.GetDefaultView(_audios);
                if (_collectionView == null) {
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

        public Window ParentWindow { get; set; }

        public ICommand<MovieAudio> EditCommand { get; private set; }
        public ICommand<MovieAudio> RemoveCommand { get; private set; }


        private void OnEditClicked(MovieAudio audio) {
            EditAudio editAudio = new EditAudio {
                Owner = ParentWindow,
                SelectedAudio = audio,
                SelectedLanguage = {
                    ItemsSource = _service.Languages
                }
            };

            editAudio.ShowDialog();

            _collectionView.Refresh();
        }

        private void OnRemoveClicked(MovieAudio audio) {
            
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
