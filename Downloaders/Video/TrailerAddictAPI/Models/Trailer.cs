using System.Xml.Serialization;

namespace SharpTrailerAddictAPI.Models {

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class Trailer {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("pubDate")]
        public string PublishedDate { get; set; }

        [XmlElement("trailer_id")]
        public uint TrailerID { get; set; }

        [XmlElement("imdb")]
        public object ImdbId { get; set; }

        [XmlElement("embed")]
        public string Embed { get; set; }
    }

}