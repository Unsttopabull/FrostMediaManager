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

        public IEnumerable<IArt> Art { get; private set; }
        public IEnumerable<ICountry> Countries { get; private set; }

        public IEnumerable<IStudio> Studios { get; private set; }

        public IEnumerable<IRating> Ratings { get; private set; }
        public IEnumerable<IPlot> Plots { get; private set; }

        public IEnumerable<IGenre> Genres {
            get { return _movies.SelectMany(m => m.Genres).Distinct(); }
        }

        public IEnumerable<IAward> Awards { get; private set; }
        public IEnumerable<IPromotionalVideo> PromotionalVideos { get; private set; }
        public IEnumerable<ICertification> Certifications { get; private set; }
        public IEnumerable<IMovieSet> Sets { get { return null; } }

        public IEnumerable<ILanguage> Languages { get; private set; }
        public IEnumerable<ISpecial> Specials { get; private set; }

        public IEnumerable<IPerson> People {
            get {
                if (_people == null) {
                    _xjb.Persons.Load();
                    _people = _xjb.Persons.Local;
                }
                return _people;
            }
        }

        public IEnumerable<IActor> Actors { get; private set; }

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