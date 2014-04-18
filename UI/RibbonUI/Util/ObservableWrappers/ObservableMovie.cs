using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.DetectFeatures;

namespace RibbonUI.Util.ObservableWrappers {

    public class ObservableMovie : MovieItemBase<IMovie> {
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

        public IArt DefaultFanart {
            get {
                if (ObservedEntity.DefaultFanart != null) {
                    return ObservedEntity.DefaultFanart;
                }
                IArt art = Art.FirstOrDefault(a => a.Type == ArtType.Fanart && !string.IsNullOrEmpty(a.PreviewOrPath));
                return art;
            }
            set { ObservedEntity.DefaultFanart = value; }
        }

        public IArt DefaultCover {
            get {
                if (ObservedEntity.DefaultCover != null) {
                    return ObservedEntity.DefaultCover;
                }

                IArt art = Art.FirstOrDefault(a => (a.Type == ArtType.Cover || a.Type == ArtType.Poster) && !String.IsNullOrEmpty(a.PreviewOrPath));
                return art;
            }
            set { ObservedEntity.DefaultCover = value; }
        }

        public IPlot MainPlot {
            get {
                if (ObservedEntity.MainPlot != null) {
                    return ObservedEntity.MainPlot;
                }

                return Plots.Any()
                    ? Plots.FirstOrDefault()
                    : null;
            }
            set { ObservedEntity.MainPlot = value; }
        }
        #endregion

        #region Relation properties

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public IEnumerable<ISubtitle> Subtitles {
            get { return _observedEntity.Subtitles; }
        }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public IEnumerable<ICountry> Countries {
            get { return _observedEntity.Countries; }
        }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        public IEnumerable<IStudio> Studios {
            get { return _observedEntity.Studios; }
        }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        public IEnumerable<IVideo> Videos {
            get { return _observedEntity.Videos; }
        }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public IEnumerable<IAudio> Audios {
            get { return _observedEntity.Audios; }
        }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public IEnumerable<IRating> Ratings {
            get { return _observedEntity.Ratings; }
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public IEnumerable<IPlot> Plots {
            get { return _observedEntity.Plots; }
        }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public IEnumerable<IArt> Art {
            get { return _observedEntity.Art; }
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public IEnumerable<ICertification> Certifications {
            get { return _observedEntity.Certifications; }
        }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        public IEnumerable<IPerson> Writers {
            get { return _observedEntity.Writers; }
        }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        public IEnumerable<IPerson> Directors {
            get { return _observedEntity.Directors; }
        }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public IEnumerable<IActor> Actors {
            get { return _observedEntity.Actors; }
        }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public IEnumerable<ISpecial> Specials {
            get { return _observedEntity.Specials; }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public IEnumerable<IGenre> Genres {
            get { return _observedEntity.Genres; }
        }

        public IEnumerable<IAward> Awards {
            get { return _observedEntity.Awards; }
        }

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get { return _observedEntity.PromotionalVideos; }
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
                        IStudio studio = Studios.FirstOrDefault();
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

        public IPlot FirstPlot {
            get {
                if (ObservedEntity.MainPlot != null) {
                    return ObservedEntity.MainPlot;
                }

                return Plots.Any()
                           ? Plots.FirstOrDefault()
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

        #endregion

        #region Add/Remove

        public IActor AddActor(IActor actor) {
            return Add(_observedEntity.AddActor, actor);
        }

        public bool RemoveActor(IActor actor) {
            return Remove(_observedEntity.RemoveActor, actor);
        }

        public IPerson AddDirector(IPerson director) {
            return Add(_observedEntity.AddDirector, director);
        }

        public bool RemoveDirector(IPerson director) {
            return Remove(_observedEntity.RemoveDirector, director);
        }

        public ISpecial AddSpecial(ISpecial special) {
            return Add(_observedEntity.AddSpecial, special);
        }

        public bool RemoveSpecial(ISpecial special) {
            return Remove(_observedEntity.RemoveSpecial, special);
        }

        public IGenre AddGenre(IGenre genre) {
            return Add(_observedEntity.AddGenre, genre);
        }

        public bool RemoveGenre(IGenre genre) {
            return Remove(_observedEntity.RemoveGenre, genre);
        }

        public IPlot AddPlot(IPlot plot) {
            return Add(_observedEntity.AddPlot, plot);
        }

        public bool RemovePlot(IPlot plot) {
            return Remove(_observedEntity.RemovePlot, plot);
        }

        public IStudio AddStudio(IStudio studio) {
            return Add(_observedEntity.AddStudio, studio);
        }

        public bool RemoveStudio(IStudio studio) {
            return Remove(_observedEntity.RemoveStudio, studio);
        }

        public ICountry AddCountry(ICountry country) {
            return Add(_observedEntity.AddCountry, country);
        }

        public bool RemoveCountry(ICountry country) {
            return Remove(_observedEntity.RemoveCountry, country);
        }

        public ISubtitle AddSubtitle(ISubtitle subtitle) {
            return Add(_observedEntity.AddSubtitle, subtitle);
        }

        public bool RemoveSubtitle(ISubtitle subtitle) {
            return Remove(_observedEntity.RemoveSubtitle, subtitle);
        }

        private bool Remove<T>(Func<T, bool> removeItem, T item) where T : IMovieEntity {
            try {
                if (removeItem(item)) {
                    return true;
                }
                UIHelper.ProviderCouldNotRemove();
            }
            catch (Exception e) {
                UIHelper.HandleProviderException(e);
            }
            return false;
        }

        private T Add<T>(Func<T, T> addItem, T item) where T : class, IMovieEntity {
            T addedItem = null;
            try {
                addedItem = addItem(item);
                if (addedItem == null) {
                    UIHelper.ProviderCouldNotAdd();
                }
            }
            catch (Exception e) {
                UIHelper.HandleProviderException(e);
            }
            return addedItem;
        }

        #endregion

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