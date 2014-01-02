using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using Frost.Common.Models.DB.Jukebox;
using Frost.Common.Models.DB.MovieVo.Arts;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.Common.Models.DB.MovieVo.People;
using Frost.Common.Models.DB.XBMC;
using Frost.Common.Models.XML.Jukebox;
using Frost.Common.Models.XML.XBMC;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>Represents an information about a movie in the library.</summary>
    public class Movie {

        /// <summary>Separator between multiple genres, certifications, person names ...</summary>
        private const string SEPARATOR = " / ";

        /// <summary>Initializes a new instance of the <see cref="Movie"/> class.</summary>
        public Movie() {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Audio = new HashSet<Audio>();
            Ratings = new HashSet<Rating>();
            Plot = new HashSet<Plot>();
            Art = new HashSet<Art>();
            Certifications = new HashSet<Certification>();
            Genres = new HashSet<Genre>();
            Videos = new HashSet<Video>();
            Subtitles = new HashSet<Subtitle>();
            Files = new HashSet<File>();
            Countries = new HashSet<Country>();
            Studios = new HashSet<Studio>();
            Specials = new HashSet<Special>();

            Directors = new HashSet<Person>();
            Writers = new HashSet<Person>();
            ActorsLink = new HashSet<MovieActor>();

            Actors = new HashSet<Actor>();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        #region Properties/Columns

        /// <summary>Gets or sets the database movie Id.</summary>
        /// <value>The database movie Id</value>
        [Key]
        public long Id { get; set; }

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

        /// <summary>Gets or sets the goofs.</summary>
        /// <value>The goofs.</value>
        public string Goofs { get; set; }

        /// <summary>Gets or sets the trivia.</summary>
        /// <value>The trivia.</value>
        public string Trivia { get; set; }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        public int? ReleaseYear { get; set; }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        public DateTime ReleaseDate { get; set; }

        /// <summary>Gets or sets the movie edithion.</summary>
        /// <value>The movie edithion.</value>
        /// <example>\eg{Extended, Directors cut, Retail ...}</example>
        public string Edithion { get; set; }

        /// <summary>Gets or sets the DVD region of this movie or source.</summary>
        /// <value>The DVD region of this movie or source.</value>
        public DVDRegion DvdRegion { get; set; }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        public DateTime LastPlayed { get; set; }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        public DateTime Premiered { get; set; }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        public DateTime Aired { get; set; }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        public string Trailer { get; set; }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        public int? Top250 { get; set; }

        /// <summary>Gets or sets the runtime of the movie</summary>
        /// <value>The runtime of the movie</value>
        public long? Runtime { get; set; }

        /// <summary>Gets or sets a value indicating whether has beed played before.</summary>
        /// <value><c>true</c> if this movie has been played before; otherwise, <c>false</c>.</value>
        public bool Watched { get; set; }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        public int PlayCount { get; set; }

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
        #endregion

        #region Foreign Keys

        /// <summary>Gets or sets the Set foreign key.</summary>
        /// <value>The Set foreign key.</value>
        public long? SetId { get; set; }

        /// <summary>Gets or sets the main plot foreign key.</summary>
        /// <value>The movie main plot foreign key.</value>
        [ForeignKey("MainPlot")]
        public long MainPlotID { get; set; }

        #endregion

        #region Relation tables

        /// <summary>Gets or sets the movie main plot.</summary>
        /// <value>The main plot of the movie.</value>
        public virtual Plot MainPlot { get; set; }

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        [ForeignKey("SetId")]
        public virtual Set Set { get; set; }

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

        /// <summary>Gets or sets the information about files containing this movie.</summary>
        /// <value>The information about files containing this movie.</value>
        public virtual HashSet<File> Files { get; set; }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public virtual HashSet<Audio> Audio { get; set; }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public virtual HashSet<Rating> Ratings { get; set; }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public virtual HashSet<Plot> Plot { get; set; }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public virtual HashSet<Art> Art { get; set; }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public virtual HashSet<Certification> Certifications { get; set; }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        [InverseProperty("MoviesAsWriter")]
        public virtual HashSet<Person> Writers { get; set; }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        [InverseProperty("MoviesAsDirector")]
        public virtual HashSet<Person> Directors { get; set; }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public virtual HashSet<MovieActor> ActorsLink { get; set; }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public virtual HashSet<Special> Specials { get; set; }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public virtual HashSet<Genre> Genres { get; set; }

        /// <summary>Gets or sets the actors that starred in the movie.</summary>
        /// <value>The actors that preformed in this movie.</value>
        [NotMapped]
        public HashSet<Actor> Actors {
            get { return new HashSet<Actor>(ActorsLink.Select(ma => (Actor) ma)); }
            set { ActorsLink.UnionWith(value.Select(a => new MovieActor(this, a))); }
        }

        #endregion

        #region Utility Functions

        /// <summary>Computes the average critic rating.</summary>
        /// <returns>The average critic rating.</returns>
        public double ComputeAverageRating() {
            return Ratings.Average(r => r.Value);
        }

        /// <summary>Gets the US MPAA movie rating.</summary>
        /// <returns>A string with the MPAA movie rating</returns>
        public string GetMPAARating() {
            Certification mpaa = Certifications.FirstOrDefault(c => c.Country.Name.OrdinalEquals("United States"));
            if (mpaa != null) {
                return mpaa.Rating;
            }
            return null;
        }

        /// <summary>Gets the file size summed from all the movie files.</summary>
        /// <returns>The movie file size in bytes summed from all its files</returns>
        public long GetFileSizeSum() {
            long? sum = Files.Where(f => f.Size != null).Sum(f => f.Size);
            return sum.HasValue
                ? sum.Value
                : 0;
        }

        /// <summary>Gets the file size in pretty printed format formatted.</summary>
        /// <returns>A string with pretty printed movie file size</returns>
        /// <example>\eg{ <c>1024</c> is <c>1 Kb</c>}</example>
        public string GetFileSizeFormatted() {
            return GetFileSizeSum().FormatFileSizeAsString();
        }

        /// <summary>Gets the genre names in a formatted string.</summary>
        /// <returns>The genre names in a single string separated with " / "</returns>
        /// <example>\eg{ <c>"Horor / SciFi"</c>}</example>
        public string GetGenreNames() {
            return string.Join(SEPARATOR, Genres.Select(g => g.Name));
        }

        /// <summary>Gets the director names in a formatted string.</summary>
        /// <returns>The director names in a single string separated with " / "</returns>
        public string GetDirectorNames() {
            string directorsJoin = string.Join(SEPARATOR, Directors.Select(d => d.Name));
            return string.IsNullOrEmpty(directorsJoin)
                ? null
                : directorsJoin;
        }

        /// <summary>Gets the cover image path name.</summary>
        /// <returns>The path to the fist cover image</returns>
        public string GetCoverPath() {
            Cover cover = Art.OfType<Cover>().FirstOrDefault();
            return (cover != null)
                ? cover.Path
                : null;
        }

        /// <summary>Gets the studio names in a formatted string.</summary>
        /// <returns>The studio names in a single string separated with " / "</returns>
        /// <example>\eg{ ''<c>MGM / Fox</c>''}</example>
        public string GetStudioNamesFormatted() {
            return String.Join(SEPARATOR, Studios.Select(stud => stud.Name));
        }

        /// <summary>Gets the names of the studios that produced this movie in an Array.</summary>
        /// <returns>An array of studios that produced the movie</returns>
        public IEnumerable<string> GetStudioNames() {
            return Studios.Select(s => s.Name);
        }

        /// <summary>Gets the movie actors as an <see cref="IEnumerable{T}"/> of <see cref="Common.Models.XML.Jukebox.XjbXmlActor">XjbXmlActor</see> instances.</summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of this movie actors as <see cref="Common.Models.XML.Jukebox.XjbXmlActor">XjbXmlActor</see> instances</returns>
        public IEnumerable<XjbXmlActor> GetXjbXmlActors() {
            return Actors.Select(a => (XjbXmlActor) a);
        }

        /// <summary>Gets the runtime sum of all the video parts in this movie in miliseconds.</summary>
        /// <returns>Full runtime sum of video parts in this movie in miliseconds.</returns>
        public long? GetVideoRuntimeSum() {
            long l = Videos.Where(v => v.Duration.HasValue).Sum(v => v.Duration.Value);

            return (l > 0)
                ? (long?) l
                : null;
        }

        #endregion

        public override string ToString() {
            return string.Format("{0} ({1})", Title, ReleaseYear);
        }

        #region Serialization

        /// <summary>Serializes an instance of <see cref="Movie"/> as xml in a system specified by a parameter <c><paramref name="system"/></c>.</summary>
        /// <param name="system">The system to serialize to.</param>
        /// <param name="xmlSaveLocation">Where to save the serialized xml.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the <c><paramref name="system"/></c> is out of range (has an unknown enmum value)</exception>
        public void Serialize(NFOSystem system, string xmlSaveLocation) {
            switch (system) {
                case NFOSystem.Xtreamer:
                    ((XjbXmlMovie) this).Serialize(xmlSaveLocation);
                    break;
                case NFOSystem.XBMC:
                    ((XbmcXmlMovie) this).Serialize(xmlSaveLocation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("system");
            }
        }

        /// <summary>Deserialize an instance of <see cref="Movie"/> from <c>xml</c> in a system specified by a parameter <paramref name="system"/>.</summary>
        /// <param name="system">The system from which to deserialize from.</param>
        /// <param name="xmlLocation">Filepath to the serialized xml.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the <c><paramref name="system"/></c> is out of range (has an unknown enmum value)</exception>
        /// <exception cref="System.IO.FileNotFoundException">Throws if the file specified with <c><paramref name="xmlLocation"/></c> can't be found</exception>
        public Movie Load(NFOSystem system, string xmlLocation) {
            switch (system) {
                case NFOSystem.Xtreamer:
                    return XjbXmlMovie.LoadAsMovie(xmlLocation);
                case NFOSystem.XBMC:
                    return XbmcXmlMovie.LoadAsMovie(xmlLocation);
                default:
                    throw new ArgumentOutOfRangeException("system");
            }
        }

        #endregion

        #region Conversion Functions

        /// <summary>Converts this instance to and instance of <see cref="Common.Models.XML.Jukebox.XjbXmlMovie">XjbXmlMovie</see>.</summary>
        /// <returns>An instance of <see cref="Common.Models.XML.Jukebox.XjbXmlMovie">XjbXmlMovie</see> converted from this instance.</returns>
        public XjbXmlMovie ToXjbXmlMovie() {
            return (XjbXmlMovie) this;
        }

        /// <summary>Converts this instance to and instance of <see cref="Common.Models.XML.XBMC.XbmcXmlMovie">XbmcXmlMovie</see>.</summary>
        /// <returns>An instance of <see cref="Common.Models.XML.XBMC.XbmcXmlMovie">XbmcXmlMovie</see> converted from this instance.</returns>
        public XbmcXmlMovie ToXbmcXmlMovie() {
            return (XbmcXmlMovie) this;
        }

        /// <summary>Converts this instance to and instance of <see cref="Common.Models.DB.XBMC.XbmcMovie">XbmcMovie</see>.</summary>
        /// <returns>An instance of <see cref="Common.Models.DB.XBMC.XbmcMovie">XbmcMovie</see> converted from this instance.</returns>
        public XbmcMovie ToXbmcMovie() {
            return (XbmcMovie) this;
        }

        /// <summary>Converts this instance to and instance of <see cref="Common.Models.DB.Jukebox">XjbMovie</see>.</summary>
        /// <returns>An instance of <see cref="Common.Models.DB.Jukebox">XjbMovie</see> converted from this instance.</returns>
        public XjbMovie ToXjbMovie() {
            return (XjbMovie) this;
        }

        #endregion

        #region Conversion Operators

        /// <summary>Converts an instance of <see cref="Movie"/> to and instance of <see cref="Common.Models.DB.Jukebox">XjbMovie</see>.</summary>
        /// <param name="movie">The <see cref="Movie"/> instance to convert.</param>
        /// <returns>An instance of <see cref="Common.Models.DB.Jukebox">XjbMovie</see> converted from <see cref="Movie"/></returns>
        public static explicit operator XjbMovie(Movie movie) {
            XjbMovie xm = new XjbMovie();

            return xm;
        }

        /// <summary>Converts an instance of <see cref="Movie"/> to and instance of <see cref="Common.Models.XML.Jukebox.XjbXmlMovie">XjbXmlMovie</see>.</summary>
        /// <param name="movie">The <see cref="Movie"/> instance to convert.</param>
        /// <returns>An instance of <see cref="Common.Models.XML.Jukebox.XjbXmlMovie">XjbXmlMovie</see> converted from <see cref="Movie"/></returns>
        public static explicit operator XjbXmlMovie(Movie movie) {
            return new XjbXmlMovie {
                Certifications = movie.Certifications.ToArray(),
                Director = movie.GetDirectorNames(),
                GenreString = movie.GetGenreNames(),
                ImdbId = movie.ImdbID,
                OriginalTitle = movie.OriginalTitle,
                Outline = movie.MainPlot.Summary,
                Plot = movie.MainPlot.Full,
                AverageRating = (float) (movie.RatingAverage ?? 0),
                //TODO: CHECK FOR CORECT FORMAT
                ReleaseDate = movie.ReleaseDate.ToString(CultureInfo.InvariantCulture),
                Runtime = movie.Runtime.HasValue
                    ? (movie.Runtime / 60)
                    : 0,
                SortTitle = movie.Title,
                Studio = movie.GetStudioNamesFormatted(),
                Tagline = movie.MainPlot.Summary,
                Title = movie.Title,
                Year = movie.ReleaseYear ?? 0,
                Actors = movie.GetXjbXmlActors().ToArray(),
                MPAA = movie.GetMPAARating(),
                Credits = String.Join(SEPARATOR, movie.Writers.Select(p => p.Name)),
                Fileinfo = ""
            };
        }

        /// <summary>Converts an instance of <see cref="Movie"/> to and instance of <see cref="Common.Models.XML.XBMC.XbmcXmlMovie">XbmcXmlMovie</see>.</summary>
        /// <param name="movie">The <see cref="Movie"/> instance to convert.</param>
        /// <returns>An instance of <see cref="Common.Models.XML.XBMC.XbmcXmlMovie">XbmcXmlMovie</see> converted from <see cref="Movie"/></returns>
        public static explicit operator XbmcXmlMovie(Movie movie) {
            throw new NotImplementedException();
        }

        /// <summary>Converts an instance of <see cref="Movie"/> to and instance of <see cref="Common.Models.DB.XBMC.XbmcMovie">XbmcMovie</see>.</summary>
        /// <param name="movie">The <see cref="Movie"/> instance to convert.</param>
        /// <returns>An instance of <see cref="Common.Models.DB.XBMC.XbmcMovie">XbmcMovie</see> converted from <see cref="Movie"/></returns>
        public static explicit operator XbmcMovie(Movie movie) {
            throw new NotImplementedException();
        }

        #endregion
    }

}
