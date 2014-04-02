using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Frost.Common;
using Frost.Common.Comparers;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.PHPtoNET;
using Frost.Providers.Xtreamer.DB;
using Frost.Providers.Xtreamer.PHP;
using Frost.Providers.Xtreamer.Proxies;
using Frost.Providers.Xtreamer.Proxies.ChangeTrackers;

namespace Frost.Providers.Xtreamer.Provider {

    internal enum Person {
        Director,
        Writer
    }

    /// <summary>Data service for Xtreamer movie jukebox provider.</summary>
    public class XjbMoviesDataService : IMoviesDataService {
        private readonly XjbEntities _xjb;
        private readonly string _xtreamerPath;
        private ObservableCollection<XtMovie> _movies;
        private readonly PersonEqualityComparer _personComparer;

        public XjbMoviesDataService() {
            _personComparer = new PersonEqualityComparer();

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
                            catch (Exception e) {
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
            get { return _movies.SelectMany(m => m.Genres).Distinct<IGenre>(new HasNameEqualityComparer()); }
        }

        public IEnumerable<IAward> Awards { get; private set; }
        public IEnumerable<IPromotionalVideo> PromotionalVideos { get; private set; }
        public IEnumerable<ICertification> Certifications { get; private set; }

        public IEnumerable<IMovieSet> Sets {
            get { return null; }
        }

        public IEnumerable<ILanguage> Languages { get; private set; }
        public IEnumerable<ISpecial> Specials { get; private set; }

        public IEnumerable<IPerson> People {
            get {
                return _movies.SelectMany(m => m.Actors)
                              .Union(_movies.SelectMany(m => m.Writers), _personComparer)
                              .Union(_movies.SelectMany(m => m.Directors), _personComparer)
                              .Where(p => !string.IsNullOrEmpty(p.Name));
            }
        }

        #region Saving

        public void SaveDetected(IEnumerable<MovieInfo> movieInfos) {
        }

        public bool HasUnsavedChanges() {
            return _movies.Any(m => m.IsDirty) || People.Cast<XtPerson>().Any(p => p.IsDirty);
        }

        public void SaveChanges() {
            SaveChangedPeople(_movies.SelectMany(m => m.GetChangedDirectors()).Union(_movies.SelectMany(m => m.GetChangedWriters())));
            SaveChangedActors(_movies.SelectMany(m => m.GetChangedActors()));

            IEnumerable<XtMovie> changedMovies = _movies.Where(m => m.IsDirty);

            PHPSerializer phpSerializer = new PHPSerializer();
            foreach (XtMovie changedMovie in changedMovies) {
                SaveMovie(changedMovie, phpSerializer);
            }
            _xjb.SaveChanges();
        }

        private void SaveChangedPeople(IEnumerable<XtPerson> changedPeople) {
            foreach (XtPerson person in changedPeople) {
                if (person == null) {
                    continue;
                }

                XjbPerson a = _xjb.People.Find(person.Id);
                if (a == null) {
                    continue;
                }

                a.Name = person.Name;
            }
        }

        private void SaveChangedActors(IEnumerable<XtActor> changedActors) {
            foreach (XtActor actor in changedActors) {
                if (actor == null) {
                    continue;
                }

                XjbActor a = _xjb.MoviesPersons
                                 .OfType<XjbActor>()
                                 .FirstOrDefault(p => p.PersonId == actor.Id && actor.Character == p.Character);

                if (a == null) {
                    continue;
                }

                HashSet<string> changed = actor.GetChangedProperties();
                foreach (string prop in changed) {
                    if (prop == "Character") {
                        a.Character = actor.Character;
                    }

                    if (prop == "Name") {
                        a.Person.Name = actor.Name;
                    }
                }
            }
        }

        private void SaveMovie(XtMovie changedMovie, PHPSerializer phpSerializer) {
            HashSet<string> changedProperties = changedMovie.GetChangedProperties();
            XjbMovie m = _xjb.Movies.Find(changedMovie.Id);

            try {
                m.MovieVo = phpSerializer.Serialize(changedMovie.ObservedEntity);
            }
            catch {
                return;
            }

            if (changedProperties.Count == 1 && changedProperties.Contains("Subtitle")) {
                return;
            }

            foreach (string property in changedProperties) {
                switch (property) {
                    case "TmdbID":
                        short tmdb;
                        if (short.TryParse(changedMovie.TmdbID, NumberStyles.Integer, CultureInfo.InvariantCulture, out tmdb)) {
                            m.TmdbID = tmdb;
                        }
                        break;
                    case "ImdbID":
                        m.ImdbID = changedMovie.ImdbID;
                        break;
                    case "Title":
                        m.Title = changedMovie.Title;
                        break;
                    case "SortTitle":
                        m.SortTitle = changedMovie.SortTitle;
                        break;
                    case "OriginalTitle":
                        m.OriginalTitle = changedMovie.OriginalTitle;
                        break;
                    case "ReleaseYear":
                        m.Year = changedMovie.ReleaseYear;
                        break;
                    case "RatingAverage":
                        m.Rating = (int?) changedMovie.RatingAverage;
                        break;
                    case "Runtime":
                        m.Runtime = changedMovie.Runtime;
                        break;
                    case "Plots":
                        XtPlot plot = (XtPlot) changedMovie.Plots.FirstOrDefault();
                        if (plot != null && plot.GetChangedProperties().Contains("Full")) {
                            m.Plot = plot.Full;
                        }
                        break;
                    case "Art":
                        if (!string.IsNullOrEmpty(changedMovie.ObservedEntity.CoverPath)) {
                            m.HasCover = true;
                        }
                        if (changedMovie.ObservedEntity.Fanart.Length > 0) {
                            m.HasFanart = true;
                        }
                        break;
                    case "Genres":
                        ChangeTrackingCollection<XtGenre> genres = (ChangeTrackingCollection<XtGenre>) changedMovie.Genres;
                        SaveGenreChanges(m, genres.AddedItems, genres.RemovedItems);
                        break;
                    case "Actors":
                        ChangeTrackingCollection<XtActor> actors = (ChangeTrackingCollection<XtActor>) changedMovie.Actors;
                        SaveActors(m, actors.AddedItems, actors.RemovedItems);
                        break;
                    case "Directors":
                        ChangeTrackingCollection<XtPerson> directors = (ChangeTrackingCollection<XtPerson>) changedMovie.Directors;
                        SavePeople(m, directors.AddedItems, directors.RemovedItems, Person.Director);
                        break;
                    case "Writers":
                        ChangeTrackingCollection<XtPerson> writers = (ChangeTrackingCollection<XtPerson>) changedMovie.Writers;
                        SavePeople(m, writers.AddedItems, writers.RemovedItems, Person.Writer);
                        break;
                }
            }
        }

        private void SaveActors(XjbMovie m, IEnumerable<XtActor> addedItems, IEnumerable<XtActor> removedItems) {
            foreach (XtActor actor in addedItems) {
                XjbMoviePerson dbActor = _xjb.MoviesPersons.OfType<XjbActor>().FirstOrDefault(a => a.Person.Name == actor.Name && a.Character == actor.Character);

                if(dbActor == null){
                    dbActor = new XjbActor(FindPerson(actor, true), actor.Character);
                }

                m.Cast.Add(dbActor);
            }

            foreach (XtActor actor in removedItems) {
                XjbActor a = actor.Id > 0
                    ? m.Cast.OfType<XjbActor>().FirstOrDefault(p => p.PersonId == actor.Id && p.Character == actor.Character)
                    : m.Cast.OfType<XjbActor>().FirstOrDefault(p => p.Person.Name == actor.Name && p.Character == actor.Character);

                if (a != null) {
                    m.Cast.Remove(a);
                }
            }
        }

        private XjbPerson FindPerson(XtPerson person, bool createNotFound) {
            XjbPerson p;
            if (person.Id > 0) {
                p = _xjb.People.Find(person.Id);

                if (p != null) {
                    return p;
                }
            }

            p = _xjb.People.FirstOrDefault(pr => pr.Name == person.Name);
            if (p == null && createNotFound) {
                p = new XjbPerson(person.Name);
            }
            return p;
        }

        private void SavePeople(XjbMovie m, IEnumerable<XtPerson> addedItems, IEnumerable<XtPerson> removedItems, Person personType) {
            foreach (XjbPerson person in addedItems.Select(p => FindPerson(p, true))) {
                if (personType == Person.Director) {
                    m.Cast.Add(new XjbDirector(person));
                }
                else {
                    m.Cast.Add(new XjbWriter(person));
                }
            }

            foreach (XtPerson person in removedItems) {
                switch (personType) {
                    case Person.Director:
                        if (person.Id > 0) {
                            m.Cast.RemoveWhere(p => p is XjbDirector && p.Id == person.Id);
                        }
                        else {
                            m.Cast.RemoveWhere(p => p is XjbDirector && p.Person.Name == person.Name);
                        }
                        break;
                    case Person.Writer:
                        if (person.Id > 0) {
                            m.Cast.RemoveWhere(p => p is XjbWriter && p.Id == person.Id);
                        }
                        else {
                            m.Cast.RemoveWhere(p => p is XjbWriter && p.Person.Name == person.Name);
                        }
                        break;
                }
            }
        }

        private void SaveGenreChanges(XjbMovie m, IEnumerable<XtGenre> addedItems, IEnumerable<XtGenre> removedItems) {
            foreach (XtGenre genre in addedItems) {
                XjbGenre g;
                if (genre.Id > 0) {
                    g = _xjb.Genres.Find(genre.Id);
                    m.Genres.Add(g);
                }
                else {
                    g = _xjb.Genres.FirstOrDefault(gnr => gnr.Name == genre.Name);
                    m.Genres.Add(g ?? new XjbGenre(genre));
                }
            }

            foreach (XtGenre genre in removedItems) {
                if (genre.Id > 0) {
                    m.Genres.RemoveWhere(g => g.Id == genre.Id);
                }
                else {
                    m.Genres.RemoveWhere(g => g.Name == genre.Name);
                }
            }
        }

        #endregion

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