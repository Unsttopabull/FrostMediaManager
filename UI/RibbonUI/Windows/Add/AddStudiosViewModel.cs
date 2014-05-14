using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using Frost.RibbonUI.Properties;
using Frost.RibbonUI.Util.ObservableWrappers;
using Frost.XamlControls.Commands;

namespace Frost.RibbonUI.Windows.Add {

    internal class AddStudiosViewModel : INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IDisposable _searchObservable;
        private readonly IDisposable _newStudioObservable;
        private ObservableCollection<MovieStudio> _studios;
        private ICollectionView _collectionView;
        private string _searchBoxText;
        private string _newStudioName;
        private Visibility _errorVisibility;

        public AddStudiosViewModel() {
            AddCommand = new RelayCommand<Window>(AddOnClick, w => ErrorVisibility != Visibility.Visible);
            CancelCommand = new RelayCommand<Window>(CancelOnClick);

            _searchObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                                          .Where(ep => ep.EventArgs.PropertyName == "SearchBoxText")
                                          .Throttle(TimeSpan.FromSeconds(0.5))
                                          .ObserveOn(SynchronizationContext.Current)
                                          .Subscribe(args => _collectionView.Refresh());

            _newStudioObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                                             .Where(ep => ep.EventArgs.PropertyName == "NewStudioName")
                                             .Throttle(TimeSpan.FromSeconds(0.5))
                                             .ObserveOn(SynchronizationContext.Current)
                                             .Subscribe(CheckStudioExists);

            ErrorVisibility = Visibility.Collapsed;
        }

        public ICommand<Window> AddCommand { get; private set; }
        public ICommand<Window> CancelCommand { get; private set; }

        public string SearchBoxText {
            get { return _searchBoxText; }
            set {
                if (value == _searchBoxText) {
                    return;
                }
                _searchBoxText = value;
                OnPropertyChanged();
            }
        }

        public string NewStudioName {
            get { return _newStudioName; }
            set {
                if (value == _newStudioName) {
                    return;
                }

                _newStudioName = value;
                OnPropertyChanged();
            }
        }

        public Visibility ErrorVisibility {
            get { return _errorVisibility; }
            set {
                if (value == _errorVisibility) {
                    return;
                }
                _errorVisibility = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MovieStudio> Studios {
            get { return _studios; }
            set {
                if (Equals(value, _studios)) {
                    return;
                }
                _studios = value;

                if (_studios != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_studios);
                    _collectionView.Filter = Filter;
                }

                OnPropertyChanged();
            }
        }

        private bool Filter(object obj) {
            MovieStudio p = (MovieStudio) obj;

            return p.Name.IndexOf(SearchBoxText ?? "", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void AddOnClick(Window w) {
            if (w == null) {
                return;
            }

            w.DialogResult = true;
            w.Close();
        }

        private void CancelOnClick(Window w) {
            if (w == null) {
                return;
            }

            w.DialogResult = false;
            w.Close();
        }

        private void CheckStudioExists(EventPattern<PropertyChangedEventArgs> args) {
            if (Studios.Any(studio => studio.Name.Equals(NewStudioName, StringComparison.CurrentCultureIgnoreCase))) {
                ErrorVisibility = Visibility.Visible;
            }
            else if (ErrorVisibility == Visibility.Visible) {
                ErrorVisibility = Visibility.Collapsed;
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
                if (_newStudioObservable != null) {
                    _newStudioObservable.Dispose();
                }

                if (_searchObservable != null) {
                    _searchObservable.Dispose();
                }

                if (!finalizer) {
                    GC.SuppressFinalize(this);
                }
                IsDisposed = true;
            }
        }

        ~AddStudiosViewModel() {
            Dispose(true);
        }

        #endregion
    }

}