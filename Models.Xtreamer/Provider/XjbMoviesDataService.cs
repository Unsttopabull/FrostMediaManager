using System;
using System.Collections.Generic;
using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Xtreamer.DB;

namespace Frost.Providers.Xtreamer.Provider {
    public class XjbMoviesDataService : IMoviesDataService {
        private readonly XjbEntities _xjb;

        public XjbMoviesDataService() {
            _xjb = new XjbEntities();
        }

        public IEnumerable<IMovie> Movies { get; private set; }
        public IEnumerable<IFile> Files { get; private set; }
        public IEnumerable<IVideo> Videos { get; private set; }
        public IEnumerable<IAudio> Audios { get; private set; }
        public IEnumerable<ISubtitle> Subtitles { get; private set; }
        public ISubtitle AddSubtitle(IMovie movie, ISubtitle subtitle) {
            throw new NotImplementedException();
        }

        public void RemoveSubtitle(IMovie selectedMovie, ISubtitle observedSubtitle) {
            throw new NotImplementedException();
        }

        public IEnumerable<IArt> Art { get; private set; }
        public IEnumerable<ICountry> Countries { get; private set; }
        public ICountry AddCountry(IMovie movie, ICountry country) {
            throw new NotImplementedException();
        }

        public void RemoveCountry(IMovie movie, ICountry country) {
            throw new NotImplementedException();
        }

        public IEnumerable<IStudio> Studios { get; private set; }
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

        public IEnumerable<IRating> Ratings { get; private set; }
        public IEnumerable<IPlot> Plots { get; private set; }
        public void RemovePlot(IMovie movie, IPlot plot) {
            throw new NotImplementedException();
        }

        public IPlot AddPlot(IMovie movie, IPlot plot) {
            throw new NotImplementedException();
        }

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

        public IEnumerable<IAward> Awards { get; private set; }
        public IEnumerable<IPromotionalVideo> PromotionalVideos { get; private set; }
        public IEnumerable<ICertification> Certifications { get; private set; }
        public IEnumerable<IMovieSet> Sets { get; private set; }
        public IMovieSet AddSet(IMovieSet set) {
            throw new NotImplementedException();
        }

        public void RemoveSet(IMovieSet set) {
            throw new NotImplementedException();
        }

        public IEnumerable<ILanguage> Languages { get; private set; }
        public IEnumerable<ISpecial> Specials { get; private set; }
        public ISpecial AddSpecial(ISpecial special) {
            throw new NotImplementedException();
        }

        public void RemoveSpecial(ISpecial special) {
            throw new NotImplementedException();
        }

        public IEnumerable<IPerson> People { get; private set; }
        public IPerson AddPerson(IPerson person) {
            throw new NotImplementedException();
        }

        public IPerson AddDirector(IMovie movie, IPerson director) {
            throw new NotImplementedException();
        }

        public void RemoveDirector(IMovie movie, IPerson director) {
            throw new NotImplementedException();
        }

        public IEnumerable<IActor> Actors { get; private set; }
        public IActor AddActor(IMovie movie, IActor actor) {
            throw new NotImplementedException();
        }

        public void RemoveActor(IMovie movie, IActor actor) {
            throw new NotImplementedException();
        }

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
                if (_xjb != null) {
                    _xjb.Dispose();
                }

                if (!finalizer) {
                    GC.SuppressFinalize(this);
                }
                IsDisposed = true;
            }
        }

        ~XjbMoviesDataService() {
            Dispose(true);
        }

        #endregion
    }
}
