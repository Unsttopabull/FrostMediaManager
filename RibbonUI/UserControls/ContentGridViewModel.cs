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
using Frost.Common.Models;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;
using GalaSoft.MvvmLight;
using RibbonUI.Messages;
using RibbonUI.Messages.Country;
using RibbonUI.Messages.Genre;
using RibbonUI.Messages.People;
using RibbonUI.Messages.Plot;
using RibbonUI.Messages.Studio;
using RibbonUI.Messages.Subtitles;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    public class ContentGridViewModel : ViewModelBase, IDisposable {
        private readonly IMoviesDataService _service;
        private readonly IDisposable _searchObservable;
        private ICollectionView _collectionView;
        private string _movieSearchFilter;
        private ObservableMovie _selectedMovie;
        private ObservableCollection<ObservableMovie> _movies;
        private ObservableCollection<MovieVideo> _videos;
        private ObservableCollection<MovieAudio> _audios;
        private ObservableCollection<MovieSubtitle> _subtitles;
        private ObservableCollection<MovieArt> _art;

        public ContentGridViewModel(IMoviesDataService service) {
            _service = service;

            Movies = new ObservableCollection<ObservableMovie>(service.Movies.Select(m => new ObservableMovie(m)));

            _searchObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                                          .Where(ep => ep.EventArgs.PropertyName == "MovieSearchFilter")
                                          .Throttle(TimeSpan.FromSeconds(0.5))
                                          .ObserveOn(SynchronizationContext.Current)
                                          .Subscribe(obj => _collectionView.Refresh());

            SubtitlesOnFocusCommand = new RelayCommand(MovieSubtitlesGotFocus);
            SubtitlesLostFocusCommand = new RelayCommand(MovieSubtitlesOnLostFocus);

            MessengerInstance.Register<RemoveSubtitleMessage>(this, msg => RemoveSubtitle(msg.Subtitle));

            MessengerInstance.Register<AddStudioMessage>(this, s => AddStudio(s.Studio));
            MessengerInstance.Register<RemoveStudioMessage>(this, s => RemoveStudio(s.Studio));

            MessengerInstance.Register<AddPlotMessage>(this, s => AddPlot(s.Plot));
            MessengerInstance.Register<RemovePlotMessage>(this, s => RemovePlot(s.Plot));

            MessengerInstance.Register<AddActorMessage>(this, s => AddActor(s.Actor));
            MessengerInstance.Register<RemoveActorMessage>(this, s => RemoveActor(s.Actor));

            MessengerInstance.Register<AddDirectorMessage>(this, s => AddDirector(s.Director));
            MessengerInstance.Register<RemoveDirectorMessage>(this, s => RemoveDirector(s.Director));

            MessengerInstance.Register<AddGenreMessage>(this, s => AddGenre(s.Genre));
            MessengerInstance.Register<RemoveGenreMessage>(this, s => RemoveGenre(s.Genre));

            MessengerInstance.Register<AddCountryMessage>(this, s => AddCountry(s.Country));
            MessengerInstance.Register<RemoveCountryMessage>(this, s => RemoveCountry(s.Country));
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
                _selectedMovie = value;

                if (_selectedMovie != null) {
                    Videos = new ObservableCollection<MovieVideo>(_selectedMovie.Videos.Select(v => new MovieVideo(v)));
                    Audios = new ObservableCollection<MovieAudio>(_selectedMovie.Audios.Select(a => new MovieAudio(a)));
                    Subtitles = new ObservableCollection<MovieSubtitle>(_selectedMovie.Subtitles.Select(s => new MovieSubtitle(s)));
                    Art = new ObservableCollection<MovieArt>(_selectedMovie.Art.Select(a => new MovieArt(a)));
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

        public ObservableCollection<MovieVideo> Videos {
            get { return _videos; }
            set {
                if (Equals(value, _videos)) {
                    return;
                }
                _videos = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MovieAudio> Audios {
            get { return _audios; }
            set {
                if (Equals(value, _audios)) {
                    return;
                }
                _audios = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MovieSubtitle> Subtitles {
            get { return _subtitles; }
            set {
                if (Equals(value, _subtitles)) {
                    return;
                }
                _subtitles = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MovieArt> Art {
            get { return _art; }
            set {
                if (Equals(value, _art)) {
                    return;
                }
                _art = value;
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

        private void MovieSubtitlesGotFocus() {
            MessengerInstance.Send(new SelectRibbonMessage(RibbonTabs.Subtitles));
        }

        private void MovieSubtitlesOnLostFocus() {
            MessengerInstance.Send(new SelectRibbonMessage(RibbonTabs.Subtitles));
        }

        #region Message Handlers

        private void AddSubtitle(MovieSubtitle subtitle) {
            ISubtitle sub = _service.AddSubtitle(SelectedMovie.ObservedMovie, subtitle.ObservedSubtitle);
            Subtitles.Add(new MovieSubtitle(sub));
        }

        private void RemoveSubtitle(MovieSubtitle subtitle) {
            _service.RemoveSubtitle(SelectedMovie.ObservedMovie, subtitle.ObservedSubtitle);
            Subtitles.Remove(subtitle);
        }

        private void RemovePlot(IPlot plot) {
            _service.RemovePlot(_selectedMovie.ObservedMovie, plot);
        }

        private void AddPlot(IPlot plot) {
            _service.AddPlot(_selectedMovie.ObservedMovie, plot);
        }

        private void AddStudio(IStudio studio) {
            _service.AddStudio(_selectedMovie.ObservedMovie, studio);
        }

        private void RemoveStudio(IStudio studio) {
            _service.RemoveStudio(_selectedMovie.ObservedMovie, studio);
        }

        private void AddActor(IActor actor) {
            _service.AddActor(_selectedMovie.ObservedMovie, actor);
        }

        private void RemoveActor(IActor actor) {
            _service.RemoveActor(_selectedMovie.ObservedMovie, actor);
        }

        private void AddDirector(IPerson director) {
            _service.AddDirector(_selectedMovie.ObservedMovie, director);
        }

        private void RemoveDirector(IPerson director) {
            _service.RemoveDirector(_selectedMovie.ObservedMovie, director);
        }

        private void AddGenre(IGenre genre) {
            _service.AddGenre(_selectedMovie.ObservedMovie, genre);
        }

        private void RemoveGenre(IGenre genre) {
            _service.RemoveGenre(_selectedMovie.ObservedMovie, genre);
        }

        private void AddCountry(ICountry country) {
            _service.AddCountry(_selectedMovie.ObservedMovie, country);
        }

        private void RemoveCountry(ICountry country) {
            _service.RemoveCountry(_selectedMovie.ObservedMovie, country);
        }

        #endregion

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