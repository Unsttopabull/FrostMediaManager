using System;
using System.Xml.Schema;
using System.Xml.Serialization;
using Common.Models.DB.MovieVo;

namespace Common.Models.XML.XBMC {
    /// <remarks/>
    [Serializable]
    public class XbmcXmlVideoInfo {

        public XbmcXmlVideoInfo() {
        }

        public XbmcXmlVideoInfo(string codec, string aspect, uint width, uint height, uint durationInSeconds, string language, string longLanguage) {
            Codec = codec;
            Aspect = aspect;
            Width = width;
            Height = height;
            DurationInSeconds = durationInSeconds;
            Language = language;
            LongLanguage = longLanguage;
        }

        public XbmcXmlVideoInfo(string codec, string aspect, uint width, uint height)
            : this(codec, aspect, width, height, 0, null, null) {
            DurationInSecondsSpecified = false;
        }

        /// <remarks/>
        [XmlElement("codec", Form = XmlSchemaForm.Unqualified)]
        public string Codec { get; set; }

        /// <remarks/>
        [XmlElement("aspect", Form = XmlSchemaForm.Unqualified)]
        public string Aspect { get; set; }

        /// <remarks/>
        [XmlElement("width", Form = XmlSchemaForm.Unqualified)]
        public uint Width { get; set; }

        /// <remarks/>
        [XmlElement("height", Form = XmlSchemaForm.Unqualified)]
        public uint Height { get; set; }

        /// <remarks/>
        [XmlElement("durationinseconds", Form = XmlSchemaForm.Unqualified)]
        public uint DurationInSeconds { get; set; }

        [XmlIgnore]
        public bool DurationInSecondsSpecified { get; set; }

        /// <remarks/>
        [XmlElement("language", Form = XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        /// <remarks/>
        [XmlElement("longlanguage", Form = XmlSchemaForm.Unqualified)]
        public string LongLanguage { get; set; }

        public static explicit operator Video(XbmcXmlVideoInfo video) {
            return new Video(video.Codec, video.Aspect, (int)video.Height, (int)video.Width);
        }
    }
}