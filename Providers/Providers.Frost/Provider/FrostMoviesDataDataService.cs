using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
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
        private ObservableCollection<IMovie> _movies;
        private IEnumerable<ICountry> _countries;
        private IEnumerable<IStudio> _studios;
        private IEnumerable<IGenre> _genres;
        private IEnumerable<IAward> _awards;
        private IEnumerable<IMovieSet> _sets;
        private IEnumerable<ILanguage> _languages;
        private IEnumerable<ISpecial> _specials;
        private IEnumerable<IPerson> _people;

        public FrostMoviesDataDataService() {
            _mvc = new FrostDbContainer();
        }

        public ObservableCollection<IMovie> Movies {
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

                _movies = new ObservableCollection<IMovie>(_mvc.Movies.Local.Select(m => new FrostMovie(m, this)));
                return _movies;
            }
        }

        public bool RemoveMovie(IMovie movie) {
            if (!(movie is FrostMovie)) {
                return false;
            }

            FrostMovie m = movie as FrostMovie;
            int rowsChanged = _mvc.Database.ExecuteSqlCommand("DELETE FROM Movies WHERE Id = {0};", m.Id);

            if (rowsChanged > 0) {
                Movies.Remove(movie);
                return true;
            }
            return false;
        }

        internal Subtitle FindSubtitle(ISubtitle subtitle, bool createIfNotFound) {
            Subtitle p;
            if (subtitle.Id > 0) {
                p = _mvc.Subtitles.Find(subtitle.Id);
                if (p == null || (subtitle.MD5 != null && p.MD5 != subtitle.MD5)) {
                    if (createIfNotFound) {
                        p = new Subtitle(subtitle);
                    }
                    else {
                        p = null;
                    }
                }
                return p;
            }

            p = _mvc.Subtitles
                    .FirstOrDefault(pr =>
                                    (subtitle.MD5 != null && pr.MD5 == subtitle.MD5) ||
                                    (subtitle.OpenSubtitlesId != null && pr.OpenSubtitlesId == subtitle.OpenSubtitlesId) ||
                                    (subtitle.PodnapisiId != null && pr.PodnapisiId == subtitle.PodnapisiId) ||
                                    (subtitle.EmbededInVideo == pr.EmbededInVideo &&
                                     subtitle.ForHearingImpaired == pr.ForHearingImpaired &&
                                     subtitle.Encoding == pr.Encoding));

            if ((p == null || _mvc.Entry(p).State == EntityState.Deleted) && createIfNotFound) {
                p = new Subtitle(subtitle);
            }
            return p;
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
            if ((a == null || _mvc.Entry(a).State == EntityState.Deleted) && createIfNotFound) {
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
            Country c = null;
            if (country.Id > 0) {
                c = _mvc.Countries.Find(country.Id);
                if (c == null || (c.Name != country.Name)) {
                    if (createIfNotFound) {
                        c = _mvc.Countries.Add(new Country(country));
                    }
                    else {
                        return null;
                    }
                }
                return c;
            }

            if (country.ISO3166 != null && !string.IsNullOrEmpty(country.ISO3166.Alpha3)) {
                c = _mvc.Countries.FirstOrDefault(pr => pr.ISO3166.Alpha3 == country.ISO3166.Alpha3);
            }

            if (c == null) {
                c = _mvc.Countries.FirstOrDefault(pr => pr.Name == country.Name);
            }

            if (c == null && createIfNotFound) {
                c = _mvc.Countries.Add(new Country(country));
            }
            return c;
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
            DbSet<Studio> set = _mvc.Set<Studio>();
            if (studio.Id > 0) {
                //check if the entity is already in the context otherwise retreive it
                Studio find = set.Find(studio.Id);
                if ((find == null || ((IHasName) find).Name != studio.Name)) {
                    if (createNotFound) {
                        find = set.Create();
                        ((IHasName) find).Name = studio.Name;

                        find = set.Add(find);
                    }
                    else {
                        return null;
                    }
                }
                return find;
            }

            //primary key not available so search in the DB by Name
            Studio hn = set.FirstOrDefault(n => ((IHasName) n).Name == studio.Name);
            if (hn == null && createNotFound) {
                hn = set.Create();
                ((IHasName) hn).Name = studio.Name;

                hn = set.Add(hn);
            }
            return hn;
        }

        #endregion

        #region Plots

        internal Plot FindPlot(IPlot plot, bool createIfNotFound, long movieId) {
            Plot p;
            if (plot.Id > 0) {
                p = _mvc.Plots.Find(plot.Id);
                if (p == null || (p.Full != plot.Full)) {
                    if (createIfNotFound) {
                        p = new Plot(plot);
                    }
                    else {
                        return null;
                    }
                }
                return p;
            }

            p = movieId > 0
                    ? _mvc.Plots.FirstOrDefault(pr => plot.Full == pr.Full && pr.MovieId == movieId)
                    : _mvc.Plots.FirstOrDefault(pr => plot.Full == pr.Full);

            if ((p == null || _mvc.Entry(p).State == EntityState.Deleted) && createIfNotFound) {
                p = new Plot(plot);
            }

            return p;
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
            Language c;
            if (language.Id > 0) {
                c = _mvc.Languages.Find(language.Id);
                if (c == null || (c.Name != language.Name)) {
                    if (createIfNotFound) {
                        c = new Language(language);
                    }
                    else {
                        c = null;
                    }
                }
                return c;
            }

            if (language.ISO639 != null) {
                c = _mvc.Languages.FirstOrDefault(pr => (pr.ISO639.Alpha3 == language.ISO639.Alpha3) || pr.Name == language.Name);
            }
            else {
                c = _mvc.Languages.FirstOrDefault(pr => pr.Name == language.Name);
            }

            if (c == null && createIfNotFound) {
                return new Language(language);
            }
            return c;
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

        internal Person FindPerson(IPerson person, bool createIfNotFound) {
            if (person == null) {
                return null;
            }

            Person p;
            if (person.Id > 0) {
                p = _mvc.People.Find(person.Id);
                if (p != null && p.Name == person.Name) {
                    return p;
                }

                return createIfNotFound
                           ? _mvc.People.Add(new Person(person))
                           : null;
            }

            p = _mvc.People.FirstOrDefault(pr => (person.ImdbID != null && pr.ImdbID == person.ImdbID) || pr.Name == person.Name);
            if (p == null && createIfNotFound) {
                p = _mvc.People.Add(new Person(person));
            }
            return p;
        }

        public Certification FindCertification(ICertification certification, long movieId, bool createNotFound) {
            if (certification.Country == null) {
                throw new NotSupportedException("The certification must have associated country");
            }

            Country c = FindCountry(certification.Country, true);

            if (certification.Id > 0) {
                Certification find = _mvc.Certifications.Find(certification.Id);
                if (find == null && createNotFound) {
                    return new Certification(certification, c);
                }
                return find;
            }

            Certification cert = _mvc.Certifications.FirstOrDefault(crt => crt.Rating == certification.Rating && crt.MovieId == movieId && crt.CountryId == c.Id);
            if ((cert == null || _mvc.Entry(cert).State == EntityState.Deleted) && createNotFound) {
                return new Certification(certification, c);
            }
            return cert;
        }

        #endregion

        public File FindFile(IFile file, bool createNofTound) {
            if (file is File) {
                return file as File;
            }

            if (file.Id > 0) {
                File find = _mvc.Files.Find(file.Id);
                if (find != null) {
                    return find;
                }
            }

            string path = file.FolderPath.Replace("/", "\\");

            File fi = _mvc.Files.FirstOrDefault(f => f.FolderPath == path && f.Name == file.Name);
            if (fi == null && createNofTound) {
                return new File(file);
            }
            return fi;
        }

        internal TSet FindHasName<TEntity, TSet>(TEntity hasName, bool createIfNotFound) where TEntity : class, IHasName, IMovieEntity
            where TSet : class, IHasName, IMovieEntity {
            DbSet<TSet> set = _mvc.Set<TSet>();
            if (hasName.Id > 0) {
                //check if the entity is already in the context otherwise retreive it
                TSet find = set.Find(hasName.Id);
                if ((find == null || find.Name != hasName.Name)) {
                    if (createIfNotFound) {
                        find = set.Create();
                        find.Name = hasName.Name;

                        find = set.Add(find);
                    }
                    else {
                        return null;
                    }
                }
                return find;
            }

            //primary key not available so search in the DB by Name
            TSet hn = set.FirstOrDefault(n => n.Name == hasName.Name);
            if ((hn == null || _mvc.Entry(hn).State == EntityState.Deleted) && createIfNotFound) {
                hn = set.Create();
                hn.Name = hasName.Name;

                hn = set.Add(hn);
            }
            return hn;
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
                    if (entry.State == EntityState.Unchanged || entry.State == EntityState.Modified) {
                        entry.State = EntityState.Deleted;
                    }
                    else {
                        entry.State = EntityState.Detached;
                    }
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
            Movie movie = ms.Save(false);
            if (movie != null) {
                _movies.Add(new FrostMovie(movie, this));
            }
        }

        public bool HasUnsavedChanges() {
            return _mvc.ChangeTracker.HasChanges();
        }

        public void SaveChanges() {
            try {
                _mvc.SaveChanges();
            }
            catch (OptimisticConcurrencyException e) {
            }
            catch (Exception e) {
                throw;
            }

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

            try {
                _mvc.SaveChanges();
            }
            catch (Exception e) {
                throw;
            }

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