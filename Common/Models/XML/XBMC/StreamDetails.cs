using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.Models.XML.XBMC {

    [Serializable]
    public class StreamDetails {
        /// <remarks/>
        [XmlElement("video", Form = XmlSchemaForm.Unqualified)]
        public XbmcXmlVideoInfo[] Video { get; set; }

        /// <remarks/>
        [XmlElement("audio", Form = XmlSchemaForm.Unqualified)]
        public XbmcXmlAudioInfo[] Audio { get; set; }

        /// <remarks/>
        [XmlElement("subtitle", Form = XmlSchemaForm.Unqualified)]
        public XbmcXmlSubtitleInfo[] Subtitle { get; set; }
    }
}