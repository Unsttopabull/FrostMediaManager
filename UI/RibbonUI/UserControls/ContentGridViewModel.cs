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
using Frost.XamlControls.Commands;
using GalaSoft.MvvmLight;
using RibbonUI.Annotations;
using RibbonUI.Design;
using RibbonUI.Messages;
using RibbonUI.Messages.Country;
using RibbonUI.Messages.Genre;
using RibbonUI.Messages.People;
using RibbonUI.Messages.Plot;
using RibbonUI.Messages.Studio;
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
        private ObservableCollection<MovieVideo> _videos;
        private ObservableCollection<MovieAudio> _audios;
        private ObservableCollection<MovieSubtitle> _subtitles;
        private ObservableCollection<MovieArt> _art;
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

                DateTime now = DateTime.Now;
                if ((now - _lastChangedMovie).TotalSeconds < 0.5) {
                    _lastChangedMovie = now;
                    return;
                }
                _lastChangedMovie = now;

                _selectedMovie = value;

                if (_selectedMovie != null) {
                    var videos = _selectedMovie.Videos;
                    var audios = _selectedMovie.Audios;
                    var subs = _selectedMovie.Subtitles;
                    var art = _selectedMovie.Art;
                    var certs = _selectedMovie.Certifications;

                    Videos = videos == null
                        ? new ObservableCollection<MovieVideo>()
                        : new ObservableCollection<MovieVideo>(videos.Select(v => new MovieVideo(v)));

                    Audios = audios == null
                        ? new ObservableCollection<MovieAudio>()
                        : new ObservableCollection<MovieAudio>(audios.Select(a => new MovieAudio(a)));

                    Subtitles = subs == null
                        ? new ObservableCollection<MovieSubtitle>()
                        : new ObservableCollection<MovieSubtitle>(subs.Select(s => new MovieSubtitle(s)));


                    Art = art == null
                        ? new ObservableCollection<MovieArt>()
                        : new ObservableCollection<MovieArt>(art.Select(a => new MovieArt(a)));

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
            ISubtitle sub = SelectedMovie.AddSubtitle(subtitle.ObservedEntity);
            Subtitles.Add(new MovieSubtitle(sub));
        }

        private void RemoveSubtitle(MovieSubtitle subtitle) {
            SelectedMovie.RemoveSubtitle(subtitle.ObservedEntity);
            Subtitles.Remove(subtitle);
        }

        private void RemovePlot(IPlot plot) {
            SelectedMovie.RemovePlot(plot);
        }

        private void AddPlot(IPlot plot) {
            SelectedMovie.AddPlot(plot);
        }

        private void AddStudio(IStudio studio) {
            SelectedMovie.AddStudio(studio);
        }

        private void RemoveStudio(IStudio studio) {
            SelectedMovie.RemoveStudio(studio);
        }

        private void AddActor(IActor actor) {
            SelectedMovie.AddActor(actor);
        }

        private void RemoveActor(IActor actor) {
            SelectedMovie.RemoveActor(actor);
        }

        private void AddDirector(IPerson director) {
            SelectedMovie.AddDirector(director);
        }

        private void RemoveDirector(IPerson director) {
            SelectedMovie.RemoveDirector(director);
        }

        private void AddGenre(IGenre genre) {
            SelectedMovie.AddGenre(genre);
        }

        private void RemoveGenre(IGenre genre) {
            SelectedMovie.RemoveGenre(genre);
        }

        private void AddCountry(ICountry country) {
            SelectedMovie.AddCountry(country);
        }

        private void RemoveCountry(ICountry country) {
            SelectedMovie.RemoveCountry(country);
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