using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Arts;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.Common.Models.DB.MovieVo.People;
using Frost.Common.Models.DB.XBMC;
using File = Frost.Common.Models.DB.MovieVo.Files.File;

namespace Frost.Common.Models.XML.XBMC {
    /// <summary>Represents an information about a movie in XBMC library ready to be serialized.</summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot("movie", Namespace = "", IsNullable = false)]
    public class XbmcXmlMovie {
        /// <summary>General separator between Genres, Names, Countries, Certifications ...</summary>
        private const string SEPARATOR = " / ";

        /// <summary>The XBMC YouTube plugin prefix for a movie trailer</summary>
        private const string YT_TRAILER_PREFIX = "plugin://plugin.video.youtube/?action=play_video&amp;videoid=";

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlMovie"/> class.</summary>
        public XbmcXmlMovie() {
            FileInfo = new XbmcXmlFileInfo();
        }

        #region Properties/Elements

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        [XmlElement("title", Form = XmlSchemaForm.Unqualified)]
        public string Title { get; set; }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        [XmlElement("originaltitle", Form = XmlSchemaForm.Unqualified)]
        public string OriginalTitle { get; set; }

        /// <summary>Gets or sets the title used for sorting.</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        [XmlElement("sorttitle", Form = XmlSchemaForm.Unqualified)]
        public string SortTitle { get; set; }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        [XmlElement("rating", Form = XmlSchemaForm.Unqualified)]
        public float Rating { get; set; }

        /// <summary>Gets or sets the EP bookmark</summary>
        /// <value>The EP bookmark.</value>
        [XmlElement("epbookmark", Form = XmlSchemaForm.Unqualified)]
        public float EPBookmark { get; set; }

