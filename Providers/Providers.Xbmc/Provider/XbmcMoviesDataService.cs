using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.Providers.Xbmc.DB;
using Frost.Providers.Xbmc.DB.People;
using Frost.Providers.Xbmc.DB.StreamDetails;
using Frost.Providers.Xbmc.Proxies;

namespace Frost.Providers.Xbmc.Provider {
    public class XbmcMoviesDataService : IMoviesDataService {
        private readonly XbmcContainer _xbmc;
        private IEnumerable<IMovie> _movies;
        private IEnumerable<IMovieSet> _sets;
        private IEnumerable<ICountry> _countries;
        private IEnumerable<IStudio> _studios;
        private IEnumerable<IArt> _art;
        private IEnumerable<IPlot> _plots;
        private IEnumerable<IPerson> _people;
        private IEnumerable<IGenre> _genres;

        public XbmcMoviesDataService() {
            string dbLoc = XbmcContainer.FindXbmcDB();
            _xbmc = !string.IsNullOrEmpty(dbLoc)
                ? new XbmcContainer(dbLoc) 
                : new XbmcContainer();

            //_xbmc.Database.Log = Console.WriteLine;
        }

        public IEnumerable<IMovie> Movies {
            get {
                if (_movies == null) {
                    _xbmc.Movies
                         .Include("Path")
                         .Include("Art")
                         .Include("Set")
                         .Include("Genres")
                         .Include("Countries")
                         .Include("Studios")
                         .Include("File")
                         .Load();
                    _movies = _xbmc.Movies.Local.Select(m => new XbmcMovie(m, this));
                }
                return _movies;
            }
        }

        public IEnumerable<IFile> Files { get; private set; }
        public IEnumerable<IVideo> Videos { get; private set; }
        public IEnumerable<IAudio> Audios { get; private set; }

        #region Subtitles

        public IEnumerable<ISubtitle> Subtitles { get; private set; }

        public XbmcSubtitleDetails FindSubtitle(ISubtitle subtitle, bool createIfNotFound) {
            XbmcSubtitleDetails p;
            if (subtitle.Id > 0) {
                p = (XbmcSubtitleDetails) _xbmc.StreamDetails.Find(subtitle.Id);
                if (p != null) {
                    return p;
                }

                if (createIfNotFound) {
                    return (XbmcSubtitleDetails) _xbmc.StreamDetails.Add(new XbmcSubtitleDetails(subtitle, FindFile(subtitle.File, true)));
                }
                return null;
            }

            if (subtitle.Language != null && subtitle.Language.ISO639 != null) {
                p = _xbmc.StreamDetails
                         .OfType<XbmcSubtitleDetails>()
                         .FirstOrDefault(pr => pr.Language == subtitle.Language.ISO639.Alpha3 &&
                                               pr.FileId == subtitle.File.Id);
            }
            else {
                p = _xbmc.StreamDetails
                         .OfType<XbmcSubtitleDetails>()
                         .FirstOrDefault(pr => pr.File.Id == subtitle.File.Id);
            }

            if (p == null && createIfNotFound) {
                p = (XbmcSubtitleDetails) _xbmc.StreamDetails.Add(new XbmcSubtitleDetails(subtitle, FindFile(subtitle.File, true)));
            }
            return p;      
        }

        private XbmcFile FindFile(IFile file, bool createIfNotFound) {
            XbmcFile f;
            if (file.Id > 0) {
                f = _xbmc.Files.Find(file.Id);
                return f ?? _xbmc.Files.Add(new XbmcFile(file));
            }

            string fileNameString = XbmcFile.GetFileNamesString(new[] { file.Name });

            f = _xbmc.Files.FirstOrDefault(xf => fileNameString == xf.FileNameString);
            if (f == null && createIfNotFound) {
                return _xbmc.Files.Add(new XbmcFile(file));
            }
            return f;
        }

        #endregion

        public IEnumerable<IArt> Art {
            get {
                if (_art == null) {
                    _xbmc.Art.Load();
                    _art = _xbmc.Art.Local;
                }
                return _art;
            }
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
                    ? _xbmc.Countries.Add(new XbmcCountry(country))
                    : null;
            }

            c = _xbmc.Countries.FirstOrDefault(pr => (country.ISO3166 != null && pr.ISO3166.Alpha3 == country.ISO3166.Alpha3) || pr.Name == country.Name);
            if (c == null && createIfNotFound) {
                _xbmc.Countries.Add(new XbmcCountry(country));
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

        public IEnumerable<IRating> Ratings { get { return null; } }

        public IEnumerable<IPlot> Plots {
            get { return _plots ?? (_plots = Movies.SelectMany(m => m.Plots)); }
        }

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
        public IEnumerable<IPromotionalVideo> PromotionalVideos { get; private set; }
        public IEnumerable<ICertification> Certifications { get; private set; }

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
                    ? _xbmc.People.Add(new XbmcPerson(person))
                    : null;
            }

            p = _xbmc.People.FirstOrDefault(pr => pr.Name == person.Name);
            if (p == null && createIfNotFound) {
                p = _xbmc.People.Add(new XbmcPerson(person));
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

                        find = set.Add(find);
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

                hn = set.Add(hn);
            }
            return hn;            
        }
        
        public void SaveDetected(IEnumerable<MovieInfo> movieInfos) {
            
        }

        public bool HasUnsavedChanges() {
            return _xbmc.ChangeTracker.HasChanges();
        }

        public void SaveChanges() {
            _xbmc.SaveChanges();
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
