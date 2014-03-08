using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Frost.Common.Annotations;
using Frost.Models.Frost.DB;
using Frost.Models.Frost.DB.Arts;
using Frost.Models.Frost.DB.Files;
using Frost.XamlControls.Commands;
using RibbonUI.UserControls;
using RibbonUI.Util;

namespace RibbonUI.ViewModels.UserControls {

    public class ContentGridViewModel : INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IDisposable _searchObservable;
        private ICollectionView _collectionView;
        private string _movieSearchFilter;
        private Movie _selectedMovie;
        private ObservableHashSet2<Movie> _movieList;
        private ObservableHashSet2<Video> _movieVideos;
        private ObservableHashSet2<Audio> _movieAudios;
        private ObservableHashSet2<Subtitle> _movieSubtitles;
        private ObservableHashSet2<Art> _movieArt;
        private Window _parentWindow;

        public ContentGridViewModel() {
            _searchObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                                          .Where(ep => ep.EventArgs.PropertyName == "MovieSearchFilter")
                                          .Throttle(TimeSpan.FromSeconds(0.5))
                                          .ObserveOn(SynchronizationContext.Current)
                                          .Subscribe(obj => _collectionView.Refresh());

            SubtitlesOnFocusCommand = new RelayCommand(MovieSubtitlesGotFocus);
            SubtitlesLostFocusCommand = new RelayCommand(MovieSubtitlesOnLostFocus);
        }

        public ObservableHashSet2<Movie> MovieList {
            get { return _movieList; }
            set {
                if (Equals(value, _movieList)) {
                    return;
                }
                _movieList = value;

                if (_movieList != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_movieList);
                    _collectionView.SortDescriptions.Add(new SortDescription("SortTitle", ListSortDirection.Ascending));
                    _collectionView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
                    _collectionView.Filter = Filter;
                }

                OnPropertyChanged();
            }
        }

        public Movie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                if (_selectedMovie != null) {
                    MovieVideos = new ObservableHashSet2<Video>(_selectedMovie.Videos);
                    MovieAudios = new ObservableHashSet2<Audio>(_selectedMovie.Audios);
                    MovieSubtitles = new ObservableHashSet2<Subtitle>(_selectedMovie.Subtitles);
                    MovieArt = new ObservableHashSet2<Art>(_selectedMovie.Art);
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

        public ObservableHashSet2<Video> MovieVideos {
            get { return _movieVideos; }
            set {
                if (Equals(value, _movieVideos)) {
                    return;
                }
                _movieVideos = value;
                OnPropertyChanged();
            }
        }
        public ObservableHashSet2<Audio> MovieAudios {
            get { return _movieAudios; }
            set {
                if (Equals(value, _movieAudios)) {
                    return;
                }
                _movieAudios = value;
                OnPropertyChanged();
            }
        }
        public ObservableHashSet2<Subtitle> MovieSubtitles {
            get { return _movieSubtitles; }
            set {
                if (Equals(value, _movieSubtitles)) {
                    return;
                }
                _movieSubtitles = value;
                OnPropertyChanged();
            }
        }
        public ObservableHashSet2<Art> MovieArt {
            get { return _movieArt; }
            set {
                if (Equals(value, _movieArt)) {
                    return;
                }
                _movieArt = value;
                OnPropertyChanged();
            }
        }

        public ICommand SubtitlesOnFocusCommand { get; private set; }
        public ICommand SubtitlesLostFocusCommand { get; private set; }

        public Window ParentWindow {
            get { return _parentWindow; }
            set {
                _parentWindow = value;
                if (_parentWindow != null) {
                    MovieList = new ObservableHashSet2<Movie>((ICollection<Movie>) ((CollectionViewSource) _parentWindow.Resources["MoviesSource"]).Source);
                }
            }
        }

        private bool Filter(object o) {
            try {
                return ((Movie) o).Title.IndexOf(MovieSearchFilter ?? "", StringComparison.CurrentCultureIgnoreCase) != -1;
            }
            catch (Exception e) {
                return false;
            }
        }

        private void MovieSubtitlesGotFocus() {
            Ribbon rb = ((MainWindow) ParentWindow).Ribbon;
            rb.ContextSubtitle.Visibility = Visibility.Visible;
            rb.SubtitlesTab.IsSelected = true;
        }

        private void MovieSubtitlesOnLostFocus() {
            Ribbon rb = ((MainWindow) ParentWindow).Ribbon;
            rb.ContextSubtitle.Visibility = Visibility.Collapsed;
            rb.Search.IsSelected = true;
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