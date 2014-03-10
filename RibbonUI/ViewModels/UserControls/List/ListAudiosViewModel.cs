using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;
using RibbonUI.Windows;

namespace RibbonUI.ViewModels.UserControls.List {
    class ListAudiosViewModel : INotifyPropertyChanged {
        private ObservableCollection<IAudio> _audios;
        public event PropertyChangedEventHandler PropertyChanged;
        private ICollectionView _collectionView;
        private readonly IMoviesDataService _service;

        public ListAudiosViewModel(IMoviesDataService service) {
            _service = service;

            EditCommand = new RelayCommand<IAudio>(OnEditClicked, a => a != null);
            RemoveCommand = new RelayCommand<IAudio>(OnRemoveClicked, a => a != null);
        }

        public ObservableCollection<IAudio> Audios {
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

        public ICommand<IAudio> EditCommand { get; private set; }
        public ICommand<IAudio> RemoveCommand { get; private set; }


        private void OnEditClicked(IAudio audio) {
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

        private void OnRemoveClicked(IAudio audio) {
            
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
