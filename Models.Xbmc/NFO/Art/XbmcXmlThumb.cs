using System;
using System.IO;
using System.Xml.Serialization;

namespace Frost.Model.Xbmc.NFO {

    /// <summary>Represent a promotional image of the movie ready to be serialized.</summary>
    [Serializable]
    public class XbmcXmlThumb {

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlThumb"/> class.</summary>
        public XbmcXmlThumb() {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlThumb"/> class.</summary>
        /// <param name="path">The path to this image (can be local or network or an URI).</param>
        /// <param name="aspect">The type of the image used as a discriminator.</param>
        /// <param name="preview">The path to the preview of the art (a smaller, lower resolution copy).</param>
        public XbmcXmlThumb(string path, string aspect = null, string preview = null) {
            Aspect = aspect;
            Preview = preview;
            Path = path;
        }


        /// <summary>Gets or sets the type of the image used as a discriminator.</summary>
        /// <value>The type of the image used as a discriminator.</value>
        [XmlAttribute("aspect")]
        public string Aspect { get; set; }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        [XmlAttribute("preview", DataType = "anyURI")]
        public string Preview { get; set; }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this image (can be local or network or an URI).</value>
        [XmlText(DataType = "anyURI")]
        public string Path { get; set; }

        /// <summary>Serializes the current instance to xml string.</summary>
        /// <returns>The current instance serialized to an xml string.</returns>
        public string SerializeToString() {
            using (MemoryStream memoryStream = new MemoryStream()) {
                XmlSerializer xs = new XmlSerializer(typeof(XbmcXmlThumb));
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
