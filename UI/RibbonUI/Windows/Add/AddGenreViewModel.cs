using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.Common.Models.Provider;
using Frost.RibbonUI.Properties;
using Frost.XamlControls.Commands;

namespace Frost.RibbonUI.Windows.Add {

    public class AddGenreViewModel : INotifyPropertyChanged {
        private string _newGenreName;
        private IEnumerable<IGenre> _genres;
        public event PropertyChangedEventHandler PropertyChanged;

        public AddGenreViewModel() {
            AddCommand = new RelayCommand<Window>(w => {
                w.DialogResult = true;
                w.Close();
            });
        }

        public IEnumerable<IGenre> Genres {
            get { return _genres; }
            set {
                if (Equals(value, _genres)) {
                    return;
                }
                _genres = value;

                if (_genres != null) {
                    try {
                        ICollectionView collectionView = CollectionViewSource.GetDefaultView(_genres);
                        collectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    }
                    catch {
                    }
                }

                OnPropertyChanged();
            }
        }

        public string NewGenreName {
            get { return _newGenreName; }
            set {
                if (value == _newGenreName) {
                    return;
                }
                _newGenreName = value;

                OnPropertyChanged();
            }
        }

        public ICommand<Window> AddCommand { get; private set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}