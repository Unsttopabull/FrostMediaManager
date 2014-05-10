using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.DetectFeatures;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers;
using Frost.InfoParsers.Models.Subtitles;
using log4net;
using Swordfish.NET.Collections;

namespace RibbonUI.Util.ObservableWrappers {

    public class ObservableMovie : MovieItemBase<IMovie>, IMovieInfo, IEquatable<IMovie> {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ObservableMovie));
        private ConcurrentObservableCollection<MovieSubtitle> _subtitles;
        private ConcurrentObservableCollection<MovieCountry> _countries;
        private ConcurrentObservableCollection<MovieStudio> _studios;
        private ConcurrentObservableCollection<MovieVideo> _videos;
        private ConcurrentObservableCollection<MovieAudio> _audios;
        private ConcurrentObservableCollection<IPromotionalVideo> _promotionalVideos;
        private ConcurrentObservableCollection<IRating> _ratings;
        private ConcurrentObservableCollection<MoviePlot> _plots;
        private ConcurrentObservableCollection<MovieArt> _art;
        private ConcurrentObservableCollection<MovieCertification> _certifications;
        private ConcurrentObservableCollection<IPerson> _writers;
        private ConcurrentObservableCollection<MoviePerson> _directors;
        private ConcurrentObservableCollection<MovieActor> _actors;
        private ConcurrentObservableCollection<ISpecial> _specials;
        private ConcurrentObservableCollection<IGenre> _genres;
        private ConcurrentObservableCollection<IAward> _awards;

        public ObservableMovie(IMovie movie) : base(movie) {
        }

        #region Properties

        public long Id {
            get { return _observedEntity.Id; }
        }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        public string Title {
            get { return _observedEntity.Title; }
            set {
                _observedEntity.Title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        public string OriginalTitle {
            get { return _observedEntity.OriginalTitle; }
            set {
                _observedEntity.OriginalTitle = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        public string SortTitle {
            get { return _observedEntity.SortTitle; }
            set {
                _observedEntity.SortTitle = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the type of the movie.</summary>
        /// <value>The type of the movie.</value>
        /// <example>\eg{ DVD, BluRay, ...}</example>
        public MovieType Type {
            get { return _observedEntity.Type; }
            set {
                _observedEntity.Type = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the goofs.</summary>
        /// <value>The goofs.</value>
        public string Goofs {
            get { return _observedEntity.Goofs; }
            set {
                _observedEntity.Goofs = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the trivia.</summary>
        /// <value>The trivia.</value>
        public string Trivia {
            get { return _observedEntity.Trivia; }
            set {
                _observedEntity.Trivia = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        public long? ReleaseYear {
            get { return _observedEntity.ReleaseYear; }
            set {
                _observedEntity.ReleaseYear = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        public DateTime? ReleaseDate {
            get { return _observedEntity.ReleaseDate; }
            set {
                _observedEntity.ReleaseDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the movie edithion.</summary>
        /// <value>The movie edithion.</value>
        /// <example>\eg{Extended, Directors cut, Retail ...}</example>
        public string Edithion {
            get { return _observedEntity.Edithion; }
            set {
                _observedEntity.Edithion = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the DVD region of this movie or source.</summary>
        /// <value>The DVD region of this movie or source.</value>
        public DVDRegion DvdRegion {
            get { return _observedEntity.DvdRegion; }
            set {
                _observedEntity.DvdRegion = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        public DateTime? LastPlayed {
            get { return _observedEntity.LastPlayed; }
            set {
                _observedEntity.LastPlayed = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        public DateTime? Premiered {
            get { return _observedEntity.Premiered; }
            set {
                _observedEntity.Premiered = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        public DateTime? Aired {
            get { return _observedEntity.Aired; }
            set {
                _observedEntity.Aired = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        public string Trailer {
            get { return _observedEntity.Trailer; }
            set {
                _observedEntity.Trailer = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        public long? Top250 {
            get { return _observedEntity.Top250; }
            set {
                _observedEntity.Top250 = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        public long? Runtime {
            get { return _observedEntity.Runtime; }
            set {
                _observedEntity.Runtime = value;

                OnPropertyChanged("RuntimeTimeSpan");
                OnPropertyChanged("DurationFormatted");
                OnPropertyChanged();
            }
        }

        public TimeSpan RuntimeTimeSpan {
            get {
                return Runtime.HasValue
                           ? TimeSpan.FromMilliseconds((double) Runtime)
                           : new TimeSpan();
            }
            set { Runtime = Convert.ToInt64(value.TotalMilliseconds); }
        }

        /// <summary>Gets or sets a value indicating whether has beed played before.</summary>
        /// <value><c>true</c> if this movie has been played before; otherwise, <c>false</c>.</value>
        public bool Watched {
            get { return _observedEntity.Watched; }
            set {
                _observedEntity.Watched = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        public long PlayCount {
            get { return _observedEntity.PlayCount; }
            set {
                _observedEntity.PlayCount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        public double? RatingAverage {
            get { return _observedEntity.RatingAverage; }
            set {
                _observedEntity.RatingAverage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        public string ImdbID {
            get { return _observedEntity.ImdbID; }
            set {
                _observedEntity.ImdbID = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets The Movie Databse identifier of this movie.</summary>
        /// <value>The Movie Databse identifier of this movie.</value>
        public string TmdbID {
            get { return _observedEntity.TmdbID; }
            set {
                _observedEntity.TmdbID = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the release group.</summary>
        /// <value>The release group.</value>
        public string ReleaseGroup {
            get { return _observedEntity.ReleaseGroup; }
            set {
                _observedEntity.ReleaseGroup = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets a value indicating whether this movie is comprised of multiple files.</summary>
        /// <value>Is <c>true</c> if the movie is comprised of multiple files; otherwise, <c>false</c>.</value>
        public bool IsMultipart {
            get { return _observedEntity.IsMultipart; }
            set {
                _observedEntity.IsMultipart = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the part types.</summary>
        /// <value>If the movie is Multipart it represents the type of the parts.</value>
        /// <example>\eg{DVD, CD, ...}</example>
        public string PartTypes {
            get { return _observedEntity.PartTypes; }
            set {
                _observedEntity.PartTypes = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the directory path to this movie.</summary>
        public string DirectoryPath {
            get { return _observedEntity.DirectoryPath; }
            set {
                _observedEntity.DirectoryPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the full path of the first file to begin playing the movie.</summary>
        public string FirstFileName {
            get { return _observedEntity.FirstFileName; }
            set {
                _observedEntity.FirstFileName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        public int? NumberOfAudioChannels {
            get { return _observedEntity.NumberOfAudioChannels; }
            set {
                _observedEntity.NumberOfAudioChannels = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio codec used most frequently in associated audios.</summary>
        /// <value>The audio codec used most frequently in associated audios</value>
        public string AudioCodec {
            get { return _observedEntity.AudioCodec; }
            set {
                _observedEntity.AudioCodec = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video resolution used most frequently in associated audios.</summary>
        /// <value>The video resolution used most frequently in associated audios</value>
        public string VideoResolution {
            get { return _observedEntity.VideoResolution; }
            set {
                _observedEntity.VideoResolution = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video codec used most frequently in associated audios.</summary>
        /// <value>The video codec used most frequently in associated audios</value>
        public string VideoCodec {
            get { return _observedEntity.VideoCodec; }
            set {
                _observedEntity.VideoCodec = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public IMovieSet Set {
            get { return _observedEntity.Set; }
            set {
                _observedEntity.Set = value;
                OnPropertyChanged();
            }
        }

        public MovieArt DefaultFanart {
            get {
                if (ObservedEntity.DefaultFanart != null) {
                    return new MovieArt(ObservedEntity.DefaultFanart);
                }

                return Art.FirstOrDefault(a => a.Type == ArtType.Fanart && !string.IsNullOrEmpty(a.PreviewOrPath));
            }
            set {
                ObservedEntity.DefaultFanart = value != null
                    ? value.ObservedEntity
                    : null;

                OnPropertyChanged();
            }
        }

        public MovieArt DefaultCover {
            get {
                if (ObservedEntity.DefaultCover != null) {
                    return new MovieArt(ObservedEntity.DefaultCover);
                }

                return Art.FirstOrDefault(a => (a.Type == ArtType.Cover || a.Type == ArtType.Poster) && !String.IsNullOrEmpty(a.PreviewOrPath));
            }
            set {
                ObservedEntity.DefaultCover = value != null
                    ? value.ObservedEntity
                    : null;

                OnPropertyChanged();
            }
        }

        public MoviePlot MainPlot {
            get {
                if (ObservedEntity.MainPlot != null) {
                    return new MoviePlot(ObservedEntity.MainPlot);
                }

                return Plots.Any()
                           ? Plots.FirstOrDefault()
                           : null;
            }
            set {
                ObservedEntity.MainPlot = value != null 
                    ? value.ObservedEntity
                    : null;

                OnPropertyChanged();
                OnPropertyChanged("FirstPlot");
            }
        }

        #endregion

        #region Relation properties

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public ConcurrentObservableCollection<MovieSubtitle> Subtitles {
            get {
                if (_subtitles == null) {
                    _subtitles = _observedEntity.Subtitles == null
                        ? new ConcurrentObservableCollection<MovieSubtitle>()
                        : new ConcurrentObservableCollection<MovieSubtitle>(_observedEntity.Subtitles.Select(s => new MovieSubtitle(s)));
                }
                return _subtitles;
            }
        }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public ConcurrentObservableCollection<MovieCountry> Countries {
            get {
                if (_countries == null) {
                    _countries = _observedEntity.Countries == null 
                        ? new ConcurrentObservableCollection<MovieCountry>() 
                        : new ConcurrentObservableCollection<MovieCountry>(_observedEntity.Countries.Select(s => new MovieCountry(s)));
                }
                return _countries;
            }
        }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        public ConcurrentObservableCollection<MovieStudio> Studios {
            get {
                if (_studios == null) {
                    _studios = _observedEntity.Studios == null 
                        ? new ConcurrentObservableCollection<MovieStudio>() 
                        : new ConcurrentObservableCollection<MovieStudio>(_observedEntity.Studios.Select(s => new MovieStudio(s)));
                }
                return _studios;
            }
        }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        public ConcurrentObservableCollection<MovieVideo> Videos {
            get {
                if (_videos == null) {
                    _videos = _observedEntity.Videos == null 
                        ? new ConcurrentObservableCollection<MovieVideo>() 
                        : new ConcurrentObservableCollection<MovieVideo>(_observedEntity.Videos.Select(v => new MovieVideo(v)));
                }
                return _videos;
            }
        }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public ConcurrentObservableCollection<MovieAudio> Audios {
            get {
                if (_audios == null) {
                    _audios = _observedEntity.Audios == null
                        ? new ConcurrentObservableCollection<MovieAudio>()
                        : new ConcurrentObservableCollection<MovieAudio>(_observedEntity.Audios.Select(a => new MovieAudio(a)));
                }
                return _audios;
            }
        }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public ConcurrentObservableCollection<IRating> Ratings {
            get {
                if (_ratings == null) {
                    _ratings = _observedEntity.Ratings == null 
                        ? new ConcurrentObservableCollection<IRating>() 
                        : new ConcurrentObservableCollection<IRating>(_observedEntity.Ratings);
                }

                return _ratings;
            }
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public ConcurrentObservableCollection<MoviePlot> Plots {
            get {
                if (_plots == null) {
                    _plots = _observedEntity.Plots == null 
                        ? new ConcurrentObservableCollection<MoviePlot>() 
                        : new ConcurrentObservableCollection<MoviePlot>(_observedEntity.Plots.Select(p => new MoviePlot(p)));
                }

                return _plots;
            }
        }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public ConcurrentObservableCollection<MovieArt> Art {
            get {
                if (_art == null) {
                    _art = _observedEntity.Art == null 
                        ? new ConcurrentObservableCollection<MovieArt>() 
                        : new ConcurrentObservableCollection<MovieArt>(_observedEntity.Art.Select(a => new MovieArt(a)));
                }

                return _art;
            }
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public ConcurrentObservableCollection<MovieCertification> Certifications {
            get {
                if (_certifications == null) {
                    _certifications = _observedEntity.Certifications == null
                        ? new ConcurrentObservableCollection<MovieCertification>()
                        : new ConcurrentObservableCollection<MovieCertification>(_observedEntity.Certifications.Select(c => new MovieCertification(c)));
                }

                return _certifications;
            }
        }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        public ConcurrentObservableCollection<IPerson> Writers {
            get {
                if (_writers == null) {
                    _writers = _observedEntity.Writers == null 
                        ? new ConcurrentObservableCollection<IPerson>() 
                        : new ConcurrentObservableCollection<IPerson>(_observedEntity.Writers);
                }

                return _writers;
            }
        }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        public ConcurrentObservableCollection<MoviePerson> Directors {
            get {
                if (_directors == null) {
                    _directors = _observedEntity.Directors == null 
                        ? new ConcurrentObservableCollection<MoviePerson>() 
                        : new ConcurrentObservableCollection<MoviePerson>(_observedEntity.Directors.Select(p => new MoviePerson(p)));
                }

                return _directors;
            }
        }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public ConcurrentObservableCollection<MovieActor> Actors {
            get {
                if (_actors == null) {
                    _actors = _observedEntity.Actors == null 
                        ? new ConcurrentObservableCollection<MovieActor>() 
                        : new ConcurrentObservableCollection<MovieActor>(_observedEntity.Actors.Select(a => new MovieActor(a)));
                }

                return _actors;
            }
        }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public ConcurrentObservableCollection<ISpecial> Specials {
            get {
                if (_specials == null) {
                    _specials = _observedEntity.Specials == null 
                        ? new ConcurrentObservableCollection<ISpecial>() 
                        : new ConcurrentObservableCollection<ISpecial>(_observedEntity.Specials);
                }

                return _specials;
            }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public ConcurrentObservableCollection<IGenre> Genres {
            get {
                if (_genres == null) {
                    _genres = _observedEntity.Genres == null 
                        ? new ConcurrentObservableCollection<IGenre>() 
                        : new ConcurrentObservableCollection<IGenre>(_observedEntity.Genres);
                }

                return _genres;
            }
        }

        public ConcurrentObservableCollection<IAward> Awards {
            get {
                if (_awards == null) {
                    _awards = _observedEntity.Awards == null
                        ? new ConcurrentObservableCollection<IAward>()
                        : new ConcurrentObservableCollection<IAward>(_observedEntity.Awards);
                }

                return _awards;
            }
        }

        public ConcurrentObservableCollection<IPromotionalVideo> PromotionalVideos {
            get {
                if (_promotionalVideos == null) {
                    _promotionalVideos = _observedEntity.PromotionalVideos == null 
                        ? new ConcurrentObservableCollection<IPromotionalVideo>()
                        : new ConcurrentObservableCollection<IPromotionalVideo>(_observedEntity.PromotionalVideos);
                }

                return _promotionalVideos;
            }
        }

        #endregion

        #region Has X

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        public bool HasTrailer {
            get { return _observedEntity.HasTrailer; }
        }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        public bool HasSubtitles {
            get { return _observedEntity.HasSubtitles; }
        }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value>
        public bool HasArt {
            get { return _observedEntity.HasArt; }
        }

        public bool HasNfo {
            get { return _observedEntity.HasNfo; }
        }

        #endregion

        #region Utlity

        public IEnumerable<string> MovieHashes {
            get {       
                if (_observedEntity["Videos"]) {
                    return _observedEntity.Videos
                                          .Where(v => v != null && !string.IsNullOrEmpty(v.MovieHash))
                                          .Select(m => m.MovieHash)
                                          .ToList();
                }
                return null;
            }
        }

        public IEnumerable<IMovieHash> MovieHashesInfo {
            get {       
                if (_observedEntity["Videos"]) {
                    return _observedEntity.Videos
                                          .Where(v => v != null && !string.IsNullOrEmpty(v.MovieHash))
                                          .Select(m => new MovieHash(m.MovieHash, m.File.Size.HasValue ? m.File.Size.Value : 0))
                                          .ToList();
                }
                return null;
            }
        }

        #region Awards

        public int NumberOfOscarsWon {
            get {
                return Awards != null
                    ? Awards.Count(a => a.Organization == "Oscar" && !a.IsNomination)
                    : 0;
            }
        }

        public int NumberOfGoldenGlobesWon {
            get {
                return Awards != null
                    ? Awards.Count(a => a.Organization == "Golden Globe" && !a.IsNomination)
                    : 0;
            }
        }

        public int NumberOfGoldenGlobeNominations {
            get {
                return Awards != null
                    ? Awards.Count(a => a.Organization == "Golden Globe" && a.IsNomination)
                    : 0;
            }
        }

        public int NumberOfCannesAwards {
            get {
                return Awards != null
                    ? Awards.Count(a => a.Organization == "Cannes" && !a.IsNomination)
                    : 0;
            }
        }

        public int NumberOfCannesNominations {
            get {
                return Awards != null
                    ? Awards.Count(a => a.Organization == "Cannes" && a.IsNomination)
                    : 0;
            }
        }

        public int NumberOfOscarNominations {
            get {
                return Awards != null
                    ? Awards.Count(a => a.Organization == "Oscar" && a.IsNomination)
                    : 0;
            }
        }

        #endregion

        /// <summary>Gets the US MPAA movie rating.</summary>
        /// <value>A string with the MPAA movie rating</value>
        public string MPAARating {
            get {
                if (Certifications == null) {
                    return null;
                }

                ICertification mpaa = null;
                try {
                    mpaa = Certifications.FirstOrDefault(c => c.Country.ISO3166.Alpha3.Equals("usa", StringComparison.OrdinalIgnoreCase));
                }
                catch (Exception e) {
                    if (Log.IsErrorEnabled) {
                        Log.Error(string.Format("Error while trying to retreive MPAA rating for movie \"{0}\".", Title), e);
                    }
                }

                return mpaa != null
                           ? mpaa.Rating
                           : null;
            }
        }

        public string DurationFormatted {
            get {
                long? sum = Runtime ?? GetVideoRuntimeSum();

                return sum.HasValue && sum.Value > 0
                           ? TimeSpan.FromMilliseconds(Convert.ToDouble(sum)).ToString("h'h 'm'm'")
                           : null;
            }
        }

        public string FirstStudioName {
            get {
                if (Studios == null) {
                    return null;
                }

                try {
                    if (Studios.Any()) {
                        MovieStudio studio = Studios.FirstOrDefault();
                        if (studio != null) {
                            return studio.Name;
                        }
                    }
                }
                catch {
                    return null;
                }
                return null;
            }
        }

        public string FirstStudioLogo {
            get {
                if (string.IsNullOrEmpty(FirstStudioName)) {
                    return null;
                }

                var studio = Path.Combine(Directory.GetCurrentDirectory(), "Images/StudiosE/" + FirstStudioName + ".png");
                return File.Exists(studio)
                           ? studio
                           : null;
            }
        }

        public MoviePlot FirstPlot {
            get {
                if (ObservedEntity.MainPlot != null) {
                    return new MoviePlot(ObservedEntity.MainPlot);
                }

                return Plots.Any()
                    ? Plots.FirstOrDefault()
                    : null;
            }
        }

        public string GenreNames {
            get {
                return Genres != null
                    ? string.Join(", ", Genres.Where(g => g != null).Select(g => g.Name)) 
                    : null;
            }
        }

        public string DirectorNames {
            get {
                return Directors != null
                    ? string.Join(", ", Directors.Where(d => d != null).Select(g => g.Name)) 
                    : null;
            }
        }

        private long? GetVideoRuntimeSum() {
            if (Videos == null) {
                return null;
            }

            long l = Videos.Where(v => v.Duration.HasValue).Sum(v => v.Duration.Value);

            if (!Runtime.HasValue && l > 0) {
                Runtime = l;
            }

            return (l > 0)
                       ? (long?) l
                       : null;
        }

        public void NotifyPropertyChanged(string property) {
            OnPropertyChanged(property);
        }

        #endregion

        #region Add/Remove

        public void AddActor(IActor actor, bool silent = false) {
            IActor a = Add(_observedEntity.AddActor, actor, silent);
            if (a == null) {
                return;
            }

            MovieActor ac = Actors.FirstOrDefault(act => act.Equals(a));
            if (ac == null) {
                if (Update(actor, a)) {
                    return;
                }

                Actors.Add(new MovieActor(a));
            }
            else {
                if (!silent) {
                    MessageBox.Show(Gettext.T("This person has already been added to this movie as {0}.", "an actor"));
                }
            }
        }

        private bool Update(IActor newActor, IActor returnedActor) {
            MovieActor ac;
            if (!string.IsNullOrEmpty(newActor.Character)) {
                ac = Actors.FirstOrDefault(act => act.Equals(returnedActor as IPerson) && string.IsNullOrEmpty(act.Character));
                if (ac != null) {
                    if (!string.IsNullOrEmpty(newActor.Thumb) && string.IsNullOrEmpty(ac.Thumb)) {
                        ac.Thumb = newActor.Thumb;
                    }

                    ac.Character = newActor.Character;
                    return true;
                }
            }

            if (!string.IsNullOrEmpty(newActor.Thumb)) {
                ac = Actors.FirstOrDefault(act => act.Equals(returnedActor as IPerson) && string.IsNullOrEmpty(act.Thumb));
                if (ac != null) {
                    ac.Thumb = newActor.Thumb;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveActor(MovieActor actor, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveActor, actor.ObservedEntity as IActor, silent);
            if (success) {
                Actors.Remove(actor);
            }
            return success;
        }

        public IPerson AddWriter(IPerson writer, bool silent = false) {
            IPerson p = Add(_observedEntity.AddWriter, writer, silent);

            if (p == null) {
                return null;
            }

            if (!Writers.Any(d => d.Equals(p))) {
                Writers.Add(p);
            }
            else if(!silent){
                MessageBox.Show(Gettext.T("This person has already been added to this movie as {0}.", "a writer"));
            }
            return p;
        }

        public void RemoveWriter(MoviePerson writer, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveWriter, writer.ObservedEntity, silent);
            if (success) {
                Writers.Remove(writer.ObservedEntity);
            } 
        }

        public IPerson AddDirector(IPerson director, bool silent = false) {
            IPerson p = Add(_observedEntity.AddDirector, director, silent);

            if (p == null) {
                return null;
            }

            if (!Directors.Any(d => d.Equals(p))) {
                Directors.Add(new MoviePerson(p));
                OnPropertyChanged("DirectorNames");
            }
            else if(!silent){
                MessageBox.Show(Gettext.T("This person has already been added to this movie as {0}.", "a director"));
            }
            return p;
        }

        public bool RemoveDirector(MoviePerson director, bool silent = false) {
            bool removed = Remove(_observedEntity.RemoveDirector, director.ObservedEntity, silent);
            if (removed) {
                Directors.Remove(director);
                OnPropertyChanged("DirectorNames");
            }
            return removed;
        }

        public ISpecial AddSpecial(ISpecial special, bool silent = false) {
            return Add(_observedEntity.AddSpecial, special, silent);
        }

        public bool RemoveSpecial(ISpecial special, bool silent = false) {
            return Remove(_observedEntity.RemoveSpecial, special, silent);
        }

        public void AddGenre(IGenre genre, bool silent = false) {
            IGenre g = Add(_observedEntity.AddGenre, genre, silent);
            if (g == null) {
                return;
            }

            if (!Genres.Contains(g)) {
                Genres.Add(g);
                OnPropertyChanged("GenreNames");
            }
            else if(!silent){
                MessageBox.Show(Gettext.T("This {0} has already been added to this movie.", "genre"));
            }
        }

        public void RemoveGenre(IGenre genre, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveGenre, genre, silent);
            if (success) {
                Genres.Remove(genre);
                OnPropertyChanged("GenreNames");
            }
        }

        public void AddPlot(IPlot plot, bool silent = false) {
            IPlot p = Add(_observedEntity.AddPlot, plot, silent);

            if (p != null) {
                Plots.Add(new MoviePlot(p));
                OnPropertyChanged("FirstPlot");
            }
        }

        public bool RemovePlot(MoviePlot plot, bool silent = false) {
            bool success = Remove(_observedEntity.RemovePlot, plot.ObservedEntity, silent);

            if (success) {
                Plots.Remove(plot);
                OnPropertyChanged("FirstPlot");
            }
           
            return success;
        }

        public void AddStudio(IStudio studio, bool silent = false) {
            IStudio stud = Add(_observedEntity.AddStudio, studio, silent);
            if (stud == null) {
                return;
            }

            if (!Studios.Any(s => s.Equals(stud))) {
                Studios.Add(new MovieStudio(stud));
                OnPropertyChanged("FirstStudioName");
                OnPropertyChanged("FirstStudioLogo");
            }
            else if(!silent){
                MessageBox.Show(Gettext.T("This {0} has already been added to this movie.", "genre"));
            }
        }

        public void RemoveStudio(MovieStudio studio, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveStudio, studio.ObservedEntity, silent);
            if (success) {
                Studios.Remove(studio);
                OnPropertyChanged("FirstStudioName");
                OnPropertyChanged("FirstStudioLogo");
            }
        }

        public void AddAward(IAward award, bool silent = false) {
            IAward a = Add(_observedEntity.AddAward, award, silent);
            if (a == null) {
                return;
            }

            if (!Awards.Any(aw => string.Equals(aw.AwardType, award.AwardType) && string.Equals(aw.Organization, award.Organization) && award.IsNomination == aw.IsNomination)) {
                Awards.Add(a);
            }
            else if(!silent){
                MessageBox.Show(Gettext.T("This {0} has already been added to this movie.", "award"));
            }
        }

        public void RemoveAward(IAward award, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveAward, award, silent);
            if (success) {
                Awards.Remove(award);
            }            
        }

        public void AddCountry(ICountry country, bool silent = false) {
            ICountry c = Add(_observedEntity.AddCountry, country, silent);
            if (c == null) {
                return;
            }

            if (!Countries.Any(cntry => cntry.Equals(c))) {
                Countries.Add(new MovieCountry(c));
            }
            else if(!silent){
                MessageBox.Show(Gettext.T("This {0} has already been added to this movie.", "country"));
            }
        }

        public void RemoveCountry(MovieCountry country, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveCountry, country.ObservedEntity, silent);
            if (success) {
                Countries.Remove(country);
            }
        }

        public void AddSubtitle(ISubtitle subtitle, bool silent = false) {
            ISubtitle sub = Add(_observedEntity.AddSubtitle, subtitle, silent);
            if (sub != null) {
                Subtitles.Add(new MovieSubtitle(sub));
            }
        }

        public void RemoveSubtitle(MovieSubtitle subtitle, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveSubtitle, subtitle.ObservedEntity, silent);
            if (success) {
                Subtitles.Remove(subtitle);
            }
        }

        public void AddAudio(IAudio audio, bool silent = false) {
            IAudio a = Add(_observedEntity.AddAudio, audio, silent);
            if (a != null) {
                Audios.Add(new MovieAudio(a));
            }            
        }

        public void RemoveAudio(MovieAudio audio, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveAudio, audio.ObservedEntity, silent);
            if (success) {
                Audios.Remove(audio);
            }            
        }

        public void AddVideo(IVideo video, bool silent = false) {
            IVideo v = Add(_observedEntity.AddVideo, video, silent);
            if (v != null) {
                Videos.Add(new MovieVideo(v));
            }               
        }

        public void RemoveVideo(MovieVideo video, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveVideo, video.ObservedEntity, silent);
            if (success) {
                Videos.Remove(video);
            }                
        }

        public void AddPromotionalVideo(IPromotionalVideo promotionalVideo, bool silent = false) {
            IPromotionalVideo v = Add(_observedEntity.AddPromotionalVideo, promotionalVideo, silent);
            if (v == null) {
                if (!silent) {
                    MessageBox.Show(Gettext.T("This {0} has already been added to this movie.", "promotional video"));
                }
                return;
            }

            if (!PromotionalVideos.Any(pv => pv.Type == promotionalVideo.Type && string.Equals(pv.Title, promotionalVideo.Title, StringComparison.CurrentCultureIgnoreCase))) {
                PromotionalVideos.Add(v);
            }
            else if(!silent){
                MessageBox.Show(Gettext.T("This {0} has already been added to this movie.", "promotional video"));
            }
        }

        public void RemovePromotionalVideo(IPromotionalVideo promotionalVideo, bool silent = false) {
            bool success = Remove(_observedEntity.RemovePromotionalVideo, promotionalVideo, silent);
            if (success) {
                PromotionalVideos.Remove(promotionalVideo);
            }                 
        }

        public void AddArt(IArt art, bool silent = false) {
            IArt a = Add(_observedEntity.AddArt, art, silent);
            if (a == null) {
                if (!silent) {
                    MessageBox.Show(Gettext.T("This {0} has already been added to this movie.", "art"));
                }
                return;
            }

            if (!Art.Any(pv => pv.Type == art.Type && string.Equals(pv.Path, art.Path, StringComparison.CurrentCultureIgnoreCase))) {
                MovieArt movieArt = new MovieArt(a);
                Art.Add(movieArt);

                OnPropertyChanged("HasArt");

                if (movieArt.Type == ArtType.Fanart) {
                    OnPropertyChanged("DefaultFanart");
                }
                else if (movieArt.Type == ArtType.Cover || movieArt.Type == ArtType.Poster) {
                    OnPropertyChanged("DefaultCover");
                }
            }
            else if(!silent){
                MessageBox.Show(Gettext.T("This {0} has already been added to this movie.", "promotional video"));
            }            
        }

        public void RemoveArt(MovieArt art, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveArt, art.ObservedEntity, silent);
            if (success) {
                Art.Remove(art);
                OnPropertyChanged("HasArt");

                if (art.Type == ArtType.Fanart) {
                    OnPropertyChanged("DefaultFanart");
                }
                else if (art.Type == ArtType.Cover || art.Type == ArtType.Poster) {
                    OnPropertyChanged("DefaultCover");
                }
            }               
        }


        public void AddCertification(ICertification cert, bool silent) {
            ICertification c = Add(_observedEntity.AddCertification, cert, silent);
            if (c == null) {
                return;
            }

            if (!Certifications.Any(cer => cer.Equals(c))) {
                Certifications.Add(new MovieCertification(c));
                OnPropertyChanged("HasArt");
            }
            else if(!silent){
                MessageBox.Show(Gettext.T("This {0} has already been added to this movie.", "country"));
            }
        }

        public void RemoveCertification(MovieCertification cert, bool silent = false) {
            bool success = Remove(_observedEntity.RemoveCertification, cert.ObservedEntity, silent);
            if (success) {
                Certifications.Remove(cert);
            }
        }

        private bool Remove<T>(Func<T, bool> removeItem, T item, bool silent) where T : IMovieEntity {
            try {
                if (removeItem(item)) {
                    return true;
                }

                if (!silent) {
                    UIHelper.ProviderCouldNotRemove();
                }
                else {
                    throw new Exception(Gettext.T("Provider could not remove the item.\nProbable causes:\n\t* Item does not exists in the store\n\t* An error has occured."));
                }
            }
            catch (Exception e) {
                if (!silent) {
                    UIHelper.HandleProviderException(Log, e);
                }
                else {
                    throw;
                }
            }
            return false;
        }

        private T Add<T>(Func<T, T> addItem, T item, bool silent) where T : class, IMovieEntity {
            T addedItem = null;
            try {
                addedItem = addItem(item);
                if (addedItem == null) {
                    if (!silent) {
                        UIHelper.ProviderCouldNotAdd();
                    }
                    else {
                        throw new Exception(Gettext.T("Error: Provider could not add the item.\nPlease contact provider creator."));
                    }
                }
            }
            catch (Exception e) {
                if (!silent) {
                    UIHelper.HandleProviderException(Log, e);
                }
                else {
                    throw;
                }
            }
            return addedItem;
        }

        #endregion

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IMovie other) {
            return _observedEntity.Equals(other);
        }

        #region Images

        public string AudioCodecImage {
            get {
                if (AudioCodec == null) {
                    return null;
                }

                string mapping;
                FileFeatures.AudioCodecIdMappings.TryGetValue(AudioCodec, out mapping);
                return GetImageSourceFromPath("Images/FlagsE/acodec_" + (mapping ?? AudioCodec) + ".png");
            }
        }

        public string AudioChannelsImage {
            get {
                if (!NumberOfAudioChannels.HasValue) {
                    return null;
                }

                return GetImageSourceFromPath("Images/FlagsE/achan_" + NumberOfAudioChannels + ".png");
            }
        }

        public string VideoCodecImage {
            get {
                if (VideoCodec == null) {
                    return null;
                }

                string mapping;
                FileFeatures.VideoCodecIdMappings.TryGetValue(VideoCodec, out mapping);
                return GetImageSourceFromPath("Images/FlagsE/vcodec_" + (mapping ?? VideoCodec) + ".png");
            }
        }

        public string VideoResolutionImage {
            get {
                if (VideoResolution == null) {
                    return null;
                }

                return GetImageSourceFromPath("Images/FlagsE/vres_" + VideoResolution + ".png");
            }
        }

        #endregion
    }

}