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
        private readonly IDisposable _searchObservable;
        private ICollectionView _collectionView;
        private string _movieSearchFilter;
        private IMovie _selectedMovie;
        private ObservableCollection<IMovie> _movies;
        private ObservableCollection<MovieVideo> _movieVideos;
        private ObservableCollection<MovieAudio> _movieAudios;
        private ObservableCollection<MovieSubtitle> _movieSubtitles;
        private ObservableCollection<MovieArt> _movieArt;

        public ContentGridViewModel(IMoviesDataService service) {
            Movies = new ObservableCollection<IMovie>(service.Movies);

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

        public ObservableCollection<IMovie> Movies {
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

        public IMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                if (_selectedMovie != null) {
                    MovieVideos = new ObservableCollection<MovieVideo>(_selectedMovie.Videos.Select(v => new MovieVideo(v)));
                    MovieAudios = new ObservableCollection<MovieAudio>(_selectedMovie.Audios.Select(a => new MovieAudio(a)));
                    MovieSubtitles = new ObservableCollection<MovieSubtitle>(_selectedMovie.Subtitles.Select(s => new MovieSubtitle(s)));
                    MovieArt = new ObservableCollection<MovieArt>(_selectedMovie.Art.Select(a => new MovieArt(a)));
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

        public ObservableCollection<MovieVideo> MovieVideos {
            get { return _movieVideos; }
            set {
                if (Equals(value, _movieVideos)) {
                    return;
                }
                _movieVideos = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MovieAudio> MovieAudios {
            get { return _movieAudios; }
            set {
                if (Equals(value, _movieAudios)) {
                    return;
                }
                _movieAudios = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MovieSubtitle> MovieSubtitles {
            get { return _movieSubtitles; }
            set {
                if (Equals(value, _movieSubtitles)) {
                    return;
                }
                _movieSubtitles = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MovieArt> MovieArt {
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

        public Window ParentWindow { get; set; }

        private bool Filter(object o) {
            try {
                return ((IMovie) o).Title.IndexOf(MovieSearchFilter ?? "", StringComparison.CurrentCultureIgnoreCase) != -1;
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

        private void RemoveSubtitle(MovieSubtitle subtitle) {
            MovieSubtitles.Remove(subtitle);
        }

        private void RemovePlot(IPlot plot) {
            SelectedMovie.Remove(plot);
        }

        private void AddPlot(IPlot plot) {
            SelectedMovie.Add(plot);
        }

        private void AddStudio(IStudio studio) {
            SelectedMovie.Add(studio);
        }

        private void RemoveStudio(IStudio studio) {
            SelectedMovie.Add(studio);
        }

        private void AddActor(IActor actor) {
            SelectedMovie.Add(actor);
        }

        private void RemoveActor(IActor actor) {
            SelectedMovie.Remove(actor);
        }

        private void AddDirector(IPerson director) {
            SelectedMovie.Add(director, PersonType.Director);
        }

        private void RemoveDirector(IPerson director) {
            SelectedMovie.Remove(director, PersonType.Director);
        }

        private void AddGenre(IGenre genre) {
            SelectedMovie.Add(genre);
        }

        private void RemoveGenre(IGenre genre) {
            SelectedMovie.Remove(genre);
        }

        private void AddCountry(ICountry country) {
            SelectedMovie.Add(country);
        }

        private void RemoveCountry(ICountry country) {
            SelectedMovie.Remove(country);
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