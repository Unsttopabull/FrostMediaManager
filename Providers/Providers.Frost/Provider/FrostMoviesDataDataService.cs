using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Frost.DB;
using Frost.Providers.Frost.DB.Files;
using Frost.Providers.Frost.DB.People;
using Frost.Providers.Frost.Proxies;

namespace Frost.Providers.Frost.Provider {
    public class FrostMoviesDataDataService : IMoviesDataService {
        private readonly FrostDbContainer _mvc;
        private IEnumerable<IMovie> _movies;
        private IEnumerable<IFile> _files;
        private IEnumerable<IVideo> _videos;
        private IEnumerable<IAudio> _audios;
        private IEnumerable<ISubtitle> _subtitles;
        private IEnumerable<IArt> _art;
        private IEnumerable<ICountry> _countries;
        private IEnumerable<IStudio> _studios;
        private IEnumerable<IRating> _ratings;
        private IEnumerable<IPlot> _plots;
        private IEnumerable<IGenre> _genres;
        private IEnumerable<IAward> _awards;
        private IEnumerable<IPromotionalVideo> _promotionalVideos;
        private IEnumerable<ICertification> _certifications;
        private IEnumerable<IMovieSet> _sets;
        private IEnumerable<ILanguage> _languages;
        private IEnumerable<ISpecial> _specials;
        private IEnumerable<IPerson> _people;
        private IEnumerable<IActor> _actors;

        public FrostMoviesDataDataService() {
            _mvc = new FrostDbContainer();
        }

        #region Movies

        public IEnumerable<IMovie> Movies {
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
                    .Include("Plots")
                    //.Include("Directors")
                    //.Include("Countries")
                    //.Include("Audios")
                    //.Include("Videos")
                    .Load();

                _movies = _mvc.Movies.Local.Select(m => new FrostMovie(m, this));
                return _movies;
            }
        }

        #endregion

        public IEnumerable<IFile> Files {
            get {
                if (_files != null) {
                    return _files;
                }

                _mvc.Files.Load();
                _files = _mvc.Files.Local;
                return _files;
            }
        }

        public IEnumerable<IVideo> Videos {
            get {
                if (_videos != null) {
                    return _videos;
                }

                _mvc.VideoDetails
                    .Include("File")
                    .Include("Language")
                    .Load();

                _videos = _mvc.VideoDetails.Local.Select(v => new FrostVideo(v, this));
                return _videos;
            }
        }

        public IEnumerable<IAudio> Audios {
            get {
                if (_audios != null) {
                    return _audios;
                }

                _mvc.AudioDetails
                    .Include("File")
                    .Include("Language")
                    .Load();

                _audios = _mvc.AudioDetails.Local.Select(a => new FrostAudio(a, this));
                return _audios;
            }
        }

        #region Subtitles

        public IEnumerable<ISubtitle> Subtitles {
            get {
                if (_subtitles == null) {
                    _mvc.Subtitles
                        .Include("Language")
                        .Load();

                    _subtitles = _mvc.Subtitles.Local.Select(s => new FrostSubtitle(s, this));
                }
                return _subtitles;
            }
        }

