using System;
using System.IO;
using System.Xml.Serialization;

namespace Common.Models.XML.XBMC {
    /// <remarks/>
    [Serializable]
    public class XbmcXmlThumb {

        public XbmcXmlThumb() {
        }

        public XbmcXmlThumb(string value, string aspect, string preview) {
            Aspect = aspect;
            Preview = preview;
            URL = value;
        }

        public XbmcXmlThumb(string value) : this(value, null, null) {
        }

        /// <remarks/>
        [XmlAttribute("aspect")]
        public string Aspect { get; set; }

        /// <remarks/>
        [XmlAttribute("preview", DataType = "anyURI")]
        public string Preview { get; set; }

        /// <remarks/>
        [XmlText(DataType = "anyURI")]
        public string URL { get; set; }

        public string SerializeToString() {
            StreamReader streamReader = null;
            MemoryStream memoryStream = null;
            try {
                memoryStream = new MemoryStream();

                XmlSerializer xs = new XmlSerializer(typeof(XbmcXmlThumb));
                xs.Serialize(memoryStream, this);

                memoryStream.Seek(0, SeekOrigin.Begin);

                streamReader = new StreamReader(memoryStream);
                return streamReader.ReadToEnd();
            }
            finally {
                if (streamReader != null) {
                    streamReader.Dispose();
                }
                if (memoryStream != null) {
                    memoryStream.Dispose();
                }
            }
        }

    }
}