using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.Providers.Frost.DB;
using Frost.Providers.Frost.Proxies;
using IOFile = System.IO.File;

namespace Frost.Providers.Frost.Provider {
    public class FrostMoviesDataDataService : IMoviesDataService {
        private readonly FrostDbContainer _mvc;
        private IEnumerable<IMovie> _movies;
        private IEnumerable<IFile> _files;
        private IEnumerable<IVideo> _videos;
        private IEnumerable<IAudio> _audios;
        private IEnumerable<ISubtitle> _subtitles;
        private IEnumerable<IArt> _art;
        private IEnumerable<ICountry> _countries;
        private IEnumerable<IStudio> _studios;
        private IEnumerable<IRating> _ratings;
        private IEnumerable<IPlot> _plots;
        private IEnumerable<IGenre> _genres;
        private IEnumerable<IAward> _awards;
        private IEnumerable<IPromotionalVideo> _promotionalVideos;
        private IEnumerable<ICertification> _certifications;
        private IEnumerable<IMovieSet> _sets;
        private IEnumerable<ILanguage> _languages;
        private IEnumerable<ISpecial> _specials;
        private IEnumerable<IPerson> _people;

        public FrostMoviesDataDataService() {
            _mvc = new FrostDbContainer();
        }

        public IEnumerable<IMovie> Movies {
            get {
                if (_movies != null) {
                    return _movies;
                }

                _mvc.Movies
                    .Include("Studios")
                    .Include("Art")
                    //.Include("Genres")
                    //.Include("Awards")
                    .Include("Actors")
                    .Include("Actors.Person")
                    .Include("Plots")
                    //.Include("Directors")
                    //.Include("Countries")
                    //.Include("Audios")
                    //.Include("Videos")
                    .Load();

                _mvc.Languages.Load();

                _movies = _mvc.Movies.Local.Select(m => new FrostMovie(m, this));
                return _movies;
            }
        }

        public IEnumerable<IFile> Files {
            get {
                if (_files != null) {
                    return _files;
                }

                _mvc.Files.Load();
                _files = _mvc.Files.Local;
                return _files;
            }
        }

        public IEnumerable<IVideo> Videos {
            get {
                if (_videos != null) {
                    return _videos;
                }

                _mvc.VideoDetails
                    .Include("File")
                    .Include("Language")
                    .Load();

                _videos = _mvc.VideoDetails.Local.Select(v => new FrostVideo(v, this));
                return _videos;
            }
        }

        public IEnumerable<IAudio> Audios {
            get {
                if (_audios != null) {
                    return _audios;
                }

                _mvc.AudioDetails
                    .Include("File")
                    .Include("Language")
                    .Load();

                _audios = _mvc.AudioDetails.Local.Select(a => new FrostAudio(a, this));
                return _audios;
            }
        }

        #region Subtitles

        public IEnumerable<ISubtitle> Subtitles {
            get {
                if (_subtitles == null) {
                    _mvc.Subtitles
                        .Include("Language")
                        .Load();

                    _subtitles = _mvc.Subtitles.Local.Select(s => new FrostSubtitle(s, this));
                }
                return _subtitles;
            }
        }

        internal Subtitle FindOrCreateSubtitle(ISubtitle subtitle, bool createIfNotFound) {
            return _mvc.FindOrCreateSubtitle(subtitle, createIfNotFound);
        }

        #endregion

        public IEnumerable<IArt> Art {
            get {
                if (_art != null) {
                    return _art;
                }

                _mvc.Art.Load();
                _art = _mvc.Art.Local;
                return _art;
            }
        }

        public Art FindArt(IArt art, bool createIfNotFound) {
            Art a;
            if (art.Id > 0) {
                a = _mvc.Art.Find(art.Id);
                if (a == null && createIfNotFound) {
                    return new Art(art);
                }
                return a;
            }

            a = _mvc.Art.FirstOrDefault(ar => ar.Path == art.Path);
            if (a == null && createIfNotFound) {
                return new Art(art);
            }
            return a;
        }

