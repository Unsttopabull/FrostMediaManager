using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Frost.Common;
using Frost.Common.Models;
using Frost.PHPtoNET;
using Frost.Providers.Xtreamer.DB;
using Frost.Providers.Xtreamer.PHP;
using Frost.Providers.Xtreamer.Proxies;

namespace Frost.Providers.Xtreamer.Provider {

    public class XjbMoviesDataService : IMoviesDataService {
        private readonly XjbEntities _xjb;
        private readonly string _xtreamerPath;
        private ObservableCollection<XtMovie> _movies;
        private IEnumerable<IPerson> _people;

        public XjbMoviesDataService() {
            string xjbDb = FindDb.FindXjbDB();
            if (xjbDb != null) {
                _xtreamerPath = FindDb.FindXjbDriveLocation(xjbDb);

                if (xjbDb.StartsWith("\\\\")) {
                    xjbDb = "\\\\" + xjbDb;
                }
                _xjb = new XjbEntities(xjbDb);
            }
            else {
                _xjb = new XjbEntities();
            }
        }

        public IEnumerable<IMovie> Movies {
            get {
                if (_movies == null) {
                    _movies = new ObservableCollection<XtMovie>();

                    var serialized = _xjb.Movies.Select(m => m.MovieVo);
                    PHPDeserializer2 deser = new PHPDeserializer2();
                    foreach (string serializedMovie in serialized) {
                        using (PHPSerializedStream pser = new PHPSerializedStream(serializedMovie, Encoding.UTF8)) {
                            try {
                                XjbPhpMovie movie = deser.Deserialize<XjbPhpMovie>(pser);
                                if (movie != null) {
                                    _movies.Add(new XtMovie(movie, _xtreamerPath));
                                }
                            }
                            catch(Exception e) {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                }
                return _movies;
            }
        }

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

        public IEnumerable<IGenre> Genres {
            get { return _movies.SelectMany(m => m.Genres).Distinct(); }
        }

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
        public IEnumerable<IMovieSet> Sets { get { return null; } }

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

        public IEnumerable<IPerson> People {
            get {
                if (_people == null) {
                    _xjb.Persons.Load();
                    _people = _xjb.Persons.Local;
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

        public IEnumerable<IActor> Actors { get; private set; }

        public IActor AddActor(IMovie movie, IActor actor) {
            throw new NotImplementedException();
        }

        public void RemoveActor(IMovie movie, IActor actor) {
            throw new NotImplementedException();
        }

        public bool HasUnsavedChanges() {
            return false;
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