using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Frost.Common;
using Frost.Common.Models;

namespace Frost.Providers.Xtreamer.NFO {

    /// <summary>Represents an infromation about a movie in Xtreamer Movie Jukebox database that is ready to be serialized.</summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot("movie", Namespace = "", IsNullable = false)]
    public class XjbXmlMovie {

        private const string SEPARATOR = " / ";

        /// <summary>Initializes a new instance of the <see cref="XjbXmlMovie"/> class.</summary>
        public XjbXmlMovie() {
            Votes = "";
            MPAA = "";
            Credits = "";
            Fileinfo = "";
            GenreString = "";
        }

        public XjbXmlMovie(IMovie movie) {
            FromIMovie(movie);
        }

        #region Properties

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

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        [XmlElement("sorttitle", Form = XmlSchemaForm.Unqualified)]
        public string SortTitle { get; set; }

        /// <summary>Gets or sets Imdb ID</summary>
        /// <value>The IMDB Id</value>
        [XmlElement("id", Form = XmlSchemaForm.Unqualified)]
        public string ImdbId { get; set; }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        [XmlElement("year", Form = XmlSchemaForm.Unqualified)]
        public int Year { get; set; }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        [XmlElement("releasedate", Form = XmlSchemaForm.Unqualified)]
        public string ReleaseDate { get; set; }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        [XmlElement("rating", Form = XmlSchemaForm.Unqualified)]
        public float AverageRating { get; set; }

        /// <summary>Gets or sets the number of votes the average rating was computed from</summary>
        /// <value>The number of ratings</value>
        [XmlElement("votes", Form = XmlSchemaForm.Unqualified)]
        public string Votes { get; set; }

        /// <summary>Gets or sets the US MPAA movie rating and the reason for it</summary>
        /// <value>The US Movie rating</value>
        [XmlElement("mpaa", Form = XmlSchemaForm.Unqualified)]
        public string MPAA { get; set; }

        /// <summary>Gets or sets the certifications for other countries</summary>
        /// <value>Other country ratings</value>
        /// <remarks>If more than 1 they are split by " / " with country and rating split by ":" without space</remarks>
        /// <example>\eg{ ''<c>us:R / au:M</c>''}</example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("certification", Form = XmlSchemaForm.Unqualified)]
        public string CertificationsString { get; set; }

