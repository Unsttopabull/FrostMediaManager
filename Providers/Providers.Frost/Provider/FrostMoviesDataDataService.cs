using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Frost.DB;
using Frost.Providers.Frost.DB.Files;
using Frost.Providers.Frost.DB.People;

namespace Frost.Providers.Frost.Service {
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

                _movies = _mvc.Movies.Local;
                return _movies;
            }
        }

        private Movie FindMovie(IMovie movie) {
            if (movie.Id > 0) {
                return _mvc.Movies.Find(movie.Id);
            }
            return _mvc.Movies.FirstOrDefault(m => (movie.ImdbID != null && m.ImdbID == movie.ImdbID) || m.Title == movie.Title);
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

                _videos = _mvc.VideoDetails.Local;
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

                _audios = _mvc.AudioDetails.Local;
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

                    _subtitles = _mvc.Subtitles.Local;
                }
                return _subtitles;
            }
        }

        public ISubtitle AddSubtitle(IMovie movie, ISubtitle subtitle) {

            Subtitle sub = FindSubtitle(subtitle);
            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Subtitles.Add(sub);

            return sub;
        }

        public void RemoveSubtitle(IMovie movie, ISubtitle subtitle) {
            Subtitle sub = FindSubtitle(subtitle);
            Movie mv = movie as Movie ?? FindMovie(movie);

            mv.Subtitles.Remove(sub);
        }

        private Subtitle FindSubtitle(ISubtitle subtitle) {
            Subtitle p;
            if (subtitle.Id > 0) {
                p = _mvc.Subtitles.Find(subtitle.Id);
                if (p == null || (subtitle.MD5 != null && p.MD5 != subtitle.MD5)) {
                    p = _mvc.Subtitles.Add(new Subtitle(subtitle));
                }
                return p;
            }

            p = _mvc.Subtitles.FirstOrDefault(pr => (subtitle.MD5 != null && pr.MD5 == subtitle.MD5) ||
                                                    (subtitle.OpenSubtitlesId != null && pr.OpenSubtitlesId == subtitle.OpenSubtitlesId) ||
                                                    (subtitle.PodnapisiId != null && pr.PodnapisiId == subtitle.PodnapisiId) ||
                                                    (subtitle.EmbededInVideo == pr.EmbededInVideo &&
                                                     subtitle.ForHearingImpaired == pr.ForHearingImpaired &&
                                                     subtitle.Encoding == pr.Encoding)
                    ) ?? _mvc.Subtitles.Add(new Subtitle(subtitle));
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

        private Country FindCountry(ICountry country) {
            Country c;
            if (country.Id > 0) {
                c = _mvc.Countries.Find(country.Id);
                if (c == null || (c.Name != country.Name)) {
                    c = _mvc.Countries.Add(new Country(country));
                }
                return c;
            }

            c = _mvc.Countries.FirstOrDefault(pr => (country.ISO3166 != null && pr.ISO3166.Alpha3 == country.ISO3166.Alpha3) || pr.Name == country.Name)
                ?? _mvc.Countries.Add(new Country(country));
            return c;
        }

        public ICountry AddCountry(IMovie movie, ICountry country) {
            Country c = FindCountry(country);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Countries.Add(c);

            return c;
        }

        public void RemoveCountry(IMovie movie, ICountry country) {
            Country c = FindCountry(country);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Countries.Remove(c);
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

        public IStudio AddStudio(IStudio studio) {
            return AddHasName(_mvc.Studios, studio);
        }

        public void RemoveStudio(IStudio studio) {
            RemoveHasName(_mvc.Studios, studio);
        }

        public IStudio AddStudio(IMovie movie, IStudio studio) {
            Studio s = FindStudio(studio);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Studios.Add(s);
            return s;
        }

        private Studio FindStudio(IStudio studio) {
            Studio s;
            if (studio.Id > 0) {
                s = _mvc.Studios.Find(studio.Id);
                if (s == null || (s.Name != studio.Name)) {
                    s = _mvc.Studios.Add(new Studio(studio));
                }
                return s;
            }

            s = _mvc.Studios.FirstOrDefault(pr => pr.Name == studio.Name) ?? _mvc.Studios.Add(new Studio(studio));
            return s;    
        }

        public void RemoveStudio(IMovie movie, IStudio studio) {
            Studio s = FindStudio(studio);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Studios.Remove(s);
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

        public void RemovePlot(IMovie movie, IPlot plot) {
            Plot p = FindPlot(plot);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Plots.Remove(p);
        }

        public IPlot AddPlot(IMovie movie, IPlot plot) {
            Plot p = FindPlot(plot);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Plots.Add(p);
            return p;
        }

        private Plot FindPlot(IPlot plot) {
            Plot p;
            if (plot.Id > 0) {
                p = _mvc.Plots.Find(plot.Id);
                if (p == null || (p.Full != plot.Full)) {
                    p = _mvc.Plots.Add(new Plot(plot));
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

        public IGenre AddGenre(IGenre genre) {
            return AddHasName(_mvc.Genres, genre);
        }

        public void RemoveGenre(IGenre genre) {
            RemoveHasName(_mvc.Genres, genre);
        }

        public IGenre AddGenre(IMovie movie, IGenre genre) {
            Genre g = FindGenre(genre);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Genres.Add(g);
            return g;
        }

        public void RemoveGenre(IMovie movie, IGenre genre) {
            Genre g = FindGenre(genre);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Genres.Remove(g);
        }

        private Genre FindGenre(IGenre genre) {
            Genre p;
            if (genre.Id > 0) {
                p = _mvc.Genres.Find(genre.Id);
                if (p == null || (p.Name != genre.Name)) {
                    p = _mvc.Genres.Add(new Genre(genre));
                }
                return p;
            }

            p = _mvc.Genres.FirstOrDefault(pr => genre.Name == pr.Name) ?? _mvc.Genres.Add(new Genre(genre));
            return p; 
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

                _certifications = _mvc.Certifications.Local;
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

        public IMovieSet AddSet(IMovieSet set) {
            return AddHasName(_mvc.Sets, set);
        }

        public void RemoveSet(IMovieSet set) {
            RemoveHasName(_mvc.Sets, set);
        }

        #endregion

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

        #region Specials

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

        public ISpecial AddSpecial(ISpecial special) {
            return AddHasName(_mvc.Specials, special);
        }

        public void RemoveSpecial(ISpecial special) {
            RemoveHasName(_mvc.Specials, special);
        }

        #endregion

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

        private Person GetPerson(IPerson person) {
            Person p;
            if (person.Id > 0) {
                p = _mvc.People.Find(person.Id);
                if (p == null || p.Name != person.Name) {
                    p = _mvc.People.Add(new Person(person));
                }
                return p;
            }

            p = _mvc.People.FirstOrDefault(pr => (person.ImdbID != null && pr.ImdbID == person.ImdbID) || pr.Name == person.Name);
            if (p == null) {
                p = _mvc.People.Add(new Person(person));
            }
            return p;
        }

        public IPerson AddPerson(IPerson person) {
            return null;
        }

        public IPerson AddDirector(IMovie movie, IPerson director) {
            Person p = GetPerson(director);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Directors.Add(p);
            return p;
        }

        public void RemoveDirector(IMovie movie, IPerson director) {
            Person p = GetPerson(director);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Directors.Remove(p);
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

        public IActor AddActor(IMovie movie, IActor actor) {
            Actor a = FindActor(actor);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Actors.Add(a);

            return a;
        }

        public void RemoveActor(IMovie movie, IActor actor) {
            Actor a = FindActor(actor);

            Movie mv = movie as Movie ?? FindMovie(movie);
            mv.Actors.Remove(a);
        }

        private Actor FindActor(IActor actor) {
            Actor p;
            if (actor.Id > 0) {
                p = _mvc.Actors.Find(actor.Id);
                if (p == null || p.Name != actor.Name) {
                    p = _mvc.Actors.Add(new Actor(actor));
                }
                return p;
            }

            p = _mvc.Actors.FirstOrDefault(pr => (actor.ImdbID != null && pr.ImdbID == actor.ImdbID) || (pr.Name == actor.Name && pr.Character == actor.Character))
                ?? _mvc.Actors.Add(new Actor(actor));
            return p;               
        }

        #endregion


        public bool HasUnsavedChanges() {
            return _mvc.HasUnsavedChanges();
        }

        public void SaveChanges() {
            _mvc.SaveChanges();
        }


        private TSet AddHasName<TEntity, TSet>(IDbSet<TSet> set, TEntity hasName) where TEntity : class, IHasName, IMovieEntity
                                                                                  where TSet : class, IHasName, IMovieEntity {
            if (hasName.Id > 0) {
                TSet find = set.Find(hasName.Id);
                if (find == null || find.Name != hasName.Name) {
                    find = set.Create();
                    find.Name = hasName.Name;

                    find = set.Add(find);
                }
                return find;
            }

            TSet hn = set.FirstOrDefault(n => n.Name == hasName.Name);
            if (hn == null) {
                hn = set.Create();
                hn.Name = hasName.Name;

                hn = set.Add(hn);
            }
            return hn;            
        }

        private void RemoveHasName<TEntity, TSet>(IDbSet<TSet> set, TEntity hasName) where TEntity : class, IHasName, IMovieEntity
                                                                                     where TSet : class, IHasName, IMovieEntity {
            if (hasName.Id > 0) {
                TSet find = set.Find(hasName.Id);
                if (find == null) {
                    find = set.FirstOrDefault(s => s.Name == hasName.Name);
                    if (find != null) {
                        set.Remove(find);
                        return;
                    }
                }
                set.Remove(find);
                return;
            }

            TSet hn = set.FirstOrDefault(n => n.Name == hasName.Name);
            if (hn != null) {
                set.Remove(hn);
            }      
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
