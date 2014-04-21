using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Comparers;

namespace Frost.Providers.Frost.DB {

    /// <summary>Represents an information about a movie in the library.</summary>
    public partial class Movie {
        /// <summary>Separator between multiple genres, certifications, person names ...</summary>
        private const string SEPARATOR = " / ";

        /// <summary>Initializes a new instance of the <see cref="Movie"/> class.</summary>
        public Movie() {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Audios = new HashSet<Audio>();
            Ratings = new HashSet<Rating>();
            Plots = new HashSet<Plot>();
            Art = new HashSet<Art>();
            Certifications = new HashSet<Certification>();
            Awards = new HashSet<Award>();
            Videos = new HashSet<Video>();
            Subtitles = new HashSet<Subtitle>();
            Countries = new HashSet<Country>();
            
            Genres = new HashSet<Genre>(new HasNameEqualityComparer());
            Studios = new HashSet<Studio>(new HasNameEqualityComparer());
            Specials = new HashSet<Special>(new HasNameEqualityComparer());

            Directors = new HashSet<Person>();
            Writers = new HashSet<Person>();
            Actors = new HashSet<Actor>();
            PromotionalVideos = new HashSet<PromotionalVideo>();

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        #region Properties/Columns

        /// <summary>Gets or sets the database movie Id.</summary>
        /// <value>The database movie Id</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        [Required]
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

        #endregion

        #region Foreign Keys

        /// <summary>Gets or sets the Set foreign key.</summary>
        /// <value>The Set foreign key.</value>
        public long? SetId { get; set; }

        #endregion

        #region Relation tables

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public virtual Set Set { get; set; }

        public virtual Plot MainPlot { get; set; }

        //public long? CoverId { get; set; }

        public virtual Art DefaultCover { get; set; }

        //public long? FanartId { get; set; }

        public virtual Art DefaultFanart { get; set; }

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public virtual HashSet<Subtitle> Subtitles { get; set; }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public virtual HashSet<Country> Countries { get; set; }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        public virtual HashSet<Studio> Studios { get; set; }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        public virtual HashSet<Video> Videos { get; set; }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public virtual HashSet<Audio> Audios { get; set; }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public virtual HashSet<Rating> Ratings { get; set; }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        [InverseProperty("Movie")]
        public virtual HashSet<Plot> Plots { get; set; }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        [InverseProperty("Movie")]
        public virtual HashSet<Art> Art { get; set; }
        
        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public virtual HashSet<Certification> Certifications { get; set; }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        public virtual HashSet<Person> Writers { get; set; }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        public virtual HashSet<Person> Directors { get; set; }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public virtual HashSet<Actor> Actors { get; set; }
        
        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public virtual HashSet<Special> Specials { get; set; }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public virtual HashSet<Genre> Genres { get; set; }
        
        public virtual HashSet<Award> Awards { get; set; }

        public virtual HashSet<PromotionalVideo> PromotionalVideos { get; set; }

        #endregion

        #region Utility Functions / Properties

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        public bool HasTrailer {
            get { return !string.IsNullOrEmpty(Trailer); }
        }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        public bool HasSubtitles {
            get { return Subtitles.Count != 0; }
        }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value
        public bool HasArt {
            get { return Art.Count != 0; }
        }

        public bool HasNfo {
            get {
                if (DirectoryPath == null) {
                    return false;
                }
                return Directory.EnumerateFiles(DirectoryPath, "*.nfo").Any();
            }
        }

        /// <summary>Gets the file size summed from all the movie files.</summary>
        /// <returns>The movie file size in bytes summed from all its files</returns>
        private long GetFileSizeSum() {
            long sumA = Audios.Where(f => f.File != null && f.File.Size.HasValue).Sum(f => f.File.Size.Value);
            long sumV = Videos.Where(f => f.File != null && f.File.Size.HasValue).Sum(f => f.File.Size.Value);
            long sumS = Subtitles.Where(f => f.File != null && f.File.Size.HasValue).Sum(f => f.File.Size.Value);

            return sumA + sumV + sumS;
        }

        /// <summary>Gets the file size in pretty printed format formatted.</summary>
        /// <returns>A string with pretty printed movie file size</returns>
        /// <example>\eg{ <c>1024</c> is <c>1 Kb</c>}</example>
        public string GetFileSizeFormatted() {
            return GetFileSizeSum().FormatFileSizeAsString();
        }

        #endregion

        public override string ToString() {
            return String.Format("{0} ({1})", Title, ReleaseYear);
        }
    }

}