using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Design;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using IMovieList = System.Collections.Generic.List<Frost.Common.Models.Provider.IMovie>;

namespace RibbonUI.UserControls {

    public class ContentGridViewModel : INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IDisposable _searchObservable;
        private readonly IMoviesDataService _service;
        private ICollectionView _collectionView;
        private string _movieSearchFilter;
        private ObservableMovie _selectedMovie;
        private ObservableCollection<ObservableMovie> _movies;
        private ObservableCollection<MovieCertification> _certifications;
        private DateTime _lastChangedMovie;
        private RibbonTabs _tab;

        public ContentGridViewModel() {
            _service = TranslationManager.IsInDesignMode 
                           ? new DesignMoviesDataService()
                           : LightInjectContainer.GetInstance<IMoviesDataService>();

            _lastChangedMovie = DateTime.Now;

            Movies = new ThreadSafeObservableCollection<ObservableMovie>(_service.Movies.Select(m => new ObservableMovie(m)));
            _service.Movies.CollectionChanged += MoviesChanged;


            _searchObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                                          .Where(ep => ep.EventArgs.PropertyName == "MovieSearchFilter")
                                          .Throttle(TimeSpan.FromSeconds(0.5))
                                          .ObserveOn(SynchronizationContext.Current)
                                          .Subscribe(obj => _collectionView.Refresh());

            SubtitlesOnFocusCommand = new RelayCommand(() => Tab = RibbonTabs.Subtitles);
            SubtitlesLostFocusCommand = new RelayCommand(() => Tab = RibbonTabs.Search);
        }

        void MoviesChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    foreach (IMovie movie in e.NewItems) {
                        Movies.Add(new ObservableMovie(movie));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    IMovieList movies = new IMovieList();
                    foreach (IMovie mov in movies) {
                        ObservableMovie movie = Movies.FirstOrDefault(m => m.Equals(mov));
                        if (movie != null) {
                            Movies.Remove(movie);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Movies = new ObservableCollection<ObservableMovie>(_service.Movies.Select(m => new ObservableMovie(m)));
                    break;
                default:
                    return;
            }
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

        public RibbonTabs Tab {
            get { return _tab; }
            set {
                if (value == _tab) {
                    return;
                }
                _tab = value;
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