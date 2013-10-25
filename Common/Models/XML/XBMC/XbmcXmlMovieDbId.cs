using System;
using System.Xml.Serialization;

namespace Common.Models.XML.XBMC {
    /// <remarks/>
    [Serializable]
    public class XbmcXmlMovieDbId {

        public XbmcXmlMovieDbId() {
        }

        public XbmcXmlMovieDbId(string value) {
            Value = value;
        }

        public XbmcXmlMovieDbId(string value, string movieDb) : this(value) {
            MovieDb = movieDb;
        }

        /// <remarks/>
        [XmlAttribute("moviedb")]
        public string MovieDb { get; set; }

        /// <remarks/>
        [XmlText]
        public string Value { get; set; }
    }
}