        internal Subtitle FindOrCreateSubtitle(ISubtitle subtitle, bool createIfNotFound) {
            Subtitle p;
            if (subtitle.Id > 0) {
                p = _mvc.Subtitles.Find(subtitle.Id);
                if (p == null || (subtitle.MD5 != null && p.MD5 != subtitle.MD5)) {
                    if (createIfNotFound) {
                        p = _mvc.Subtitles.Add(new Subtitle(subtitle));
                    }
                    else {
                        return null;
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

            if (p == null && createIfNotFound) {
                _mvc.Subtitles.Add(new Subtitle(subtitle));
            }
            return p;             
        }

        #endregion

        public IEnumerable<IArt> Art {
            get {
                if (_art != null) {
                    return _art;
                }

                _mvc.Art.Load();
                _art = _mvc.Art.Local;
                return _art;
            }
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

        internal Country FindOrCreateCountry(ICountry country, bool createIfNotFound) {
            Country c;
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

            c = _mvc.Countries.FirstOrDefault(pr => (country.ISO3166 != null && pr.ISO3166.Alpha3 == country.ISO3166.Alpha3) || pr.Name == country.Name);
            if (c == null && createIfNotFound) {
                _mvc.Countries.Add(new Country(country));
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
            if (studio.Id > 0) {
                Studio s = _mvc.Studios.Find(studio.Id);
                if (s != null && (s.Name == studio.Name)) {
                    return s;
                }

                return createNotFound
                    ? _mvc.Studios.Add(new Studio(studio))
                    : null;
            }

            Studio stud = _mvc.Studios.FirstOrDefault(pr => pr.Name == studio.Name);
            if (stud == null && createNotFound) {
                _mvc.Studios.Add(new Studio(studio));
            }
            return stud;    
        }

        #endregion

        public IEnumerable<IRating> Ratings {
            get {
                if (_ratings != null) {
                    return _ratings;
                }

                _mvc.Ratings.Load();
                _ratings = _mvc.Ratings.Local;
                return _ratings;
            }
        }

        #region Plots

        public IEnumerable<IPlot> Plots {
            get {
                if (_plots != null) {
                    return _plots;
                }

                _mvc.Plots.Load();
                _plots = _mvc.Plots.Local;
                return _plots;
            }
        }

        internal Plot FindOrCreatePlot(IPlot plot, bool createIfNotFound) {
            Plot p;
            if (plot.Id > 0) {
                p = _mvc.Plots.Find(plot.Id);
                if (p == null || (p.Full != plot.Full)) {
                    if (createIfNotFound) {
                        p = _mvc.Plots.Add(new Plot(plot));
                    }
                    else {
                        return null;
                    }
                }
                return p;
            }

            p = _mvc.Plots.FirstOrDefault(pr => plot.Full == pr.Full) ?? _mvc.Plots.Add(new Plot(plot));
            return p; 
        }

        #endregion

        #region Genres

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
        #endregion

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

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get {
                if (_promotionalVideos != null) {
                    return _promotionalVideos;
                }

                _mvc.PromotionalVideos.Load();
                _promotionalVideos = _mvc.PromotionalVideos.Local;
                return _promotionalVideos;
            }
        }

        public IEnumerable<ICertification> Certifications {
            get {
                if (_certifications != null) {
                    return _certifications;
                }

                _mvc.Certifications
                    .Include("Country")
                    .Load();

                _certifications = _mvc.Certifications.Local.Select(c => new FrostCertification(c, this));
                return _certifications;
            }
        }

        #region Sets

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

        #endregion

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
                        c = _mvc.Languages.Add(new Language(language));
                    }
                    else {
                        return null;
                    }
                }
                return c;
            }

            c = _mvc.Languages.FirstOrDefault(pr => (language.ISO639 != null && pr.ISO639.Alpha3 == language.ISO639.Alpha3) || pr.Name == language.Name);
            if (c == null) {
                return null;
            }
            return _mvc.Languages.Add(new Language(language));
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

        internal Person FindOrCreatePerson(IPerson person, bool createIfNotFound) {
            Person p;
            if (person.Id > 0) {
                p = _mvc.People.Find(person.Id);
                if (p == null || p.Name != person.Name) {
                    if (createIfNotFound) {
                        p = _mvc.People.Add(new Person(person));
                    }
                    else {
                        return null;
                    }
                }
                return p;
            }

            p = _mvc.People.FirstOrDefault(pr => (person.ImdbID != null && pr.ImdbID == person.ImdbID) || pr.Name == person.Name);
            if (p == null && createIfNotFound) {
                p = _mvc.People.Add(new Person(person));
            }
            return p;
        }

        #endregion

        #region Actors

        public IEnumerable<IActor> Actors {
            get {
                if (_actors != null) {
                    return _actors;
                }

                _mvc.Actors.Load();
                _actors = _mvc.Actors.Local;
                return _actors;
            }
        }

        internal Actor FindOrCreateActor(IActor actor, bool createIfNotFound) {
            Actor p;
            if (actor.Id > 0) {
                p = _mvc.Actors.Find(actor.Id);
                if (p == null || p.Name != actor.Name) {
                    if (createIfNotFound) {
                        p = _mvc.Actors.Add(new Actor(actor));
                    }
                    else {
                        return null;
                    }
                }
                return p;
            }

            p = _mvc.Actors.FirstOrDefault(pr => (actor.ImdbID != null && pr.ImdbID == actor.ImdbID) || (pr.Name == actor.Name && pr.Character == actor.Character));
            if (p == null && createIfNotFound) {
                p = _mvc.Actors.Add(new Actor(actor));
            }
            return p;               
        }

        #endregion

        internal TSet FindHasName<TEntity, TSet>(TEntity hasName, bool createIfNotFound) where TEntity : class, IHasName, IMovieEntity
                                                                                         where TSet : class, IHasName, IMovieEntity {
            DbSet<TSet> set = _mvc.Set<TSet>();
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

        public bool HasUnsavedChanges() {
            return _mvc.HasUnsavedChanges();
        }

        public void SaveChanges() {
            _mvc.SaveChanges();
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
