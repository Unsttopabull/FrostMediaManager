using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Frost.Common;
using Frost.Common.Models;
using Frost.DetectFeatures;
using RibbonUI.Annotations;

namespace RibbonUI.Util.ObservableWrappers {

    public class ObservableMovie : MovieItemBase, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IMovie _movie;

        public ObservableMovie(IMovie movie) {
            _movie = movie;
        }

        #region Properties

        public long Id {
            get { return _movie.Id; }
        }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        public string Title {
            get { return _movie.Title; }
            set {
                _movie.Title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        public string OriginalTitle {
            get { return _movie.OriginalTitle; }
            set {
                _movie.OriginalTitle = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        public string SortTitle {
            get { return _movie.SortTitle; }
            set {
                _movie.SortTitle = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the type of the movie.</summary>
        /// <value>The type of the movie.</value>
        /// <example>\eg{ DVD, BluRay, ...}</example>
        public MovieType Type {
            get { return _movie.Type; }
            set {
                _movie.Type = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the goofs.</summary>
        /// <value>The goofs.</value>
        public string Goofs {
            get { return _movie.Goofs; }
            set {
                _movie.Goofs = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the trivia.</summary>
        /// <value>The trivia.</value>
        public string Trivia {
            get { return _movie.Trivia; }
            set {
                _movie.Trivia = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        public long? ReleaseYear {
            get { return _movie.ReleaseYear; }
            set {
                _movie.ReleaseYear = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        public DateTime? ReleaseDate {
            get { return _movie.ReleaseDate; }
            set {
                _movie.ReleaseDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the movie edithion.</summary>
        /// <value>The movie edithion.</value>
        /// <example>\eg{Extended, Directors cut, Retail ...}</example>
        public string Edithion {
            get { return _movie.Edithion; }
            set {
                _movie.Edithion = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the DVD region of this movie or source.</summary>
        /// <value>The DVD region of this movie or source.</value>
        public DVDRegion DvdRegion {
            get { return _movie.DvdRegion; }
            set {
                _movie.DvdRegion = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        public DateTime? LastPlayed {
            get { return _movie.LastPlayed; }
            set {
                _movie.LastPlayed = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        public DateTime? Premiered {
            get { return _movie.Premiered; }
            set {
                _movie.Premiered = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        public DateTime? Aired {
            get { return _movie.Aired; }
            set {
                _movie.Aired = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        public string Trailer {
            get { return _movie.Trailer; }
            set {
                _movie.Trailer = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        public long? Top250 {
            get { return _movie.Top250; }
            set {
                _movie.Top250 = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        public long? Runtime {
            get { return _movie.Runtime; }
            set {
                _movie.Runtime = value;

                OnPropertyChanged("RuntimeTimeSpan");
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
            get { return _movie.Watched; }
            set {
                _movie.Watched = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        public long PlayCount {
            get { return _movie.PlayCount; }
            set {
                _movie.PlayCount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        public double? RatingAverage {
            get { return _movie.RatingAverage; }
            set {
                _movie.RatingAverage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        public string ImdbID {
            get { return _movie.ImdbID; }
            set {
                _movie.ImdbID = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets The Movie Databse identifier of this movie.</summary>
        /// <value>The Movie Databse identifier of this movie.</value>
        public string TmdbID {
            get { return _movie.TmdbID; }
            set {
                _movie.TmdbID = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the release group.</summary>
        /// <value>The release group.</value>
        public string ReleaseGroup {
            get { return _movie.ReleaseGroup; }
            set {
                _movie.ReleaseGroup = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets a value indicating whether this movie is comprised of multiple files.</summary>
        /// <value>Is <c>true</c> if the movie is comprised of multiple files; otherwise, <c>false</c>.</value>
        public bool IsMultipart {
            get { return _movie.IsMultipart; }
            set {
                _movie.IsMultipart = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the part types.</summary>
        /// <value>If the movie is Multipart it represents the type of the parts.</value>
        /// <example>\eg{DVD, CD, ...}</example>
        public string PartTypes {
            get { return _movie.PartTypes; }
            set {
                _movie.PartTypes = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the directory path to this movie.</summary>
        public string DirectoryPath {
            get { return _movie.DirectoryPath; }
            set {
                _movie.DirectoryPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        public int? NumberOfAudioChannels {
            get { return _movie.NumberOfAudioChannels; }
            set {
                _movie.NumberOfAudioChannels = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio codec used most frequently in associated audios.</summary>
        /// <value>The audio codec used most frequently in associated audios</value>
        public string AudioCodec {
            get { return _movie.AudioCodec; }
            set {
                _movie.AudioCodec = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video resolution used most frequently in associated audios.</summary>
        /// <value>The video resolution used most frequently in associated audios</value>
        public string VideoResolution {
            get { return _movie.VideoResolution; }
            set {
                _movie.VideoResolution = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video codec used most frequently in associated audios.</summary>
        /// <value>The video codec used most frequently in associated audios</value>
        public string VideoCodec {
            get { return _movie.VideoCodec; }
            set {
                _movie.VideoCodec = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public IMovieSet Set {
            get { return _movie.Set; }
            set {
                _movie.Set = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Relation properties

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public IEnumerable<ISubtitle> Subtitles {
            get { return _movie.Subtitles; }
        }


        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public IEnumerable<ICountry> Countries {
            get { return _movie.Countries; }
        }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        public IEnumerable<IStudio> Studios {
            get { return _movie.Studios; }
        }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        public IEnumerable<IVideo> Videos {
            get { return _movie.Videos; }
        }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public IEnumerable<IAudio> Audios {
            get { return _movie.Audios; }
        }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public IEnumerable<IRating> Ratings {
            get { return _movie.Ratings; }
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public IEnumerable<IPlot> Plots {
            get { return _movie.Plots; }
        }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public IEnumerable<IArt> Art {
            get { return _movie.Art; }
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public IEnumerable<ICertification> Certifications {
            get { return _movie.Certifications; }
        }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        public IEnumerable<IPerson> Writers {
            get { return _movie.Writers; }
        }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        public IEnumerable<IPerson> Directors {
            get { return _movie.Directors; }
        }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public IEnumerable<IActor> Actors {
            get { return _movie.Actors; }
        }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public IEnumerable<ISpecial> Specials {
            get { return _movie.Specials; }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public IEnumerable<IGenre> Genres {
            get { return _movie.Genres; }
        }

        public IEnumerable<IAward> Awards {
            get { return _movie.Awards; }
        }

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get { return _movie.PromotionalVideos; }
        }

        #endregion

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        public bool HasTrailer {
            get { return _movie.HasTrailer; }
        }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        public bool HasSubtitles {
            get { return _movie.HasSubtitles; }
        }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value>
        public bool HasArt {
            get { return _movie.HasArt; }
        }

        public bool HasNfo {
            get { return _movie.HasNfo; }
        }

        public IMovie ObservedMovie {get { return _movie; }}

        #region Images

        public ImageSource AudioCodecImage {
            get {
                if (AudioCodec == null) {
                    return null;
                }

                string mapping;
                FileFeatures.AudioCodecIdMappings.TryGetValue(AudioCodec, out mapping);
                return GetImageSourceFromPath("Images/FlagsE/acodec_" + (mapping ?? AudioCodec) + ".png");
            }
        }

        public ImageSource AudioChannelsImage {
            get {
                if (!NumberOfAudioChannels.HasValue) {
                    return null;
                }

                return GetImageSourceFromPath("Images/FlagsE/achan_" + NumberOfAudioChannels + ".png");
            }
        }

        public ImageSource VideoCodecImage {
            get {
                if (VideoCodec == null) {
                    return null;
                }

                string mapping;
                FileFeatures.VideoCodecIdMappings.TryGetValue(VideoCodec, out mapping);
                return GetImageSourceFromPath("Images/FlagsE/vcodec_" + (mapping ?? VideoCodec) + ".png");
            }
        }

        public ImageSource VideoResolutionImage {
            get {
                if (VideoResolution == null) {
                    return null;
                }

                return GetImageSourceFromPath("Images/FlagsE/vres_" + VideoResolution + ".png");
            }
        }

        #endregion 

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
