using System;
using System.Xml.Schema;
using System.Xml.Serialization;
using Common.Models.DB.MovieVo;

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


        public static explicit operator Subtitle(XbmcXmlSubtitleInfo subtitle) {
            return new Subtitle(subtitle.LongLanguage ?? subtitle.Language);
        }
    }
}