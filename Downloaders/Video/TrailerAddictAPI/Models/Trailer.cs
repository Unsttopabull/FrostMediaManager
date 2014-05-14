using System.Xml.Serialization;

namespace Frost.SharpTrailerAddictAPI.Models {

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class Trailer {

        /// <summary>Gets or sets the video title.</summary>
        /// <value>The video title.</value>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>Gets or sets the video link.</summary>
        /// <value>The video link.</value>
        [XmlElement("link")]
        public string Link { get; set; }

        /// <summary>Gets or sets the time the video was published.</summary>
        /// <value>The time the video was published.</value>
        [XmlElement("pubDate")]
        public string PublishedDate { get; set; }

        /// <summary>Gets or sets video id.</summary>
        /// <value>The video id</value>
        [XmlElement("trailer_id")]
        public uint TrailerID { get; set; }

        /// <summary>Gets or sets the Imdb Id of the movie this video if for.</summary>
        /// <value>The Imdb Id of the movie this video if for.</value>
        [XmlElement("imdb")]
        public object ImdbId { get; set; }

        /// <summary>Gets or sets the Embed HTML code for this video.</summary>
        /// <value>The Embed HTML code for this video</value>
        [XmlElement("embed")]
        public string Embed { get; set; }
    }

}