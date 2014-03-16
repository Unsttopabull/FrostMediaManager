using System;
using System.Xml.Serialization;

namespace Frost.Providers.Xbmc.NFO {

    /// <summary>Represent the online movie databse indentifier</summary>
    [Serializable]
    public class XbmcXmlMovieDbId {

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlMovieDbId"/> class.</summary>
        public XbmcXmlMovieDbId() {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlMovieDbId"/> class.</summary>
        /// <param name="indentifier">The value of the indentifier.</param>
        /// <remarks>If the <see cref="XbmcXmlMovieDbId.MovieDb"/> is not set it defaults to "IMDB"</remarks>
        public XbmcXmlMovieDbId(string indentifier) {
            Indentifier = indentifier;
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlMovieDbId"/> class.</summary>
        /// <param name="indentifier">The value of the indentifier.</param>
        /// <param name="movieDb">The online movie database this indentifier is for.</param>
        public XbmcXmlMovieDbId(string indentifier, string movieDb) : this(indentifier) {
            MovieDb = movieDb;
        }

        /// <summary>Gets or sets the online movie database this indentifier is for.</summary>
        /// <value>The online movie database this indentifier is for.</value>
        /// <remarks>If null it defaults to "IMDB".</remarks>
        /// <example>\eg{<c>"Imdb"</c>, <c>"Tmdb"</c>}</example>
        [XmlAttribute("moviedb")]
        public string MovieDb { get; set; }

        /// <summary>Gets or sets the indentifier of the movie in the <see cref="XbmcXmlMovieDbId.MovieDb"/> database.</summary>
        /// <value>The value of the indentifier.</value>
        /// <example>\eg{ Imdb: <c>tt0068646</c>}</example>
        [XmlText]
        public string Indentifier { get; set; }

    }

}
