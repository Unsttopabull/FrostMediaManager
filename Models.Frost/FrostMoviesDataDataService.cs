using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows;
using Frost.Common;
using Frost.Common.Models;
using Frost.Models.Frost.DB;

namespace Frost.Models.Frost {
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

        public IEnumerable<IMovie> Movies {
            get {
                if (_movies != null) {
                    return _movies;
                }

                _mvc.Movies
                    .Include("Studios")
                    .Include("Art")
                    .Include("Genres")
                    .Include("Awards")
                    .Include("Actors")
                    .Include("Plots")
                    .Include("Directors")
                    .Include("Countries")
                    .Include("Audios")
                    .Include("Videos")
                    .Load();

                _movies = _mvc.Movies.Local;
                return _movies;
            }
        }

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
