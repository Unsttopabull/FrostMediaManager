using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Frost.Common;
using Frost.XamlControls.Commands;
using GalaSoft.MvvmLight;
using RibbonUI.Annotations;
using RibbonUI.Design;
using RibbonUI.Messages;
using RibbonUI.Messages.Subtitles;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    public class ContentGridViewModel : ViewModelBase, IDisposable {
        private readonly IDisposable _searchObservable;
        private ICollectionView _collectionView;
        private string _movieSearchFilter;
        private ObservableMovie _selectedMovie;
        private ObservableCollection<ObservableMovie> _movies;
        private DateTime _lastChangedMovie;
        private ObservableCollection<MovieCertification> _certifications;

        public ContentGridViewModel() {
            if (IsInDesignMode) {
                LightInjectContainer.Register<IMoviesDataService, DesignMoviesDataService>();
            }

            IMoviesDataService service = LightInjectContainer.GetInstance<IMoviesDataService>();

            _lastChangedMovie = DateTime.Now;

            Movies = new ObservableCollection<ObservableMovie>(service.Movies.Select(m => new ObservableMovie(m)));

            _searchObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                                          .Where(ep => ep.EventArgs.PropertyName == "MovieSearchFilter")
                                          .Throttle(TimeSpan.FromSeconds(0.5))
                                          .ObserveOn(SynchronizationContext.Current)
                                          .Subscribe(obj => _collectionView.Refresh());

            SubtitlesOnFocusCommand = new RelayCommand(MovieSubtitlesGotFocus);
            SubtitlesLostFocusCommand = new RelayCommand(MovieSubtitlesOnLostFocus);

            RegisterReceiveMessages();
        }

        public ObservableCollection<ObservableMovie> Movies {
            get { return _movies; }
            set {
                if (Equals(value, _movies)) {
                    return;
                }
                _movies = value;

                _collectionView = CollectionViewSource.GetDefaultView(_movies);
                if (_collectionView != null) {
                    _collectionView.SortDescriptions.Add(new SortDescription("SortTitle", ListSortDirection.Ascending));
                    _collectionView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
                    _collectionView.Filter = Filter;
                }

                OnPropertyChanged();
            }
        }

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }

                DateTime now = DateTime.Now;
                if ((now - _lastChangedMovie).TotalSeconds < 0.3) {
                    _lastChangedMovie = now;
                    return;
                }
                _lastChangedMovie = now;

                _selectedMovie = value;

                if (_selectedMovie != null) {
                    var certs = _selectedMovie.Certifications;

                    Certifications = certs == null
                        ? new ObservableCollection<MovieCertification>()
                        : new ObservableCollection<MovieCertification>(certs.Select(c => new MovieCertification(c)));
                }

                OnPropertyChanged();
            }
        }

        public string MovieSearchFilter {
            get { return _movieSearchFilter; }
            set {
                if (value == _movieSearchFilter) {
                    return;
                }
                _movieSearchFilter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MovieCertification> Certifications {
            get { return _certifications; }
            set {
                if (Equals(value, _certifications)) {
                    return;
                }
                _certifications = value;
                OnPropertyChanged();
            }
        }

        public ICommand SubtitlesOnFocusCommand { get; private set; }
        public ICommand SubtitlesLostFocusCommand { get; private set; }

        public Window ParentWindow { get; set; }

        private bool Filter(object o) {
            try {
                return ((ObservableMovie) o).Title.IndexOf(MovieSearchFilter ?? "", StringComparison.CurrentCultureIgnoreCase) != -1;
            }
            catch (Exception e) {
                return false;
            }
        }

        private void RegisterReceiveMessages() {
            MessengerInstance.Register<AddSubtitleMessage>(this, HandleAddSubtitleMessage);
        }

        private void HandleAddSubtitleMessage(AddSubtitleMessage msg) {
            if (SelectedMovie == null) {
                return;
            }

            try {
                SelectedMovie.AddSubtitle(msg.Subtitle.ObservedEntity);
            }
            catch (Exception e) {
                UIHelper.HandleProviderException(e);
            }
        }

        private void MovieSubtitlesGotFocus() {
            MessengerInstance.Send(new SelectRibbonMessage(RibbonTabs.Subtitles));
        }

        private void MovieSubtitlesOnLostFocus() {
            MessengerInstance.Send(new SelectRibbonMessage(RibbonTabs.Search));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChangedHandler != null) {
                PropertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
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
                if (_searchObservable != null) {
                    _searchObservable.Dispose();
                }

                if (!finalizer) {
                    GC.SuppressFinalize(this);
                }
                IsDisposed = true;
            }
        }

        ~ContentGridViewModel() {
            Dispose(true);
        }

        #endregion
    }

}