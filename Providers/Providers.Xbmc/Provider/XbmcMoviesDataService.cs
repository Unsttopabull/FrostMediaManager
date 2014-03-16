using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Xbmc.DB;
using Frost.Providers.Xbmc.DB.Actor;
using Frost.Providers.Xbmc.DB.Art;

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

        public XbmcMoviesDataService() {
            _xbmc = new XbmcContainer();

            //_xbmc.Database.Log = Console.WriteLine;
        }

        public IEnumerable<IMovie> Movies {
            get {
                if (_movies == null) {
                    _xbmc.Movies
                         .Include("Path")
                         .Load();
                    _movies = _xbmc.Movies.Local;
                }
                return _movies;
            }
        }

        public IEnumerable<IFile> Files { get; private set; }
        public IEnumerable<IVideo> Videos { get; private set; }
        public IEnumerable<IAudio> Audios { get; private set; }

        #region Subtitles

        public IEnumerable<ISubtitle> Subtitles { get; private set; }

        public ISubtitle AddSubtitle(IMovie movie, ISubtitle subtitle) {
            throw new NotImplementedException();
        }

        public void RemoveSubtitle(IMovie selectedMovie, ISubtitle observedSubtitle) {
            throw new NotImplementedException();
        }

        #endregion

        public IEnumerable<IArt> Art {
            get {
                if (_art != null) {
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

        public ICountry AddCountry(IMovie movie, ICountry country) {
            throw new NotImplementedException();
        }

        public void RemoveCountry(IMovie movie, ICountry country) {
            throw new NotImplementedException();
        }

        #endregion

        #region Studios

        public IEnumerable<IStudio> Studios {
            get {
                if (_studios != null) {
                    _xbmc.Studios.Load();
                    _studios = _xbmc.Studios.Local;
                }
                return _studios;
            }
        }

        public IStudio AddStudio(IStudio studio) {
            throw new NotImplementedException();
        }

        public void RemoveStudio(IStudio studio) {
            throw new NotImplementedException();
        }

        public IStudio AddStudio(IMovie movie, IStudio studio) {
            throw new NotImplementedException();
        }

        public void RemoveStudio(IMovie movie, IStudio studio) {
            throw new NotImplementedException();
        }

        #endregion

        public IEnumerable<IRating> Ratings { get; private set; }

        #region Plots

        public IEnumerable<IPlot> Plots {
            get {
                if (_plots != null) {
                    _plots = Movies.SelectMany(m => m.Plots);
                }
                return _plots;
            }
        }

        public void RemovePlot(IMovie movie, IPlot plot) {
            throw new NotImplementedException();
        }

        public IPlot AddPlot(IMovie movie, IPlot plot) {
            throw new NotImplementedException();
        }

        #endregion

        #region Genres

        public IEnumerable<IGenre> Genres { get; private set; }

        public IGenre AddGenre(IGenre genre) {
            throw new NotImplementedException();
        }

        public void RemoveGenre(IGenre genre) {
            throw new NotImplementedException();
        }

        public IGenre AddGenre(IMovie movie, IGenre genre) {
            throw new NotImplementedException();
        }

        public void RemoveGenre(IMovie movie, IGenre genre) {
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

        public IMovieSet AddSet(IMovieSet set) {
            throw new NotImplementedException();
        }

        public void RemoveSet(IMovieSet set) {
            throw new NotImplementedException();
        }

        #endregion

        public IEnumerable<ILanguage> Languages { get; private set; }

        #region Specials

        public IEnumerable<ISpecial> Specials { get; private set; }

        public ISpecial AddSpecial(ISpecial special) {
            throw new NotImplementedException();
        }

        public void RemoveSpecial(ISpecial special) {
            throw new NotImplementedException();
        }

        #endregion

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

        public IPerson AddPerson(IPerson person) {
            throw new NotImplementedException();
        }

        public IPerson AddDirector(IMovie movie, IPerson director) {
            throw new NotImplementedException();
        }

        public void RemoveDirector(IMovie movie, IPerson director) {
            throw new NotImplementedException();
        }

        #endregion

        #region Actors

        public IEnumerable<IActor> Actors { get; private set; }

        public IActor AddActor(IMovie movie, IActor actor) {
            throw new NotImplementedException();
        }

        public void RemoveActor(IMovie movie, IActor actor) {
            throw new NotImplementedException();
        }

        #endregion

        public bool HasUnsavedChanges() {
            throw new NotImplementedException();
        }

        public void SaveChanges() {
            throw new NotImplementedException();
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
