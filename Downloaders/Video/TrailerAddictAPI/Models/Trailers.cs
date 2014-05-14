using System.Xml.Serialization;

namespace Frost.SharpTrailerAddictAPI.Models {

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot("trailers", Namespace = "", IsNullable = false)]
    public class Trailers {
        /// <remarks/>
        [XmlElement("trailer")]
        public Trailer[] Trailer { get; set; }
    }

}