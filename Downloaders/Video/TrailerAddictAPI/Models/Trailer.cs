using System.Xml.Serialization;

namespace SharpTrailerAddictAPI.Models {

    [XmlType("trailersTrailer", AnonymousType = true)]
    public class Trailer {

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("pubDate")]
        public string PublishedDate { get; set; }

        [XmlElement("trailer_id")]
        public uint TrailerID { get; set; }

        [XmlElement("embed")]
        public object ImdbMovieId { get; set; }

        [XmlElement("title")]
        public string Embed { get; set; }
    }

}