using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Frost.Model.Xbmc.NFO {

    /// <summary>Represents serialized information about a subtitle in a movie</summary>
    [Serializable]
    public class XbmcXmlSubtitleInfo {

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlSubtitleInfo"/> class.</summary>
        public XbmcXmlSubtitleInfo() {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlSubtitleInfo"/> class.</summary>
        /// <param name="language">The language of this subtitle in a 3 letter abreviation (ISO 639-2 Code).</param>
        public XbmcXmlSubtitleInfo(string language) {
            Language = language;
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlSubtitleInfo"/> class.</summary>
        /// <param name="language">The language of this subtitle in a 3 letter abreviation (ISO 639-2 Code).</param>
        /// <param name="longLanguage">The full name of the language in this subtitle stream</param>
        public XbmcXmlSubtitleInfo(string language, string longLanguage) : this(language) {
            LongLanguage = longLanguage;
        }

        /// <summary>Gets or sets the language of this subtitle in a 3 letter abreviation (ISO 639-2 Code).</summary>
        /// <value>The language of this subtitle in a 3 letter abreviation (ISO 639-2 Code).</value>
        /// <example>\eg{<c>"eng"</c> for English, <c>"spa"</c> for Spanish }</example>
        [XmlElement("language", Form = XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        /// <summary>Gets or sets the full name of the language in this subtitle stream.</summary>
        /// <value>The full name of the language in this subtitle stream</value>
        [XmlElement("longlanguage", Form = XmlSchemaForm.Unqualified)]
        public string LongLanguage { get; set; }

    }

}
