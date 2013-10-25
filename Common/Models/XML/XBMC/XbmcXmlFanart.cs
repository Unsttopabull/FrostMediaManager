using System;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.Models.XML.XBMC {
    /// <remarks/>
    [Serializable]
    public class XbmcXmlFanart {

        public XbmcXmlFanart() {
        }

        public XbmcXmlFanart(string url, XbmcXmlThumb[] thumb) {
            Thumbs = thumb;
            URL = url;
        }

        /// <remarks/>
        [XmlElement("thumb", Form = XmlSchemaForm.Unqualified)]
        public XbmcXmlThumb[] Thumbs { get; set; }

        /// <remarks/>
        [XmlAttribute("url", DataType = "anyURI")]
        public string URL { get; set; }

        public string SerializeToXml() {
            StreamReader streamReader = null;
            MemoryStream memoryStream = null;
            try {
                memoryStream = new MemoryStream();

                XmlSerializer xs = new XmlSerializer(typeof(XbmcXmlFanart));
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