using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Frost.Common.Models;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;
using Microsoft.Win32;

namespace RibbonUI.Windows {
    class AddPersonViewModel : INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IDisposable _searchTextObservable;
        private string _searchText;
        private IEnumerable<IPerson> _people;
        private ICollectionView _collectionView;
        private IPerson _selectedPerson;
        private string _personName;
        private string _personThumb;
        private string _personCharacter;

        public AddPersonViewModel() {
            //_selectedPerson = LightInjectContainer.GetInstance<IPerson>(App.SystemType);
            _people = new List<IPerson>();
            _searchText = "";

            ThumbSearchCommand = new RelayCommand(ThumbSearch);
            AddCommand = new RelayCommand<Window>(w => {
                w.DialogResult = true;
                w.Close();
            });
            CancelCommand = new RelayCommand<Window>(w => {
                w.DialogResult = false;
                w.Close();
            });

            _searchTextObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                                              .Where(ep => ep.EventArgs.PropertyName == "SearchText")
                                              .Throttle(TimeSpan.FromSeconds(0.5))
                                              .ObserveOn(SynchronizationContext.Current)
                                              .Subscribe(obj => {
                                                  if (_collectionView != null) {
                                                      _collectionView.Refresh();
                                                  }
                                              });
        }

        public IEnumerable<IPerson> People {
            get { return _people; }
            set {
                if (Equals(value, _people)) {
                    return;
                }
                _people = value;

                
                if (_people != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_people);

                    if (_collectionView != null) {
                        _collectionView.Filter = Filter;
                    }
                }
                OnPropertyChanged();
            }
        }

        public IPerson SelectedPerson {
            get { return _selectedPerson; }
            set {
                if (Equals(value, _selectedPerson)) {
                    return;
                }

                _selectedPerson = value;
                if (_selectedPerson != null) {
                    IsThumbEditable = _selectedPerson["Thumb"];
                    IsThumbEditable = _selectedPerson["Character"];

                    PersonName = _selectedPerson.Name;
                    PersonThumb = _selectedPerson.Thumb;
                }
                OnPropertyChanged();
            }
        }

        public bool IsThumbEditable { get; private set; }

        public string PersonName {
            get { return _personName; }
            set {
                if (value == _personName) {
                    return;
                }

                _personName = value;

                if (!string.IsNullOrEmpty(_personName)) {
                    IPerson person = _people.FirstOrDefault(p => p.Name.Equals(_personName, StringComparison.CurrentCultureIgnoreCase));
                    if (person != null) {
                        SelectedPerson = person;
                    }
                }

                OnPropertyChanged();
            }
        }

        public string PersonThumb {
            get { return _personThumb; }
            set {
                if (value == _personThumb) {
                    return;
                }

                _personThumb = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        public string PersonCharacter {
            get { return _personCharacter; }
            set {
                if (value == _personCharacter) {
                    return;
                }
                _personCharacter = value;
                OnPropertyChanged();
            }
        }

        public string SearchText {
            get { return _searchText; }
            set {
                if (value == _searchText) {
                    return;
                }
                _searchText = value;
                OnPropertyChanged();
            }
        }

        #region ICommands

        public ICommand<Window> AddCommand { get; private set; }

        public ICommand<Window> CancelCommand { get; private set; }

        public ICommand ThumbSearchCommand { get; private set; }

        #endregion

        private bool Filter(object obj) {
            if (SearchText == null) {
                return false;
            }

            IPerson p = (IPerson) obj;
            return p.Name.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void ThumbSearch() {
            OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Multiselect = false };

            if (ofd.ShowDialog() == true) {
                PersonThumb = ofd.FileName;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        #region IDisposable

        public bool IsDisposed { get; private set; }

        public void Dispose() {
            Dispose(false);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        private void Dispose(bool finalizer) {
            if (!IsDisposed) {
                if (_searchTextObservable != null) {
                    _searchTextObservable.Dispose();
                }

                if (!finalizer) {
                    GC.SuppressFinalize(this);
                }
                IsDisposed = true;
            }
        }

        ~AddPersonViewModel() {
            Dispose(true);
        }

        #endregion

    }
}
