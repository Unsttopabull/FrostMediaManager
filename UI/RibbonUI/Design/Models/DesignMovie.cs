using System;
using System.Collections.Generic;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;

namespace RibbonUI.Design.Models {
    public class DesignMovie : IMovie {
        private List<ISubtitle> _subtitles;
        private List<ICountry> _countries;
        private List<IStudio> _studios;
        private List<IVideo> _videos;
        private List<IAudio> _audios;
        private List<IRating> _ratings;
        private List<IPlot> _plots;
        private List<IArt> _art;
        private List<ICertification> _certifications;
        private List<IPerson> _writers;
        private List<IPerson> _directors;
        private List<IActor> _actors;
        private List<ISpecial> _specials;
        private List<IGenre> _genres;
        private List<IAward> _awards;
        private List<IPromotionalVideo> _promotionalVideos;

        public DesignMovie() {
            _subtitles = new List<ISubtitle>();
            _countries = new List<ICountry>();
            _studios = new List<IStudio>();
            _videos = new List<IVideo>();
            _audios = new List<IAudio>();
            _ratings = new List<IRating>();
            _plots = new List<IPlot>();
            _art = new List<IArt>();
            _certifications = new List<ICertification>();
            _writers = new List<IPerson>();
            _directors = new List<IPerson>();
            _actors = new List<IActor>();
            _specials = new List<ISpecial>();
            _genres = new List<IGenre>();
            _awards = new List<IAward>();
            _promotionalVideos = new List<IPromotionalVideo>();
        }

        #region Properties

        public long Id { get; set; }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Title":
                    case "OriginalTitle":
                    case "SortTitle":
                    case "Type":
                    case "Goofs":
                    case "Trivia":
                    case "ReleaseYear":
                    case "ReleaseDate":
                    case "Edithion":
                    case "DvdRegion":
                    case "LastPlayed":
                    case "Premiered":
                    case "Aired":
                    case "Trailer":
                    case "Top250":
                    case "Runtime":
                    case "Watched":
                    case "PlayCount":
                    case "RatingAverage":
                    case "ImdbID":
                    case "TmdbID":
                    case "ReleaseGroup":
                    case "IsMultipart":
                    case "PartTypes":
                    case "DirectoryPath":
                    case "NumberOfAudioChannels":
                    case "AudioCodec":
                    case "VideoResolution":
                    case "VideoCodec":
                    case "Countries":
                    case "Studios":
                    case "Videos":
                    case "Audios":
                    case "Ratings":
                    case "Plots":
                    case "Art":
                    case "Certifications":
                    case "Writers":
                    case "Directors":
                    case "Actors":
                    case "Specials":
                    case "Genres":
                    case "Awards":
                    case "PromotionalVideos":
                    case "HasTrailer":
                    case "HasSubtitles":
                    case "HasArt":
                    case "HasNfo":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        public string Title { get; set; }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        public string OriginalTitle { get; set; }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        public string SortTitle { get; set; }

        /// <summary>Gets or sets the type of the movie.</summary>
        /// <value>The type of the movie.</value>
        /// <example>\eg{ DVD, BluRay, ...}</example>
        public MovieType Type { get; set; }

        /// <summary>Gets or sets the goofs.</summary>
        /// <value>The goofs.</value>
        public string Goofs { get; set; }

        /// <summary>Gets or sets the trivia.</summary>
        /// <value>The trivia.</value>
        public string Trivia { get; set; }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        public long? ReleaseYear { get; set; }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>Gets or sets the movie edithion.</summary>
        /// <value>The movie edithion.</value>
        /// <example>\eg{Extended, Directors cut, Retail ...}</example>
        public string Edithion { get; set; }

        /// <summary>Gets or sets the DVD region of this movie or source.</summary>
        /// <value>The DVD region of this movie or source.</value>
        public DVDRegion DvdRegion { get; set; }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        public DateTime? LastPlayed { get; set; }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        public DateTime? Premiered { get; set; }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        public DateTime? Aired { get; set; }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        public string Trailer { get; set; }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        public long? Top250 { get; set; }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        public long? Runtime { get; set; }

        /// <summary>Gets or sets a value indicating whether has beed played before.</summary>
        /// <value><c>true</c> if this movie has been played before; otherwise, <c>false</c>.</value>
        public bool Watched { get; set; }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        public long PlayCount { get; set; }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        public double? RatingAverage { get; set; }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        public string ImdbID { get; set; }

        /// <summary>Gets or sets The Movie Databse identifier of this movie.</summary>
        /// <value>The Movie Databse identifier of this movie.</value>
        public string TmdbID { get; set; }

        /// <summary>Gets or sets the release group.</summary>
        /// <value>The release group.</value>
        public string ReleaseGroup { get; set; }

        /// <summary>Gets or sets a value indicating whether this movie is comprised of multiple files.</summary>
        /// <value>Is <c>true</c> if the movie is comprised of multiple files; otherwise, <c>false</c>.</value>
        public bool IsMultipart { get; set; }

        /// <summary>Gets or sets the part types.</summary>
        /// <value>If the movie is Multipart it represents the type of the parts.</value>
        /// <example>\eg{DVD, CD, ...}</example>
        public string PartTypes { get; set; }

