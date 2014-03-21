using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Xbmc.DB;

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
                    _movies = _xbmc.Movies.Local;
                }
                return _movies;
            }
        }

        public IEnumerable<IFile> Files { get; private set; }
        public IEnumerable<IVideo> Videos { get; private set; }
        public IEnumerable<IAudio> Audios { get; private set; }

        public IEnumerable<ISubtitle> Subtitles { get; private set; }

        public IEnumerable<IArt> Art {
            get {
                if (_art == null) {
                    _xbmc.Art.Load();
                    _art = _xbmc.Art.Local;
                }
                return _art;
            }
        }

        public IEnumerable<ICountry> Countries {
            get {
                if (_countries == null) {
                    _xbmc.Countries.Load();
                    _countries = _xbmc.Countries.Local;
                }
                return _countries;
            }
        }

        public IEnumerable<IStudio> Studios {
            get {
                if (_studios == null) {
                    _xbmc.Studios.Load();
                    _studios = _xbmc.Studios.Local;
                }
                return _studios;
            }
        }

        public IEnumerable<IRating> Ratings { get { return null; } }

        public IEnumerable<IPlot> Plots {
            get { return _plots ?? (_plots = Movies.SelectMany(m => m.Plots)); }
        }

        public IEnumerable<IGenre> Genres {
            get {
                if (_genres == null) {
                    _xbmc.Genres.Load();
                    _genres = _xbmc.Genres.Local;
                }
                return _genres;
            }
        }

        public IEnumerable<IAward> Awards { get; private set; }
        public IEnumerable<IPromotionalVideo> PromotionalVideos { get; private set; }
        public IEnumerable<ICertification> Certifications { get; private set; }

        public IEnumerable<IMovieSet> Sets {
            get {
                if (_sets == null) {
                    _xbmc.Sets.Load();
                    _sets = _xbmc.Sets.Local;
                }
                return _sets;
            }
        }

        public IEnumerable<ILanguage> Languages { get; private set; }

        public IEnumerable<ISpecial> Specials { get; private set; }

        public IEnumerable<IPerson> People {
            get {
                if (_people == null) {
                    _xbmc.People.Load();
                    _people = _xbmc.People.Local;
                }
                return _people;
            }
        }

        public IEnumerable<IActor> Actors { get; private set; }

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
