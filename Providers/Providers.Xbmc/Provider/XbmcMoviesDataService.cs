using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Xbmc.DB;
using Frost.Providers.Xbmc.DB.Actor;
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
            _xbmc = new XbmcContainer();

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        public XbmcStudio FindStudio(IStudio studio, bool createIfNotFound) {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        internal XbmcPerson FindPerson(IPerson actor, bool createIfNotFound) {
            throw new NotImplementedException();
        }

        #endregion

        #region Actors

        public IEnumerable<IActor> Actors { get; private set; }

        internal XbmcMovieActor FindActor(IActor actor, bool createIfNotFound) {
            throw new NotImplementedException();
        }

        #endregion

        public bool HasUnsavedChanges() {
            return false;
        }

        public void SaveChanges() {
            
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
