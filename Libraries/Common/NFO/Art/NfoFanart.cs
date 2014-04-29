using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Frost.Common.NFO.Art {

    /// <summary>Holds information about movie's promotional images ready to be serialized.</summary>
    [Serializable]
    public class NfoFanart {

        /// <summary>Initializes a new instance of the <see cref="NfoFanart"/> class.</summary>
        public NfoFanart() {
        }

        /// <summary>Initializes a new instance of the <see cref="NfoFanart"/> class.</summary>
        /// <param name="thumbs">The fanart images.</param>
        /// <param name="url">The URL or path from where the images originate (optional)</param>
        public NfoFanart(IEnumerable<NfoThumb> thumbs, string url = null) : this(new List<NfoThumb>(thumbs), url) {
        }

        /// <summary>Initializes a new instance of the <see cref="NfoFanart"/> class.</summary>
        /// <param name="thumbs">The fanart images.</param>
        /// <param name="url">The URL or path from where the images originate (optional)</param>
        public NfoFanart(List<NfoThumb> thumbs, string url = null) {
            Thumbs = thumbs;
            Url = url;
        }

        /// <summary>Gets or sets the fanart images.</summary>
        /// <value>The fanart images.</value>
        [XmlElement("thumb", Form = XmlSchemaForm.Unqualified)]
        public List<NfoThumb> Thumbs { get; set; }

        /// <summary>Gets or sets the URL or path from where the images originate (optional).</summary>
        /// <value>The URL or path from where the images originate (optional)</value>
        [XmlAttribute("url", DataType = "anyURI")]
        public string Url { get; set; }

        /// <summary>Serializes the current instance to xml string.</summary>
        /// <returns>The current instance serialized to an xml string.</returns>
        public string SerializeToXml() {
            using (MemoryStream memoryStream = new MemoryStream()) {
                XmlSerializer xs = new XmlSerializer(typeof(NfoFanart));
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
