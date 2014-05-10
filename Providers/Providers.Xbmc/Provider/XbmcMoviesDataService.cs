using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.Providers.Xbmc.DB;
using Frost.Providers.Xbmc.DB.Art;
using Frost.Providers.Xbmc.DB.People;
using Frost.Providers.Xbmc.DB.StreamDetails;
using Frost.Providers.Xbmc.Proxies;

namespace Frost.Providers.Xbmc.Provider {
    public class XbmcMoviesDataService : IMoviesDataService {
        private readonly XbmcContainer _xbmc;
        private ObservableCollection<IMovie> _movies;
        private IEnumerable<IMovieSet> _sets;
        private IEnumerable<ICountry> _countries;
        private IEnumerable<IStudio> _studios;
        private IEnumerable<IPerson> _people;
        private IEnumerable<IGenre> _genres;

        public XbmcMoviesDataService() {
            string dbLoc = XbmcContainer.FindXbmcDB();
            _xbmc = !string.IsNullOrEmpty(dbLoc) 
                ? new XbmcContainer(dbLoc) 
                : new XbmcContainer();

            //_xbmc.Database.Log = Console.WriteLine;
        }

        public ObservableCollection<IMovie> Movies {
            get {
                if (_movies == null) {
                    _xbmc.Movies
                         .Include("Path")
                         .Include("Set")
                         .Include("Genres")
                         .Include("Countries")
                         .Include("Studios")
                         .Include("File")
                         .Include("Actors")
                         .Include("Actors.Person")
                         .Load();
                    _movies = new ObservableCollection<IMovie>(_xbmc.Movies.Local.Select(m => new XbmcMovie(m, this)));
                }
                return _movies;
            }
        }

        public bool RemoveMovie(IMovie movie) {
            if (!(movie is XbmcMovie)) {
                return false;
            }

            XbmcMovie m = movie as XbmcMovie;

            int rowsChanged;
            try {
                rowsChanged = _xbmc.Database.ExecuteSqlCommand("DELETE FROM movie WHERE idMovie = {0};", m.Id);
            }
            catch {
                return false;
            }

            if (rowsChanged > 0) {
                Movies.Remove(movie);
                return true;
            }
            return false;
        }

        #region Subtitles

        public XbmcSubtitleDetails FindSubtitle(ISubtitle subtitle, bool createIfNotFound) {
            XbmcSubtitleDetails p = null;

            if (subtitle.Id > 0) {
                XbmcDbStreamDetails s = _xbmc.StreamDetails.Find(subtitle.Id);
                if (s != null) {
                    return new XbmcSubtitleDetails(s);
                }

                return createIfNotFound 
                    ? new XbmcSubtitleDetails(subtitle) 
                    : null;
            }

            if (subtitle.Language != null && subtitle.Language.ISO639 != null) {
                XbmcDbStreamDetails sd = _xbmc.StreamDetails
                                              .FirstOrDefault(pr => pr.Type == StreamType.Subtitle &&
                                                                    pr.SubtitleLanguage == subtitle.Language.ISO639.Alpha3);
                if (sd != null) {
                    p = new XbmcSubtitleDetails(sd);
                }
            }

            if (p == null && createIfNotFound) {
                p = new XbmcSubtitleDetails(subtitle);
            }
            return p;      
        }


        #endregion

        internal XbmcArt FindArt(IArt art, bool createNotFound) {
            if (art.Id > 0) {
                XbmcArt xbmcArt = _xbmc.Art.Find(art.Id);
                if (xbmcArt == null && createNotFound) {
                    return new XbmcArt(art);
                }
                return xbmcArt;
            }

            XbmcArt dbArt = _xbmc.Art.FirstOrDefault(a => a.Url == art.Path);
            if (dbArt == null && createNotFound) {
                return new XbmcArt(art);
            }
            return dbArt;
        }

        #region Countries

        public IEnumerable<ICountry> Countries {
            get {
                if (_countries == null) {
                    _xbmc.Countries.Load();
                    _countries = _xbmc.Countries.Local;
                }
                return _countries;
            }
        }

        public XbmcCountry FindCountry(ICountry country, bool createIfNotFound) {
            XbmcCountry c;
            if (country.Id > 0) {
                c = _xbmc.Countries.Find(country.Id);
                if (c != null && (c.Name == country.Name)) {
                    return c;
                }

                return createIfNotFound
                    ? new XbmcCountry(country)
                    : null;
            }

            c = _xbmc.Countries.FirstOrDefault(pr => (country.ISO3166 != null && pr.ISO3166.Alpha3 == country.ISO3166.Alpha3) || pr.Name == country.Name);
            if (c == null && createIfNotFound) {
                return new XbmcCountry(country);
            }
            return c;
        }