        [XmlIgnore]
        public XjbCertification[] Certifications {
            get { return XjbCertification.ParseCertificationsString(CertificationsString); }
            set { CertificationsString = string.Join<XjbCertification>(SEPARATOR, value); }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres separated by " / "</value>
        /// <remarks>Genre names are saved in a single string with genres divided by " / "</remarks>
        /// <example>\eg{ ''<c>Drama / Thriller</c>''}</example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("genre", Form = XmlSchemaForm.Unqualified)]
        public string GenreString { get; set; }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        [XmlIgnore]
        public string[] Genres {
            get {
                return !string.IsNullOrEmpty(GenreString)
                           ? GenreString.SplitWithoutEmptyEntries(SEPARATOR)
                           : null;
            }
            set { GenreString = string.Join(SEPARATOR, value); }
        }

        /// <summary>The studio that produced this movie.</summary>
        /// <value>The studio name.</value>
        [XmlElement("studio", Form = XmlSchemaForm.Unqualified)]
        public string Studio { get; set; }

        /// <summary>Gets or sets the director name.</summary>
        /// <remarks>If more than 1 they are separated by " / "</remarks>
        /// <value>Name of the person that directed the movie</value>
        [XmlElement("director", Form = XmlSchemaForm.Unqualified)]
        public string Director { get; set; }

        [XmlIgnore]
        public string[] Directors {
            get {
                string[] entries = Director.SplitWithoutEmptyEntries("/");
                for (int i = 0; i < entries.Length; i++) {
                    entries[i] = entries[i].Trim();
                }

                return entries;
            }
            set { Director = string.Join("  /  ", value); }
        }

        /// <summary>Gets or sets the name of the writer(s).</summary>
        /// <remarks>If more than 1 they are separated by " / "</remarks>
        /// <value>The names of script writer(s)</value>
        [XmlElement("credits", Form = XmlSchemaForm.Unqualified)]
        public string Credits { get; set; }

        /// <summary>Gets or sets the tagline (short one-liner).</summary>
        /// <value>The tagline (short promotional slogan / one-liner / clarification).</value>
        [XmlElement("tagline", Form = XmlSchemaForm.Unqualified)]
        public string Tagline { get; set; }

        /// <summary>Gets or sets the plot outline.</summary>
        /// <value>Outline, a short story summary</value>
        [XmlElement("outline", Form = XmlSchemaForm.Unqualified)]
        public string Outline { get; set; }

        /// <summary>Gets or sets the plot.</summary>
        /// <value>The plot.</value>
        [XmlElement("plot", Form = XmlSchemaForm.Unqualified)]
        public string Plot { get; set; }

        /// <summary>Gets or sets the runtime in minutes pretty printed</summary>
        /// <value>Runtime in minutes as "X min"</value>
        /// <remarks>runtime is saved as a string with seconds integer divided by 60 with appended text " min"</remarks>
        [XmlElement("runtime", Form = XmlSchemaForm.Unqualified)]
        public string RuntimeString { get; set; }

        /// <summary>Gets or sets the runtime in minutes</summary>
        /// <value>Runtime in minutes</value>
        /// <remarks>runtime is saved as a string with seconds integer divided by 60 with appended text " min"</remarks>
        [XmlIgnore]
        public long? Runtime {
            get {
                //runtime is saved as a string with seconds integer divided by 60 with appended text " min"
                string runtimeInMinutes = RuntimeString.Replace(" min", "");

                long runtimeVal;
                if (long.TryParse(runtimeInMinutes, out runtimeVal)) {
                    return runtimeVal > 0
                               ? (long?) runtimeVal
                               : null;
                }
                return null;
            }
            set {
                RuntimeString = (value.HasValue)
                                    ? value + " min"
                                    : null;
            }
        }

        /// <summary>Gets or sets the actors that starred in the movie.</summary>
        /// <value>The actors that preformed in this movie.</value>
        [XmlElement("actor", Form = XmlSchemaForm.Unqualified)]
        public XjbXmlActor[] Actors { get; set; }

        /// <summary>Gets or sets the file info of this movie.</summary>
        /// <value>The file info of this movie (video/audio/subtitle details).</value>
        [XmlElement("fileinfo", Form = XmlSchemaForm.Unqualified)]
        public string Fileinfo { get; set; }

        #endregion


        #region Serialization

        /// <summary>Serializes the instance to XML in the specified save location.</summary>
        /// <param name="xmlSaveLocation">The XML save location.</param>
        public void Serialize(string xmlSaveLocation) {
            XmlSerializer xs = new XmlSerializer(typeof(XjbXmlMovie));
            xs.Serialize(new XmlIndentedTextWriter(xmlSaveLocation), this);
        }

        /// <summary>Loads the serialized movie info XML file from the specified path.</summary>
        /// <param name="pathToXml">The path to info XML file.</param>
        /// <returns>Instance of <see cref="XjbXmlMovie"/> deserialized from info XML in the given path</returns>
        /// <exception cref="FileNotFoundException">Throws when specified file path can not be found</exception>
        /// <exception cref="InvalidOperationException">Throws when the specified file is not a serialized instance of <see cref="XjbXmlMovie"/>.</exception>
        public static XjbXmlMovie Load(string pathToXml) {
            XmlSerializer xs = new XmlSerializer(typeof(XjbXmlMovie));

            return (XjbXmlMovie) xs.Deserialize(new XmlTextReader(pathToXml));
        }
        #endregion

        #region Conversion

        private void FromIMovie(IMovie movie) {
            Certifications = movie.Certifications.Select(c => new XjbCertification(c)).ToArray();
            Director = GetDirectorNames(movie);
            GenreString = GetGenreNames(movie);
            ImdbId = movie.ImdbID;
            OriginalTitle = movie.OriginalTitle;
            //Outline = movie.MainPlot.Summary,
            //Plot = movie.MainPlot.Full,
            AverageRating = (float) (movie.RatingAverage ?? 0);
            //TODO: CHECK FOR CORECT FORMAT
            ReleaseDate = movie.ReleaseDate.HasValue ? movie.ReleaseDate.Value.ToString(CultureInfo.InvariantCulture) : null;
            Runtime = movie.Runtime.HasValue
                          ? (movie.Runtime / 60)
                          : 0;

            SortTitle = movie.Title;
            Studio = GetStudioNamesFormatted(movie);
            //Tagline = movie.MainPlot.Summary,
            Title = movie.Title;
            Year = (int) (movie.ReleaseYear ?? 0);
            Actors = GetXjbXmlActors(movie).ToArray();
            MPAA = GetMpaaRating(movie);
            Credits = string.Join(SEPARATOR, movie.Writers.Select(p => p.Name));
            Fileinfo = "";
        }

        /// <summary>Gets the director names in a formatted string.</summary>
        /// <returns>The director names in a single string separated with " / "</returns>
        private static string GetDirectorNames(IMovie movie) {
            string directorsJoin = string.Join(SEPARATOR, movie.Directors.Select(d => d.Name));
            return string.IsNullOrEmpty(directorsJoin)
                       ? null
                       : directorsJoin;
        }

        /// <summary>Gets the genre names in a formatted string.</summary>
        /// <returns>The genre names in a single string separated with " / "</returns>
        /// <example>\eg{ <c>"Horor / SciFi"</c>}</example>
        private static string GetGenreNames(IMovie movie) {
            return string.Join(SEPARATOR, movie.Genres.Select(g => g.Name));
        }

        /// <summary>Gets the studio names in a formatted string.</summary>
        /// <returns>The studio names in a single string separated with " / "</returns>
        /// <example>\eg{ ''<c>MGM / Fox</c>''}</example>
        private static string GetStudioNamesFormatted(IMovie movie) {
            return String.Join(SEPARATOR, movie.Studios.Select(stud => stud.Name));
        }

        /// <summary>Gets the movie actors as an <see cref="IEnumerable{T}"/> of <see cref="XjbXmlActor">XjbXmlActor</see> instances.</summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of this movie actors as <see cref="XjbXmlActor">XjbXmlActor</see> instances</returns>
        public static IEnumerable<XjbXmlActor> GetXjbXmlActors(IMovie movie) {
            return movie.Actors.Select(a => new XjbXmlActor(a));
        }

        public static string GetMpaaRating(IMovie movie) {
            ICertification certification = movie.Certifications.FirstOrDefault(c => c.Country.ISO3166.Alpha3 == "USA");
            return certification != null
                ? certification.Rating 
                : null;
        }

        #endregion
    }

}
