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
using Common.Models.DB.MovieVo;
using Common.Models.DB.XBMC;
using File = Common.Models.DB.MovieVo.File;

namespace Common.Models.XML.XBMC {
    /// <remarks/>
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot("movie", Namespace = "", IsNullable = false)]
    public class XbmcXmlMovie {
        private const string SEPARATOR = " / ";

        public XbmcXmlMovie() {
            FileInfo = new XbmcXmlFileInfo();
        }

        /// <remarks/>
        [XmlElement("title", Form = XmlSchemaForm.Unqualified, Order = 1)]
        public string Title { get; set; }

        /// <remarks/>
        [XmlElement("originaltitle", Form = XmlSchemaForm.Unqualified, Order = 2)]
        public string OriginalTitle { get; set; }

        /// <remarks/>
        [XmlElement("sorttitle", Form = XmlSchemaForm.Unqualified, Order = 3)]
        public string SortTitle { get; set; }

        /// <remarks/>
        [XmlElement("rating", Form = XmlSchemaForm.Unqualified, Order = 4)]
        public float Rating { get; set; }

        /// <remarks/>
        [XmlElement("epbookmark", Form = XmlSchemaForm.Unqualified, Order = 5)]
        public float EPBookmark { get; set; }

        /// <remarks/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("year", Form = XmlSchemaForm.Unqualified, DataType = "gYear", Order = 6)]
        public string YearString { get; set; }

        [XmlIgnore]
        public int Year {
            get {
                int yr;
                int.TryParse(YearString, out yr);

                return yr;
            }
            set { YearString = value.ToString(CultureInfo.InvariantCulture); }
        }

        /// <remarks/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("releasedate", Form = XmlSchemaForm.Unqualified, Order = 7)]
        public string ReleaseDateString { get; set; }

        [XmlIgnore]
        public DateTime ReleaseDate {
            get {
                DateTime releaseDate;
                DateTime.TryParseExact(ReleaseDateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                return releaseDate;
            }
            set { ReleaseDateString = value.ToString("dd.MM.yyyy"); }
        }

        /// <remarks/>
        [XmlElement("top250", Form = XmlSchemaForm.Unqualified, Order = 8)]
        public uint Top250 { get; set; }

        /// <remarks/>
        [XmlElement("votes", Form = XmlSchemaForm.Unqualified, Order = 9)]
        public string Votes { get; set; }

        /// <remarks/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("certification", Form = XmlSchemaForm.Unqualified, Order = 10)]
        public string CertificationsString { get; set; }

        [XmlIgnore]
        public XbmcXmlCertification[] Certifications {
            get {
                string[] countryCerts = CertificationsString.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

                int numCerts = countryCerts.Length;
                XbmcXmlCertification[] xbmcCerts = new XbmcXmlCertification[numCerts];
                for (int i = 0; i < numCerts; i++) {
                    string[] kvp = countryCerts[i].Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                    if (kvp.Length < 2) {
                        continue;
                    }

                    xbmcCerts[i] = new XbmcXmlCertification(kvp[0], kvp[1]);
                }

                return xbmcCerts;
            }
            set {
                int numCerts = value.Length;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < numCerts; i++) {
                    sb.Append(value[i].Country + ":" + value[i].Rating);
                    if (i < numCerts - 1) {
                        sb.Append(SEPARATOR);
                    }
                }
                CertificationsString = sb.ToString();
            }
        }

        /// <remarks/>
        [XmlElement("outline", Form = XmlSchemaForm.Unqualified, Order = 11)]
        public string Outline { get; set; }

        /// <remarks/>
        [XmlElement("plot", Form = XmlSchemaForm.Unqualified, Order = 12)]
        public string Plot { get; set; }

        /// <remarks/>
        [XmlElement("tagline", Form = XmlSchemaForm.Unqualified, Order = 13)]
        public string Tagline { get; set; }

        /// <remarks/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("runtime", Form = XmlSchemaForm.Unqualified, Order = 14)]
        public string RuntimeString { get; set; }

        [XmlIgnore]
        public long? RuntimeInSeconds {
            get {
                //runtime is saved as a string with seconds integer divided by 60 with appended text " min"
                string runtimeInMinutes = RuntimeString.Replace(" min", "");

                long runtimeVal;
                if (long.TryParse(runtimeInMinutes, out runtimeVal)) {
                    return runtimeVal > 0
                        ? (long?)(runtimeVal * 60)
                        : null;
                }
                return null;
            }
            set {
                RuntimeString = (value.HasValue) ? (value / 60) + " min" : null;
            }
        }

        /// <remarks/>
        [XmlElement("watched", Form = XmlSchemaForm.Unqualified, Order = 15)]
        public bool Watched { get; set; }

        /// <remarks/>
        [XmlElement("thumb", Form = XmlSchemaForm.Unqualified, Order = 16)]
        public XbmcXmlThumb[] Thumbs { get; set; }

        /// <remarks/>
        [XmlElement("fanart", Form = XmlSchemaForm.Unqualified, Order = 17)]
        public XbmcXmlFanart Fanart { get; set; }

        /// <remarks/>
        [XmlElement("mpaa", Form = XmlSchemaForm.Unqualified, Order = 18)]
        public string MPAA { get; set; }

        /// <remarks/>
        [XmlElement("playcount", Form = XmlSchemaForm.Unqualified, Order = 19)]
        public uint PlayCount { get; set; }

        /// <remarks/>
        [XmlElement("lastplayed", Form = XmlSchemaForm.Unqualified, DataType = "date", Order = 20)]
        public DateTime LastPlayed { get; set; }

        /// <remarks/>
        [XmlElement("id", Form = XmlSchemaForm.Unqualified, Order = 21)]
        public XbmcXmlMovieDbId[] Ids { get; set; }

        [XmlIgnore]
        public string ImdbId {
            get {
                if (Ids != null) {
                    XbmcXmlMovieDbId imdbID = GetImdbID("imdb");
                    if (imdbID != null) {
                        return imdbID.Value;
                    }
                }
                return null;
            }
            set {
                XbmcXmlMovieDbId imdb = GetImdbID("imdb");
                if (imdb != null) {
                    imdb.Value = value;
                }
            }
        }

        [XmlIgnore]
        public string TmdbId {
            get {
                if (Ids != null) {
                    XbmcXmlMovieDbId tmdbId = GetImdbID("tmdb");
                    if (tmdbId != null) {
                        return tmdbId.Value;
                    }
                }
                return null;
            }
            set {
                XbmcXmlMovieDbId tmdb = GetImdbID("tmdb");
                if (tmdb != null) {
                    tmdb.Value = value;
                }
            }
        }

        [XmlElement("filenameandpath", Form = XmlSchemaForm.Unqualified, Order = 22)]
        public string FilenameAndPath { get; set; }

        /// <remarks/>
        [XmlElement("genre", Form = XmlSchemaForm.Unqualified, Order = 23)]
        public string[] Genres { get; set; }

        /// <remarks/>
        [XmlElement("country", Form = XmlSchemaForm.Unqualified, Order = 24)]
        public string[] Countries { get; set; }

        /// <remarks/>
        [XmlElement("set", Form = XmlSchemaForm.Unqualified, Order = 25)]
        public string Set { get; set; }

        /// <remarks/>
        [XmlElement("credits", Form = XmlSchemaForm.Unqualified, Order = 26)]
        public string[] Credits { get; set; }

        [XmlIgnore]
        public string CreditsFormatted {
            get { return string.Join(SEPARATOR, Credits); }
            set { Credits = value.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries); }
        }

        /// <remarks/>
        [XmlElement("director", Form = XmlSchemaForm.Unqualified, Order = 27)]
        public string[] Directors { get; set; }

        /// <remarks/>
        [XmlElement("premiered", Form = XmlSchemaForm.Unqualified, DataType = "date", Order = 28)]
        public DateTime Premiered { get; set; }

        /// <remarks/>
        [XmlElement("status", Form = XmlSchemaForm.Unqualified, Order = 29)]
        public string Status { get; set; }

        /// <remarks/>
        [XmlElement("code", Form = XmlSchemaForm.Unqualified, Order = 30)]
        public string Code { get; set; }

        /// <remarks/>
        [XmlElement("aired", Form = XmlSchemaForm.Unqualified, DataType = "date", Order = 31)]
        public DateTime Aired { get; set; }

        /// <remarks/>
        [XmlElement("studio", Form = XmlSchemaForm.Unqualified, Order = 32)]
        public string[] Studios { get; set; }


        /// <remarks/>
        [XmlElement("trailer", Form = XmlSchemaForm.Unqualified, DataType = "anyURI", Order = 33)]
        public string Trailer { get; set; }

        /// <remarks/>
        [XmlElement("fileinfo", Form = XmlSchemaForm.Unqualified, Order = 34)]
        public XbmcXmlFileInfo FileInfo { get; set; }

        /// <remarks/>
        [XmlElement("actor", Form = XmlSchemaForm.Unqualified, Order = 35)]
        public XbmcXmlActor[] Actors { get; set; }

        /// <remarks/>
        [XmlElement("resume", Form = XmlSchemaForm.Unqualified, Order = 36)]
        public XbmcXmlResumeInfo ResumingInfo { get; set; }

        /// <remarks/>
        [XmlElement("dateadded", Form = XmlSchemaForm.Unqualified, Order = 37)]
        public string DateAdded { get; set; }

        #region Utility Functions

        public ICollection<File> GetFiles() {
            ICollection<File> files = new HashSet<File>();
            if (string.IsNullOrEmpty(FilenameAndPath)) {
                return files;
            }

            string fn = FilenameAndPath;

            if (fn.StartsWith("stack://")) {
                fn = fn.Replace("stack://", "");
                foreach (string fileName in fn.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)) {
                    AddFile(ToWinPath(fileName.Trim()), files);
                }
            }
            else {
                AddFile(ToWinPath(fn), files);
            }
            return files;
        }

        private bool AddFile(string fn, ICollection<File> files) {
            try {
                FileInfo fi = new FileInfo(fn);

                files.Add(new File(fi.Name, fi.Extension, fi.FullName, fi.Length));
            }
            catch (Exception) {
                return false;
            }
            return true;
        }

        private string ToWinPath(string fn) {
            if (fn.StartsWith("smb://")) {
                //Win does not recognize samba protocol instead
                //they use double backslash for network paths
                fn = fn.Replace("smb://", @"\\");
            }
            return fn;
        }

        public ICollection<Subtitle> GetSubtitles() {
            return FileInfo.InfoExists(MediaType.Subtitles)
               ? (ICollection<Subtitle>)FileInfo.Subtitles.ToSubtitleArray()
               : new HashSet<Subtitle>();
        }

        public ICollection<Video> GetVideo() {
            return FileInfo.InfoExists(MediaType.Video)
                ? (ICollection<Video>)FileInfo.Videos.ToVideoArray()
                : new HashSet<Video>();
        }

        public ICollection<Audio> GetAudio() {
            return FileInfo.InfoExists(MediaType.Audio)
                ? (ICollection<Audio>)FileInfo.Audios.ToAudioArray()
                : new HashSet<Audio>();
        }

        public ICollection<Certification> GetCertifications() {
            return Certifications != null
               ? (ICollection<Certification>)Certifications.ToCertificationArray()
               : new HashSet<Certification>();
        }

        public ICollection<MoviePerson> GetCast() {
            ICollection<MoviePerson> moviePersons = new HashSet<MoviePerson>();

            //add actors (convert XbmcActor to Actor)
            foreach (XbmcXmlActor actor in Actors) {
                moviePersons.Add(new MoviePerson((Actor)actor));
            }

            //Add credited writers
            foreach (string writer in Credits) {
                moviePersons.Add(new MoviePerson(writer, "writer"));
            }

            foreach (string director in Directors) {
                moviePersons.Add(new MoviePerson(director, "director"));
            }

            return moviePersons;
        }

        public ICollection<Art> GetArt() {
            ICollection<Art> art = new HashSet<Art>();

            //add all Thumbnails/Posters/Covers
            foreach (XbmcXmlThumb thumb in Thumbs) {
                art.Add(new Art(thumb.Aspect, thumb.URL));

                //if preview (small copy) exists add it aswell
                if (!string.IsNullOrEmpty(thumb.Preview)) {
                    art.Add(new Art(thumb.Aspect + " preview", thumb.Preview));
                }
            }

            //add fanart
            foreach (XbmcXmlThumb thumb in Fanart.Thumbs) {
                art.Add(new Art("fanart", thumb.URL));
            }

            return art;
        }

        public string GetTrailerUrl() {
            const string PREFIX = "plugin://plugin.video.youtube/?action=play_video&amp;videoid=";

            if (!string.IsNullOrEmpty(Trailer) && Trailer.StartsWith(PREFIX)) {
                string ytId = Trailer.Replace(PREFIX, "");
                return "www.youtube.com/watch?v=" + ytId;
            }
            return null;
        }

        public string GetStudioNames() {
            return string.Join(SEPARATOR, Studios);
        }

        private XbmcXmlMovieDbId GetImdbID(string dbName) {
            return Ids.FirstOrDefault(id =>
                string.Equals(id.MovieDb, dbName, StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrEmpty(id.MovieDb)
            );
        }
        #endregion

        #region Serialization
        public void Serialize(string xmlSaveLocation) {
            XmlSerializer xs = new XmlSerializer(typeof(XbmcXmlMovie));
            xs.Serialize(new XmlIndentedTextWriter(xmlSaveLocation), this);
        }

        public static XbmcXmlMovie Load(string xmlLocation) {
            XmlSerializer xs = new XmlSerializer(typeof(XbmcXmlMovie));

            return (XbmcXmlMovie)xs.Deserialize(new XmlTextReader(xmlLocation));            
        }

        public static Movie LoadAsMovie(string xmlLocation) {
            return (Movie) Load(xmlLocation);
        }
        #endregion

        #region Conversion Functions
        public Movie ToMovie() {
            return (Movie) this;
        }

        public XbmcMovie ToXbmcMovie() {
            return (XbmcMovie)this;
        }
        #endregion

        #region Conversion Operators
        public static explicit operator Movie(XbmcXmlMovie mx) {
            Movie mv = new Movie {
                Aired = mx.Aired,
                ImdbID = mx.ImdbId,
                LastPlayed = mx.LastPlayed,
                OriginalTitle = mx.OriginalTitle,
                Permiered = mx.Premiered,
                PlayCount = (int)mx.PlayCount,
                RatingAverage = mx.Rating,
                ReleaseDate = mx.ReleaseDate,
                Runtime = mx.RuntimeInSeconds,
                Set = mx.Set,
                SortTitle = mx.SortTitle,
                Title = mx.Title,
                TmdbID = mx.TmdbId,
                Top250 = (int)mx.Top250,
                Trailer = mx.GetTrailerUrl(),
                Watched = mx.Watched,
                Year = mx.Year,
                Art = mx.GetArt(),
                Cast = mx.GetCast(),
                Genres = Genre.GetFromNames(mx.Genres),
                Studios = Studio.GetFromNames(mx.Studios),
                Countries = Country.GetFromNames(mx.Countries),

                Certifications = mx.GetCertifications(),
                Audio = mx.GetAudio(),
                Videos = mx.GetVideo(),
                Subtitles = mx.GetSubtitles(),
                Files = mx.GetFiles()
            };
            mv.Plot.Add(new Plot(mx.Plot, mx.Outline, mx.Tagline, null));

            return mv;
        }

        public static explicit operator XbmcMovie(XbmcXmlMovie xmlMovie) {
            XbmcMovie xm = new XbmcMovie {
                CountryNames = xmlMovie.Countries,
                DirectorNames = xmlMovie.Directors,
                FolderPath = xmlMovie.FilenameAndPath,
                GenreNames = xmlMovie.Genres,
                ImdbId = xmlMovie.ImdbId,
                ImdbTop250 = xmlMovie.Top250.ToString(CultureInfo.InvariantCulture),
                MpaaRating = xmlMovie.MPAA,
                OriginalTitle = xmlMovie.OriginalTitle,
                Plot = xmlMovie.Plot,
                PlotOutline = xmlMovie.Outline,
                Rating = xmlMovie.Rating.ToString(CultureInfo.InvariantCulture),
                ReleaseYear = xmlMovie.Year.ToString(CultureInfo.InvariantCulture),
                Runtime = (xmlMovie.RuntimeInSeconds ?? 0).ToString(CultureInfo.InvariantCulture),
                //Set = new XbmcSet(xmlMovie.Set),
                StudioNames = xmlMovie.GetStudioNames(),
                Tagline = xmlMovie.Tagline,
                Title = xmlMovie.Title,
                TitleSort = xmlMovie.SortTitle,
                TrailerUrl = xmlMovie.Trailer,
                Votes = xmlMovie.Votes,
                WriterNames = xmlMovie.CreditsFormatted,
                File = new XbmcFile(
                    xmlMovie.DateAdded,
                    xmlMovie.LastPlayed.ToString(CultureInfo.InvariantCulture),
                    xmlMovie.PlayCount
                )
            };

            //xm.FanartUrls //XML
            if (xmlMovie.Fanart != null) {
                xm.FanartUrls = xmlMovie.Fanart.SerializeToXml();
            }

            //xm.Thumbnails //XML
            if (xmlMovie.Thumbs != null) {
                StringBuilder sb = new StringBuilder();
                foreach (XbmcXmlThumb thumb in xmlMovie.Thumbs) {
                    sb.Append(thumb.SerializeToString());
                }

                xm.Thumbnails = sb.ToString();
            }

            return xm;
        }
        #endregion
    }
}