        #endregion

        #region Studios

        public IEnumerable<IStudio> Studios {
            get {
                if (_studios == null) {
                    _xbmc.Studios.Load();
                    _studios = _xbmc.Studios.Local;
                }
                return _studios;
            }
        }

        public XbmcStudio FindStudio(IStudio studio, bool createNotFound) {
            return FindHasName<IStudio, XbmcStudio>(studio, createNotFound);
        }

        #endregion

        #region Genres

        public IEnumerable<IGenre> Genres {
            get {
                if (_genres == null) {
                    _xbmc.Genres.Load();
                    _genres = _xbmc.Genres.Local;
                }
                return _genres;
            }
        }

        public XbmcGenre FindGenre(IGenre genre, bool createIfNotFound) {
            return FindHasName<IGenre, XbmcGenre>(genre, createIfNotFound);
        }

        #endregion

        public IEnumerable<IAward> Awards { get; private set; }

        #region Sets

        public IEnumerable<IMovieSet> Sets {
            get {
                if (_sets == null) {
                    _xbmc.Sets.Load();
                    _sets = _xbmc.Sets.Local;
                }
                return _sets;
            }
        }

        internal XbmcSet FindSet(IMovieSet set, bool createIfNotFound) {
            return FindHasName<IMovieSet, XbmcSet>(set, createIfNotFound);
        }

        #endregion

        public IEnumerable<ILanguage> Languages { get; private set; }

        public IEnumerable<ISpecial> Specials { get; private set; }

        #region People

        public IEnumerable<IPerson> People {
            get {
                if (_people == null) {
                    _xbmc.People.Load();
                    _people = _xbmc.People.Local;
                }
                return _people;
            }
        }

        internal XbmcPerson FindPerson(IPerson person, bool createIfNotFound) {
            if (person == null) {
                return null;
            }

            XbmcPerson p;
            if (person.Id > 0) {
                p = _xbmc.People.Find(person.Id);
                if (p != null && p.Name == person.Name) {
                    return p;
                }

                return createIfNotFound
                    ? new XbmcPerson(person)
                    : null;
            }

            p = _xbmc.People.FirstOrDefault(pr => pr.Name == person.Name);
            if (p == null && createIfNotFound) {
                p = new XbmcPerson(person);
            }
            return p;
        }

        #endregion

        private TSet FindHasName<TEntity, TSet>(TEntity hasName, bool createIfNotFound) where TEntity : class, IHasName, IMovieEntity
                                                                                        where TSet : class, IHasName, IMovieEntity {
            DbSet<TSet> set = _xbmc.Set<TSet>();
            if (hasName.Id > 0) {
                TSet find = set.Find(hasName.Id);
                if ((find == null || find.Name != hasName.Name)) {
                    if (createIfNotFound) {
                        find = set.Create();
                        find.Name = hasName.Name;
                    }
                    else {
                        return null;
                    }
                }
                return find;
            }

            TSet hn = set.FirstOrDefault(n => n.Name == hasName.Name);
            if (hn == null && createIfNotFound) {
                hn = set.Create();
                hn.Name = hasName.Name;
            }
            return hn;            
        }
        
        public void SaveDetected(MovieInfo movieInfo) {
            XbmcMovieSaver ms = new XbmcMovieSaver(movieInfo, _xbmc);
            XbmcDbMovie xbmcDbMovie= ms.Save(false);
            if (xbmcDbMovie != null) {
                _movies.Add(new XbmcMovie(xbmcDbMovie, this));
            }
        }

        public bool HasUnsavedChanges() {
            return _xbmc.ChangeTracker.HasChanges();
        }

        public void SaveChanges() {
            _xbmc.SaveChanges();
            XbmcMovieSaver.Reset();
        }

        #region IDisposable

        public bool IsDisposed { get; private set; }

        public void Dispose() {
            Dispose(false);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        private void Dispose(bool finalizer) {
            if (!IsDisposed) {
                if (_xbmc != null) {
                    _xbmc.Dispose();
                }

                if (!finalizer) {
                    GC.SuppressFinalize(this);
                }
                IsDisposed = true;
            }
        }

        ~XbmcMoviesDataService() {
            Dispose(true);
        }

        #endregion
    }
}
