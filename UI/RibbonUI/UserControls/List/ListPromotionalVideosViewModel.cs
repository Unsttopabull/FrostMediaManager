using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.Common.Models.Provider;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {
    class ListPromotionalVideosViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableMovie _selectedMovie;
        private ICollectionView _collectionView;
        private ICommand<IPromotionalVideo> _removeCommand;
        private ICommand<string> _openVideoCommand;

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;
                if (_selectedMovie != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_selectedMovie.PromotionalVideos);
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

        public ICommand<IPromotionalVideo> RemoveCommand {
            get { return _removeCommand ?? (_removeCommand = new RelayCommand<IPromotionalVideo>(RemovePromotionalVideo)); }
            set { _removeCommand = value; }
        }

        public ICommand<string> OpenPromotionalVideoCommand {
            get { return _openVideoCommand ?? (_openVideoCommand = new RelayCommand<string>(OpenVideoCommand)); }
            set { _openVideoCommand = value; }
        }

        private void OpenVideoCommand(string uri) {
            if (!string.IsNullOrEmpty(uri)) {
                try {
                    Process.Start(uri);
                }
                catch {
                    MessageBox.Show(Gettext.T("Error opening video with path: " + uri));
                }
            }
        }

        private void RemovePromotionalVideo(IPromotionalVideo video) {
            SelectedMovie.RemovePromotionalVideo(video);
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
