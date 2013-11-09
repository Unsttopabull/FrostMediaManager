using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Frost.Common.Models.XML.XBMC {

    /// <summary>Holds information about movie's promotional images ready to be serialized.</summary>
    [Serializable]
    public class XbmcXmlFanart {

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlFanart"/> class.</summary>
        public XbmcXmlFanart() {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlFanart"/> class.</summary>
        /// <param name="thumbs">The fanart images.</param>
        /// <param name="url">The URL or path from where the images originate (optional)</param>
        public XbmcXmlFanart(IEnumerable<XbmcXmlThumb> thumbs, string url = null) : this(new List<XbmcXmlThumb>(thumbs), url) {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlFanart"/> class.</summary>
        /// <param name="thumbs">The fanart images.</param>
        /// <param name="url">The URL or path from where the images originate (optional)</param>
        public XbmcXmlFanart(List<XbmcXmlThumb> thumbs, string url = null) {
            Thumbs = thumbs;
            URL = url;
        }

        /// <summary>Gets or sets the fanart images.</summary>
        /// <value>The fanart images.</value>
        [XmlElement("thumb", Form = XmlSchemaForm.Unqualified)]
        public List<XbmcXmlThumb> Thumbs { get; set; }

        /// <summary>Gets or sets the URL or path from where the images originate (optional).</summary>
        /// <value>The URL or path from where the images originate (optional)</value>
        [XmlAttribute("url", DataType = "anyURI")]
        public string URL { get; set; }

        /// <summary>Serializes the current instance to xml string.</summary>
        /// <returns>The current instance serialized to an xml string.</returns>
        public string SerializeToXml() {
            using (MemoryStream memoryStream = new MemoryStream()) {
                XmlSerializer xs = new XmlSerializer(typeof(XbmcXmlFanart));
                xs.Serialize(memoryStream, this);

                //seek the stream to the begining
                memoryStream.Seek(0, SeekOrigin.Begin);

                //return the stream as text in a single string
                using (StreamReader streamReader = new StreamReader(memoryStream)) {
                    return streamReader.ReadToEnd();
                }
            }
        }

    }

}
