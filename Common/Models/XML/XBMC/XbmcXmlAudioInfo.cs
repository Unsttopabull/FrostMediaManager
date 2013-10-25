using System;
using System.Xml.Schema;
using System.Xml.Serialization;
using Common.Models.DB.MovieVo;

namespace Common.Models.XML.XBMC {
    /// <remarks/>
    [Serializable]
    public class XbmcXmlAudioInfo {

        public XbmcXmlAudioInfo() {
        }

        public XbmcXmlAudioInfo(string codec,string channels, string language, string longLanguage ) {
            Codec = codec;
            Language = language;
            LongLanguage = longLanguage;
            Channels = channels;
        }

        public XbmcXmlAudioInfo(string codec, string channels, string language) : this(codec, channels, language, null) {
        }

        /// <remarks/>
        [XmlElement("codec", Form = XmlSchemaForm.Unqualified)]
        public string Codec { get; set; }

        /// <remarks/>
        [XmlElement("language", Form = XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        /// <remarks/>
        [XmlElement("longlanguage", Form = XmlSchemaForm.Unqualified)]
        public string LongLanguage { get; set; }

        /// <remarks/>
        [XmlElement("channels", Form = XmlSchemaForm.Unqualified)]
        public string Channels { get; set; }

        public static explicit operator Audio(XbmcXmlAudioInfo audio) {
            return new Audio(audio.Codec, audio.Channels, audio.Language);
        }
    }
}