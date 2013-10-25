using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.Models.XML.XBMC {
    /// <remarks/>
    [Serializable]
    public class XbmcXmlSubtitleInfo {

        public XbmcXmlSubtitleInfo() {
        }

        public XbmcXmlSubtitleInfo(string language) {
            Language = language;
        }

        public XbmcXmlSubtitleInfo(string language, string longLanguage) : this(language){
            LongLanguage = longLanguage;
        }

        /// <remarks/>
        [XmlElement("language", Form = XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        /// <remarks/>
        [XmlElement("longlanguage", Form = XmlSchemaForm.Unqualified)]
        public string LongLanguage { get; set; }
    }
}