using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows {
    class AddCountriesViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private IEnumerable<MovieCountry> _countries;

        public AddCountriesViewModel() {
            AddCommand = new RelayCommand<Window>(w => {
                w.DialogResult = true;
                w.Close();
            });

            CancelCommand = new RelayCommand<Window>(w => {
                w.DialogResult = false;
                w.Close();
            });
        }

        public ICommand<Window> AddCommand { get; private set; }
        public ICommand<Window> CancelCommand { get; private set; }

        public IEnumerable<MovieCountry> Countries {
            get { return _countries; }
            set {
                if (Equals(value, _countries)) {
                    return;
                }
                _countries = value;
                if (_countries != null) {
                    ICollectionView collectionView = CollectionViewSource.GetDefaultView(_countries);
                    collectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }

                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