        /// <summary>Gets or sets the directory path to this movie.</summary>
        public string DirectoryPath { get; set; }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        public int? NumberOfAudioChannels { get; set; }

        /// <summary>Gets or sets the audio codec used most frequently in associated audios.</summary>
        /// <value>The audio codec used most frequently in associated audios</value>
        public string AudioCodec { get; set; }

        /// <summary>Gets or sets the video resolution used most frequently in associated audios.</summary>
        /// <value>The video resolution used most frequently in associated audios</value>
        public string VideoResolution { get; set; }

        /// <summary>Gets or sets the video codec used most frequently in associated audios.</summary>
        /// <value>The video codec used most frequently in associated audios</value>
        public string VideoCodec { get; set; }

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public IMovieSet Set { get; set; }

        #endregion

        #region 1 to M

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public IEnumerable<ISubtitle> Subtitles {
            get { return _subtitles; }
            set { _subtitles = new List<ISubtitle>(value); }
        }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public IEnumerable<ICountry> Countries {
            get { return _countries; }
            set { _countries = new List<ICountry>(value); }
        }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        public IEnumerable<IStudio> Studios {
            get { return _studios; }
            set { _studios = new List<IStudio>(value); }
        }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        public IEnumerable<IVideo> Videos {
            get { return _videos; }
            set { _videos = new List<IVideo>(value); }
        }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public IEnumerable<IAudio> Audios {
            get { return _audios; }
            set { _audios = new List<IAudio>(value); }
        }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public IEnumerable<IRating> Ratings {
            get { return _ratings; }
            set { _ratings = new List<IRating>(value); }
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public IEnumerable<IPlot> Plots {
            get { return _plots; }
            set { _plots = new List<IPlot>(value); }
        }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public IEnumerable<IArt> Art {
            get { return _art; }
            set { _art = new List<IArt>(value); }
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public IEnumerable<ICertification> Certifications {
            get { return _certifications; }
            set { _certifications = new List<ICertification>(value); }
        }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        public IEnumerable<IPerson> Writers {
            get { return _writers; }
            set { _writers = new List<IPerson>(value); }
        }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        public IEnumerable<IPerson> Directors {
            get { return _directors; }
            set { _directors = new List<IPerson>(value); }
        }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public IEnumerable<IActor> Actors {
            get { return _actors; }
            set { _actors = new List<IActor>(value); }
        }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public IEnumerable<ISpecial> Specials {
            get { return _specials; }
            set { _specials = new List<ISpecial>(value); }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public IEnumerable<IGenre> Genres {
            get { return _genres; }
            set { _genres = new List<IGenre>(value); }
        }

        public IEnumerable<IAward> Awards {
            get { return _awards; }
            set { _awards = new List<IAward>(value); }
        }

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get { return _promotionalVideos; }
            set { _promotionalVideos = new List<IPromotionalVideo>(value); }
        }

        #endregion

        #region Utility

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        public bool HasTrailer { get; set; }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        public bool HasSubtitles { get; set; }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value
        public bool HasArt { get; set; }

        public bool HasNfo { get; set; }

        #endregion

        #region Add/Remove

        public IActor AddActor(IActor actor) {
            _actors.Add(actor);
            return actor;
        }

        public bool RemoveActor(IActor actor) {
            return _actors.Remove(actor);
        }

        public IPerson AddDirector(IPerson director) {
            _directors.Add(director);
            return director;
        }

        public bool RemoveDirector(IPerson director) {
            return _directors.Remove(director);
        }

        public ISpecial AddSpecial(ISpecial special) {
            _specials.Add(special);
            return special;
        }

        public bool RemoveSpecial(ISpecial special) {
            return _specials.Remove(special);
        }

        public IGenre AddGenre(IGenre genre) {
            _genres.Add(genre);
            return genre;
        }

        public bool RemoveGenre(IGenre genre) {
            return _genres.Remove(genre);
        }

        public IPlot AddPlot(IPlot plot) {
            _plots.Add(plot);
            return plot;
        }

        public bool RemovePlot(IPlot plot) {
            return _plots.Remove(plot);
        }

        public IStudio AddStudio(IStudio studio) {
            _studios.Add(studio);
            return studio;
        }

        public bool RemoveStudio(IStudio studio) {
            return _studios.Remove(studio);
        }

        public ICountry AddCountry(ICountry country) {
            _countries.Add(country);
            return country;
        }

        public bool RemoveCountry(ICountry country) {
            return _countries.Remove(country);
        }

        public ISubtitle AddSubtitle(ISubtitle subtitle) {
            _subtitles.Add(subtitle);
            return subtitle;
        }

        public bool RemoveSubtitle(ISubtitle subtitle) {
            return _subtitles.Remove(subtitle);
        }

        public IVideo AddVideo(IVideo video) {
            _videos.Add(video);
            return video;
        }

        public IAudio AddAudio(IAudio audio) {
            _audios.Add(audio);
            return audio;
        }

        #endregion

        public void Update(MovieInfo movieInfo) {
            
        }

    }
}