        #region Countries

        public IEnumerable<ICountry> Countries {
            get {
                if (_countries != null) {
                    return _countries;
                }

                _mvc.Countries.Load();
                _countries = _mvc.Countries.Local;
                return _countries;
            }
        }

        internal Country FindCountry(ICountry country, bool createIfNotFound) {
            return _mvc.FindCountry(country, createIfNotFound);
        }

        #endregion

        #region Studios

        public IEnumerable<IStudio> Studios {
            get {
                if (_studios != null) {
                    return _studios;
                }

                _mvc.Studios.Load();
                _studios = _mvc.Studios.Local;
                return _studios;
            }
        }

        internal Studio FindStudio(IStudio studio, bool createNotFound) {
            return _mvc.FindHasName<IStudio, Studio>(studio, createNotFound);
        }

        #endregion

        public IEnumerable<IRating> Ratings {
            get {
                if (_ratings != null) {
                    return _ratings;
                }

                _mvc.Ratings.Load();
                _ratings = _mvc.Ratings.Local;
                return _ratings;
            }
        }

        #region Plots

        public IEnumerable<IPlot> Plots {
            get {
                if (_plots != null) {
                    return _plots;
                }

                _mvc.Plots.Load();
                _plots = _mvc.Plots.Local;
                return _plots;
            }
        }

        internal Plot FindPlot(IPlot plot, bool createIfNotFound, long movieId) {
            return _mvc.FindPlot(plot, createIfNotFound, movieId);
        }

        #endregion

        public IEnumerable<IGenre> Genres {
            get {
                if (_genres != null) {
                    return _genres;
                }

                _mvc.Genres.Load();
                _genres = _mvc.Genres.Local;
                return _genres;
            }
        }

        public IEnumerable<IAward> Awards {
            get {
                if (_awards != null) {
                    return _awards;
                }

                _mvc.Awards.Load();
                _awards = _mvc.Awards.Local;
                return _awards;
            }
        }

        public Award FindAward(IAward award, bool createNotFound) {
            if (award.Id > 0) {
                Award aw = _mvc.Awards.Find(award.Id);
                if (aw == null && createNotFound) {
                    return new Award(award);
                }
                return aw;
            }

            Award a = _mvc.Awards.FirstOrDefault(awrd => awrd.AwardType == award.AwardType &&
                                                         award.IsNomination == awrd.IsNomination &&
                                                         award.Organization == awrd.Organization);
            if (a == null && createNotFound) {
                return new Award(award);
            }
            return a;
        }

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get {
                if (_promotionalVideos != null) {
                    return _promotionalVideos;
                }

                _mvc.PromotionalVideos.Load();
                _promotionalVideos = _mvc.PromotionalVideos.Local;
                return _promotionalVideos;
            }
        }

        public PromotionalVideo FindPromotionalVideo(IPromotionalVideo video, bool createNotFound) {
            if (video.Id > 0) {
                PromotionalVideo promotionalVideo = _mvc.PromotionalVideos.Find(video.Id);
                if (promotionalVideo == null && createNotFound) {
                    return new PromotionalVideo(video);
                }
                return promotionalVideo;
            }

            PromotionalVideo vid = _mvc.PromotionalVideos.FirstOrDefault(pv => string.Equals(video.Title, pv.Title));
            if (vid == null && createNotFound) {
                return new PromotionalVideo(video);
            }
            return vid;
        }

        public IEnumerable<ICertification> Certifications {
            get {
                if (_certifications != null) {
                    return _certifications;
                }

                _mvc.Certifications
                    .Include("Country")
                    .Load();

                _certifications = _mvc.Certifications.Local.Select(c => new FrostCertification(c, this));
                return _certifications;
            }
        }