        /// <summary>Gets or sets the year this movie was released in string format (YYYY).</summary>
        /// <value>The year this movie was released in string format (YYYY).</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("year", Form = XmlSchemaForm.Unqualified, DataType = "gYear")]
        public string YearString { get; set; }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in or zero if this information is not available.</value>
        [XmlIgnore]
        public int Year {
            get {
                int yr;
                if (!string.IsNullOrEmpty(YearString)) {
                    int.TryParse(YearString, out yr);

                    return yr;
                }
                yr = 0;
                return yr;
            }
            set { YearString = value.ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>Gets or sets the date and time the movie was released in the cinemas in (dd.MM.yyyy) format</summary>
        /// <value>The date and time the movie was released in the cinemas in (dd.MM.yyyy) format.</value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("releasedate", Form = XmlSchemaForm.Unqualified)]
        public string ReleaseDateString { get; set; }

        /// <summary>Gets or sets the date and time the movie was released in the cinemas.</summary>
        /// <value>The date and time the movie was released in the cinemas.</value>
        [XmlIgnore]
        public DateTime ReleaseDate {
            get {
                DateTime releaseDate;
                DateTime.TryParseExact(ReleaseDateString, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out releaseDate);

                //if parsing failed returns default(DateTime)
                return releaseDate;
            }
            set {
                //Convert DateTime to string in the specified format
                ReleaseDateString = value.ToString("dd.MM.yyyy");
            }
        }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        [XmlElement("top250", Form = XmlSchemaForm.Unqualified)]
        public int Top250 { get; set; }

        /// <summary>Gets or sets the number of votes the average rating was computed from</summary>
        /// <value>The number of ratings</value>
        [XmlElement("votes", Form = XmlSchemaForm.Unqualified)]
        public string Votes { get; set; }

        /// <summary>Gets or sets the certifications for other countries</summary>
        /// <value>Other country ratings</value>
        /// <remarks>If more than 1 they are split by " / " with country and rating split by ":" without space</remarks>
        /// <example>eg{''<c>us:R / au:M</c>''}</example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("certification", Form = XmlSchemaForm.Unqualified)]
        public string CertificationsString { get; set; }

        /// <summary>Gets or sets the certifications for other countries</summary>
        /// <value>Other country ratings</value>
        [XmlIgnore]
        public XbmcXmlCertification[] Certifications {
            get { return XbmcXmlCertification.ParseCertificationsString(CertificationsString); }
            set { CertificationsString = string.Join<XbmcXmlCertification>(SEPARATOR, value); }
        }

        /// <summary>Gets or sets the story summary.</summary>
        /// <value>A short story summary, the plot outline</value>
        [XmlElement("outline", Form = XmlSchemaForm.Unqualified)]
        public string Outline { get; set; }

        /// <summary>Gets or sets the full plot.</summary>
        /// <value>The full plot.</value>
        [XmlElement("plot", Form = XmlSchemaForm.Unqualified)]
        public string Plot { get; set; }

        /// <summary>Gets or sets the tagline (short one-liner).</summary>
        /// <value>The tagline (short promotional slogan / one-liner / clarification).</value>
        [XmlElement("tagline", Form = XmlSchemaForm.Unqualified)]
        public string Tagline { get; set; }

        /// <summary>Gets or sets the runtime of the movie pretty printed.</summary>
        /// <value>The runtime of the movie in pretty printed string.</value>
        /// <example>eg{''<c>130 min</c>''}</example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("runtime", Form = XmlSchemaForm.Unqualified)]
        public string RuntimeString { get; set; }

        /// <summary>Gets or sets the movie runtime in seconds.</summary>
        /// <value>The movie runtime in seconds.</value>
        [XmlIgnore]
        public long? RuntimeInSeconds {
            get {
                //runtime is saved as a string with seconds integer divided by 60 with appended text " min"
                string runtimeInMinutes = RuntimeString.Replace(" min", "");

                long runtimeVal;
                if (long.TryParse(runtimeInMinutes, out runtimeVal)) {
                    return runtimeVal > 0
                               ? (long?) (runtimeVal*60)
                               : null;
                }
                return null;
            }
            set {
                //if not null we integer divide seconds by 60 and append text " min"
                RuntimeString = (value.HasValue)
                                    ? (value/60) + " min"
                                    : null;
            }
        }

        /// <summary>Gets or sets a value indicating whether has beed played before.</summary>
        /// <value><c>true</c> if this movie has been played before; otherwise, <c>false</c>.</value>
        [XmlElement("watched", Form = XmlSchemaForm.Unqualified)]
        public bool Watched { get; set; }

        /// <summary>Gets or sets the movie Posters and Covers.</summary>
        /// <value>The movie Posters and Covers.</value>
        [XmlElement("thumb", Form = XmlSchemaForm.Unqualified)]
        public XbmcXmlThumb[] Thumbs { get; set; }

        /// <summary>Gets or sets the movie fanart images.</summary>
        /// <value>The fanart images.</value>
        [XmlElement("fanart", Form = XmlSchemaForm.Unqualified)]
        public XbmcXmlFanart Fanart { get; set; }

        /// <summary>Gets or sets the US movie rating and reason for it</summary>
        /// <value>The US Movie rating</value>
        [XmlElement("mpaa", Form = XmlSchemaForm.Unqualified)]
        public string MPAA { get; set; }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        [XmlElement("playcount", Form = XmlSchemaForm.Unqualified)]
        public int PlayCount { get; set; }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        [XmlElement("lastplayed", Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime LastPlayed { get; set; }

        /// <summary>Gets or sets the online movie database ids.</summary>
        /// <value>The online movie databse Ids.</value>
        [XmlElement("id", Form = XmlSchemaForm.Unqualified)]
        public List<XbmcXmlMovieDbId> Ids { get; set; }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        [XmlIgnore]
        public string ImdbId {
            get {
                if (Ids != null) {
                    XbmcXmlMovieDbId imdbID = GetOnlineDbID("imdb");
                    if (imdbID != null) {
                        return imdbID.Indentifier;
                    }
                }
                return null;
            }
            set {
                XbmcXmlMovieDbId imdb = GetOnlineDbID("imdb");
                if (imdb != null) {
                    imdb.Indentifier = value;
                }
                else {
                    Ids.Add(new XbmcXmlMovieDbId("imdb", value));
                }
            }
        }

        /// <summary>Gets or sets The Movie Databse identifier of this movie.</summary>
        /// <value>The Movie Databse identifier of this movie.</value>
        [XmlIgnore]
        public string TmdbId {
            get {
                if (Ids != null) {
                    XbmcXmlMovieDbId tmdbId = GetOnlineDbID("tmdb");
                    if (tmdbId != null) {
                        return tmdbId.Indentifier;
                    }
                }
                return null;
            }
            set {
                XbmcXmlMovieDbId tmdb = GetOnlineDbID("tmdb");
                if (tmdb != null) {
                    tmdb.Indentifier = value;
                }
                else {
                    Ids.Add(new XbmcXmlMovieDbId("tmdb", value));
                }
            }
        }

        /// <summary>Gets or sets the filename and path of the files that contain this movie.</summary>
        /// <remarks>
        /// If Movie consists of more that 1 file the string starts with <c>"stack://"</c>
        /// folwed by a full path to the files in the order to be played, separated by <c>" , "</c>
        /// </remarks>
        /// <value>The filename and path of the files that contain this movie.</value>
        /// <example>\egb{
        /// 	<list type="bullet">
        /// 		<item><description>''<c>stack://smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/Wall_E_cd1.avi , smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/Wall_E_cd2.avi</c>''</description></item>
        /// 		<item><description>''<c>smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/Wall_E.avi</c>''</description></item>
        /// 		<item><description>''<c>E:/Movies/Wall_E.avi</c>''</description></item>
        /// 	</list>}
        /// </example>
        [XmlElement("filenameandpath", Form = XmlSchemaForm.Unqualified)]
        public string FilenameAndPath { get; set; }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        [XmlElement("genre", Form = XmlSchemaForm.Unqualified)]
        public List<string> Genres { get; set; }

        /// <summary>Gets or sets the countries the movie was shot/produced in.</summary>
        /// <value>The countries the movie was shot/produced in.</value>
        [XmlElement("country", Form = XmlSchemaForm.Unqualified)]
        public List<string> Countries { get; set; }

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        [XmlElement("set", Form = XmlSchemaForm.Unqualified)]
        public string Set { get; set; }

        /// <summary>Gets or sets the name of the writer(s).</summary>
        /// <value>The names of script writer(s)</value>
        [XmlElement("credits", Form = XmlSchemaForm.Unqualified)]
        public List<string> Credits { get; set; }

        /// <summary>Gets or sets the name of the writer(s).</summary>
        /// <remarks>If more than 1 they are separated by " / "</remarks>
        /// <value>The names of script writer(s)</value>
        [XmlIgnore]
        public string CreditsFormatted {
            get { return string.Join(SEPARATOR, Credits); }
            set { Credits = value.SplitWithoutEmptyEntries(SEPARATOR).ToList(); }
        }

        /// <summary>Gets or sets the array containing the director names.</summary>
        /// <value>Full names of the directors</value>
        [XmlElement("director", Form = XmlSchemaForm.Unqualified)]
        public List<string> Directors { get; set; }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        [XmlElement("premiered", Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime Premiered { get; set; }

        /// <remarks/>
        [XmlElement("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }

        /// <remarks/>
        [XmlElement("code", Form = XmlSchemaForm.Unqualified)]
        public string Code { get; set; }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        [XmlElement("aired", Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime Aired { get; set; }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        [XmlElement("studio", Form = XmlSchemaForm.Unqualified)]
        public List<string> Studios { get; set; }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>URL to the movie trailer.</value>
        [XmlElement("trailer", Form = XmlSchemaForm.Unqualified, DataType = "anyURI")]
        public string Trailer { get; set; }

        /// <summary>Gets or sets the movie file information (Video/Audio/Subtitles).</summary>
        /// <value>The movie file information (Video/Audio/Subtitles)</value>
        [XmlElement("fileinfo", Form = XmlSchemaForm.Unqualified)]
        public XbmcXmlFileInfo FileInfo { get; set; }

        /// <summary>Gets or sets the actors that starred in the movie.</summary>
        /// <value>The actors that preformed in this movie.</value>
        [XmlElement("actor", Form = XmlSchemaForm.Unqualified)]
        public List<XbmcXmlActor> Actors { get; set; }

        /// <summary>Gets or sets the resuming information.</summary>
        /// <value>The resuming information.</value>
        [XmlElement("resume", Form = XmlSchemaForm.Unqualified)]
        public XbmcXmlResumeInfo ResumingInfo { get; set; }

        /// <summary>Gets or sets the date and time the file was added.</summary>
        /// <value>The date and time the file was added.</value>
        [XmlElement("dateadded", Form = XmlSchemaForm.Unqualified)]
        public string DateAdded { get; set; }

        #endregion

        #region Utility Functions

        /// <summary>Converts writer names into <see cref="Person"/> instances in a <see cref="HashSet{Person}"/>.</summary>
        /// <returns>A <see cref="HashSet{Person}"/> containing <see cref="Person"/> instances with names of the credited writers.</returns>
        public HashSet<Person> GetWriters() {
            return new HashSet<Person>(from c in Credits
                                       where !string.IsNullOrEmpty(c)
                                       select new Person(c));
        }

        /// <summary>Converts director names into <see cref="Person"/> instances in a <see cref="HashSet{Person}"/>.</summary>
        /// <returns>A <see cref="HashSet{Person}"/> containing <see cref="Person"/> instances with names of the credited directors.</returns>
        public HashSet<Person> GetDirectors() {
            return new HashSet<Person>(from d in Directors
                                       where !string.IsNullOrEmpty(d)
                                       select new Person(d));
        }

        /// <summary>Gets the files containing the movie as a <see cref="HashSet{T}"/> with <see cref="Common.Models.DB.MovieVo.Files">File</see> elements.</summary>
        /// <returns>A <see cref="HashSet{T}"/> with <see cref="Common.Models.DB.MovieVo.Files">File</see> elements.</returns>
        public HashSet<File> GetFiles() {
            HashSet<File> files = new HashSet<File>();
            if (string.IsNullOrEmpty(FilenameAndPath)) {
                return files;
            }

            string fn = FilenameAndPath;

            //if file is stacked split into individual filenames
            if (fn.StartsWith(XbmcFile.STACK_PREFIX)) {
                //remove the "stack://" prefix
                fn = fn.Replace(XbmcFile.STACK_PREFIX, "");

                foreach (string fileName in fn.SplitWithoutEmptyEntries(XbmcFile.STACK_FILE_SEPARATOR)) {
                    files.AddFile(fileName.Trim().ToWinPath());
                }
            }
            else {
                //if not then just add the filename as is
                files.AddFile(fn.ToWinPath());
            }
            return files;
        }

        /// <summary>Gets the movie's subtitles as a <see cref="HashSet{T}"/> with <see cref="Common.Models.DB.MovieVo.Files.Subtitle">Subtitle</see> elements.</summary>
        /// <returns>A <see cref="HashSet{T}"/> with <see cref="Common.Models.DB.MovieVo.Files.Subtitle">Subtitle</see> elements.</returns>
        public IEnumerable<Subtitle> GetSubtitles() {
            return FileInfo.InfoExists(MediaType.Subtitles)
                       ? FileInfo.Subtitles.Select(xmlSub => (Subtitle)xmlSub)
                       : new List<Subtitle>();
        }

        /// <summary>Gets the movie's video stream details as a <see cref="HashSet{T}"/> with <see cref="Common.Models.DB.MovieVo.Files.Video">Video</see> elements.</summary>
        /// <returns>A <see cref="HashSet{T}"/> with <see cref="Common.Models.DB.MovieVo.Files.Video">Video</see> elements.</returns>
        public IEnumerable<Video> GetVideo() {
            return FileInfo.InfoExists(MediaType.Video)
                       ? FileInfo.Videos.Select(xmlVideo => (Video)xmlVideo)
                       : new List<Video>();
        }

        /// <summary>Gets the movie's subtitles as a <see cref="HashSet{T}"/> with <see cref="Common.Models.DB.MovieVo.Files.Audio">Audio</see> elements.</summary>
        /// <returns>A <see cref="HashSet{T}"/> with <see cref="Common.Models.DB.MovieVo.Files.Audio">Audio</see> elements.</returns>
        public IEnumerable<Audio> GetAudio() {
            return FileInfo.InfoExists(MediaType.Audio)
                       ? FileInfo.Audios.Select(xmlAudio => (Audio)xmlAudio)
                       : new List<Audio>();
        }

        /// <summary>Gets the movie's subtitles as an <see cref="IEnumerable{T}"/> with <see cref="Common.Models.DB.MovieVo.Certification">Certification</see> elements.</summary>
        /// <returns>A <see cref="IEnumerable{T}"/> with <see cref="Common.Models.DB.MovieVo.Certification">Certification</see> elements.</returns>
        public IEnumerable<Certification> GetCertifications() {
            return Certifications != null
                       ? Certifications.Select(xmlCert => (Certification) xmlCert)
                       : new List<Certification>();
        }

        /// <summary>Gets the movie's genres as a <see cref="IEnumerable{T}"/> with <see cref="Common.Models.DB.MovieVo.Genre">Genre</see> elements.</summary>
        /// <returns>A <see cref="IEnumerable{T}"/> with <see cref="Common.Models.DB.MovieVo.Genre">Genre</see> elements.</returns>
        public IEnumerable<Genre> GetGenres() {
            return Genre.GetFromNames(Genres);
        }

        /// <summary>Gets the countries the movie has been shot or produced in as a <see cref="IEnumerable{T}"/> with <see cref="Common.Models.DB.MovieVo.Country">Country</see> elements.</summary>
        /// <returns>A <see cref="IEnumerable{T}"/> with <see cref="Common.Models.DB.MovieVo.Country">Country</see> elements.</returns>
        public IEnumerable<Country> GetCountries() {
            return Country.GetFromNames(Countries);
        }

        /// <summary>Gets the movie's subtitles as a <see cref="IEnumerable{T}"/> with <see cref="ArtBase">Art</see> elements.</summary>
        /// <returns>A <see cref="IEnumerable{T}"/> with <see cref="ArtBase">Art</see> elements.</returns>
        public IEnumerable<ArtBase> GetArt() {
            List<ArtBase> art = new List<ArtBase>();

            //add all Thumbnails/Posters/Covers
            foreach (XbmcXmlThumb thumb in Thumbs) {
                ArtBase a;
                switch (thumb.Aspect.ToLower()) {
                    case "poster":
                        a = new Poster(thumb.Path, thumb.Preview);
                        break;
                    case "cover":
                        a = new Cover(thumb.Path, thumb.Preview);
                        break;
                    default:
                        a = new Art(thumb.Path, thumb.Preview);
                        break;
                }

                art.Add(a);
            }

            //add fanart
            foreach (XbmcXmlThumb thumb in Fanart.Thumbs) {
                art.Add(new Fanart(thumb.Path));
            }

            return art;
        }

        /// <summary>Gets the movie trailer URL.</summary>
        /// <returns>If its a YouTube plugin URI returs the desktop YouTube URL, otherwise returns it unmodified</returns>
        public string GetTrailerUrl() {
            //if the trailer is not empty or null and starts with the YouTube plugin prefix
            //we extract the video Id and return the desktop YouTube video URL
            if (!string.IsNullOrEmpty(Trailer) && Trailer.StartsWith(YT_TRAILER_PREFIX)) {
                string ytId = Trailer.Replace(YT_TRAILER_PREFIX, "");
                return "www.youtube.com/watch?v=" + ytId;
            }
            //otherwise we just return trailer as is
            return Trailer;
        }

        /// <summary>Gets the studio name(s). If more that 1 they are separated by " / "</summary>
        /// <returns>The name of the studios that produced the movie</returns>
        public string GetStudioNames() {
            if (Studios != null) {
                return string.Join(SEPARATOR, Studios);
            }
            return null;
        }

        /// <summary>Gets the movie's online database identifier or <c>null</c> if it doesn't exist.</summary>
        /// <param name="dbName">Name of the online database (Imdb, Tmdb, ...).</param>
        /// <returns>
        /// If <c><paramref name="dbName"/></c> is <c>"imdb"</c> returns the first id that doesn't have an attribute <c>"MovieDb"</c> set or has it set to <c>"imdb"</c>;
        /// otherwise the first id of the specified <c><paramref name="dbName"/></c>.
        /// <para>If the movie does not have the <c><paramref name="dbName"/></c> id set yet it returns <c>null</c>.</para>
        /// </returns>
        private XbmcXmlMovieDbId GetOnlineDbID(string dbName) {
            //if movieDb attribute doesn't exist (is null) the Id is for IMDB
            return dbName.OrdinalEquals("imdb")
                       ? Ids.FirstOrDefault(id => id.MovieDb.OrdinalEquals(dbName) || string.IsNullOrEmpty(id.MovieDb))
                       : Ids.FirstOrDefault(id => id.MovieDb.OrdinalEquals(dbName));
        }

        /// <summary>Gets the folder path from a filepath.</summary>
        /// <param name="fn">The filepath.</param>
        /// <remarks>If filepath is not acessable it tries to manually split the filepath and remove the filename to get the folder path.</remarks>
        /// <returns>A folder path of a specified filename.</returns>
        private static string GetFolderPath(string fn) {
            //if the filename is null we just return null
            if (fn != null) {
                //convert the path to Win compatible
                string winPath = fn.ToWinPath();
                try {
                    //try to open the file to get its directory name
                    return new FileInfo(winPath).DirectoryName;
                }
                catch (Exception e) {
                    //write out the error message
                    Console.WriteLine(e.Message);

                    //try to manually determine the folder path
                    //by looking for the last folder delimiter and
                    //removing evertyhing after it
                    int dirPathEnd = fn.LastIndexOfAny(new[] {'\\', '/'});
                    return fn.Remove(dirPathEnd, fn.Length - dirPathEnd);
                }
            }
            return null;
        }

        #endregion

        #region Serialization

        /// <summary>Serializes the current instance as XML in the specified location</summary>
        /// <param name="xmlSaveLocation">The XML save location.</param>
        public void Serialize(string xmlSaveLocation) {
            XmlSerializer xs = new XmlSerializer(typeof (XbmcXmlMovie));
            xs.Serialize(new XmlIndentedTextWriter(xmlSaveLocation), this);
        }

        /// <summary>Deserializes an instance of <see cref="XbmcXmlMovie"/> from XML at the specified location</summary>
        /// <param name="xmlLocation">The file path of the serialied xml.</param>
        /// <returns>An instance of <see cref="XbmcXmlMovie"/> deserialized from XML at the specified location</returns>
        public static XbmcXmlMovie Load(string xmlLocation) {
            XmlSerializer xs = new XmlSerializer(typeof (XbmcXmlMovie));

            return (XbmcXmlMovie) xs.Deserialize(new XmlTextReader(xmlLocation));
        }

        /// <summary>Deserializes an instance of <see cref="XbmcXmlMovie"/> from XML at the specified location and converts it to <see cref="Movie"/></summary>
        /// <param name="xmlLocation">The file path of the serialied xml.</param>
        /// <returns>An instance of <see cref="Movie"/> converted from deserialized instance of <see cref="XbmcXmlMovie"/> at the specified location</returns>
        public static Movie LoadAsMovie(string xmlLocation) {
            return (Movie) Load(xmlLocation);
        }

        #endregion

        #region Conversion Functions

        /// <summary>Converts the current instance to the <see cref="Common.Models.DB.MovieVo.Movie">Movie</see></summary>
        /// <returns><see cref="Common.Models.DB.MovieVo.Movie">Movie</see> instance that is able to be saved in the cache Database.</returns>
        public Movie ToMovie() {
            return (Movie) this;
        }

        /// <summary>Converts the current instance to the <see cref="Common.Models.DB.XBMC.XbmcMovie">XbmcMovie</see></summary>
        /// <returns><see cref="Common.Models.DB.XBMC.XbmcMovie">XbmcMovie</see> instance that is able to be saved in the XBMC database.</returns>
        public XbmcMovie ToXbmcMovie() {
            return (XbmcMovie) this;
        }

        #endregion

        #region Conversion Operators

        /// <summary>Converts an instance of <see cref="XbmcXmlMovie"/> to <see cref="Common.Models.DB.MovieVo.Movie">Movie</see> by explicit casting</summary>
        /// <param name="mx">The <see cref="XbmcXmlMovie"/> to convert.</param>
        /// <returns><see cref="XbmcXmlMovie"/> converted to an instance of <see cref="Common.Models.DB.MovieVo.Movie">Movie</see></returns>
        public static explicit operator Movie(XbmcXmlMovie mx) {
            Movie mv = new Movie {
                Aired = mx.Aired,
                ImdbID = mx.ImdbId,
                LastPlayed = mx.LastPlayed,
                OriginalTitle = mx.OriginalTitle,
                Premiered = mx.Premiered,
                PlayCount = mx.PlayCount,
                RatingAverage = mx.Rating,
                ReleaseDate = mx.ReleaseDate,
                Runtime = mx.RuntimeInSeconds,
                Set = new Set(mx.Set),
                SortTitle = mx.SortTitle,
                Title = mx.Title,
                TmdbID = mx.TmdbId,
                Top250 = mx.Top250,
                Trailer = mx.GetTrailerUrl(),
                Watched = mx.Watched,
                ReleaseYear = mx.Year,
                Arts = new HashSet<ArtBase>(mx.GetArt()),
                Directors = mx.GetDirectors(),
                Writers = mx.GetWriters(),
                Genres = new HashSet<Genre>(Genre.GetFromNames(mx.Genres)),
                Studios = new HashSet<Studio>(Studio.GetFromNames(mx.Studios)),
                Countries = new HashSet<Country>(Country.GetFromNames(mx.Countries)),
                Certifications = new HashSet<Certification>(mx.GetCertifications()),
                Audios = new HashSet<Audio>(mx.GetAudio()),
                Videos = new HashSet<Video>(mx.GetVideo()),
                Subtitles = new HashSet<Subtitle>(mx.GetSubtitles()),
                //Files = mx.GetFiles()
            };
            mv.ActorsLink.UnionWith(mx.Actors.Select(a => new MovieActor(mv, new Person(a.Name, a.Thumb), a.Role)));
            mv.Plots.Add(new Plot(mx.Plot, mx.Outline, mx.Tagline, null));

            return mv;
        }

        /// <summary>Converts an instance of <see cref="XbmcXmlMovie"/> to <see cref="Common.Models.DB.XBMC.XbmcMovie">XbmcMovie</see> by explicit casting</summary>
        /// <param name="xm">The <see cref="XbmcXmlMovie"/> to convert.</param>
        /// <returns><see cref="XbmcXmlMovie"/> converted to an instance of <see cref="Common.Models.DB.XBMC.XbmcMovie">XbmcMovie</see></returns>
        public static explicit operator XbmcMovie(XbmcXmlMovie xm) {
            XbmcMovie mv = new XbmcMovie {
                //WARNING: ToArray() call copying
                CountryNames = xm.Countries.ToArray(),
                DirectorNames = xm.Directors.ToArray(),
                GenreNames = xm.Genres.ToArray(),
                FolderPath = GetFolderPath(xm.FilenameAndPath),
                ImdbId = xm.ImdbId,
                ImdbTop250 = xm.Top250.ToString(CultureInfo.InvariantCulture),
                MpaaRating = xm.MPAA,
                OriginalTitle = xm.OriginalTitle,
                Plot = xm.Plot,
                PlotOutline = xm.Outline,
                Rating = xm.Rating.ToICString(),
                ReleaseYear = xm.Year.ToICString(),
                Runtime = (xm.RuntimeInSeconds ?? 0).ToICString(),
                Set = new XbmcSet(xm.Set),
                StudioNames = xm.GetStudioNames(),
                Tagline = xm.Tagline,
                Title = xm.Title,
                SortTitle = xm.SortTitle,
                TrailerUrl = xm.Trailer,
                Votes = xm.Votes,
                WriterNames = xm.CreditsFormatted,
                File = new XbmcFile(
                    xm.DateAdded,
                    xm.LastPlayed.ToString(CultureInfo.InvariantCulture),
                    xm.PlayCount,
                    xm.FilenameAndPath
                    )
            };

            //xm.FanartUrls //XML
            if (xm.Fanart != null) {
                mv.FanartUrls = xm.Fanart.SerializeToXml();
            }

            //xm.Thumbnails //XML
            if (xm.Thumbs != null) {
                StringBuilder sb = new StringBuilder();
                foreach (XbmcXmlThumb thumb in xm.Thumbs) {
                    sb.Append(thumb.SerializeToString());
                }

                mv.Thumbnails = sb.ToString();
            }

            return mv;
        }

        #endregion
    }
}