        public IEnumerable<IMovieSet> Sets {
            get {
                if (_sets != null) {
                    return _sets;
                }

                _mvc.Sets.Load();
                _sets = _mvc.Sets.Local;
                return _sets;
            }
        }

        #region Language

        public IEnumerable<ILanguage> Languages {
            get {
                if (_languages != null) {
                    return _languages;
                }

                _mvc.Languages.Load();
                _languages = _mvc.Languages.Local;
                return _languages;
            }
        }

        internal Language FindLanguage(ILanguage language, bool createIfNotFound) {
            return _mvc.FindLanguage(language, createIfNotFound);
        }

        #endregion

        public IEnumerable<ISpecial> Specials {
            get {
                if (_specials != null) {
                    return _specials;
                }

                _mvc.Specials.Load();
                _specials = _mvc.Specials.Local;
                return _specials;
            }
        }

        #region People

        public IEnumerable<IPerson> People {
            get {
                if (_people != null) {
                    return _people;
                }

                _mvc.People.Load();
                _people = _mvc.People.Local;
                return _people;
            }
        }

        public bool CanSaveDetectedAsync { get { return true; } }

        internal Person FindPerson(IPerson person, bool createIfNotFound) {
            return _mvc.FindPerson(person, createIfNotFound);
        }

        #endregion

        internal TSet FindHasName<TEntity, TSet>(TEntity hasName, bool createIfNotFound) where TEntity : class, IHasName, IMovieEntity
                                                                                         where TSet : class, IHasName, IMovieEntity {
            return _mvc.FindHasName<TEntity, TSet>(hasName, createIfNotFound);
        }

        internal void ReactivateIfDeleted(object entity) {
            DbEntityEntry entry = _mvc.Entry(entity);
            if (entry.State == EntityState.Deleted) {
                entry.State = EntityState.Modified;
            }
        }

        internal bool MarkAsDeleted(object entity) {
            try {
                DbEntityEntry entry = _mvc.Entry(entity);
                if (entry != null) {
                    entry.State = EntityState.Deleted;
                }
                return true;
            }
            catch {
                return false;
            }
        }

        internal T FindById<T>(long id) where T : class {
            DbSet<T> dbSet;

            try {
                dbSet = _mvc.Set<T>();
            }
            catch {
                return null;
            }

            if (dbSet != null) {
                return dbSet.Find(id);
            }
            return null;
        }

        public void SaveDetected(MovieInfo movieInfo) {
            MovieSaver ms = new MovieSaver(movieInfo, _mvc);
            ms.Save(false);
        }

        public bool HasUnsavedChanges() {
            return _mvc.ChangeTracker.HasChanges();
        }

        public void SaveChanges() {
            _mvc.SaveChanges();

            foreach (Movie mov in _mvc.Movies.Where(m => m.MainPlot == null || m.DefaultFanart == null || m.DefaultCover == null)) {
                if (mov.MainPlot == null) {
                    mov.MainPlot = mov.Plots.FirstOrDefault();
                }

                if (mov.DefaultFanart == null) {
                    mov.DefaultFanart = mov.Art.FirstOrDefault(a => a.Type == ArtType.Fanart);
                }

                if (mov.DefaultCover == null) {
                    mov.DefaultCover = mov.Art.FirstOrDefault(a => a.Type == ArtType.Poster || a.Type == ArtType.Cover);
                }
            }
            _mvc.SaveChanges();

            MovieSaver.Reset();
        }

        #region IDisposable

        public bool IsDisposed { get; private set; }

        public void Dispose() {
            Dispose(false);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        private void Dispose(bool finalizer) {
            if (!IsDisposed) {
                if (_mvc != null) {
                    _mvc.Dispose();
                }

                if (!finalizer) {
                    GC.SuppressFinalize(this);
                }
                IsDisposed = true;
            }
        }

        ~FrostMoviesDataDataService() {
            Dispose(true);
        }

        #endregion
    }
}
