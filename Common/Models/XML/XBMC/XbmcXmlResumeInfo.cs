using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.Models.XML.XBMC {
    /// <remarks/>
    [Serializable]
    public class XbmcXmlResumeInfo {

        public XbmcXmlResumeInfo() {
            
        }

        public XbmcXmlResumeInfo(float position, float total) {
            Position = position;
            Total = total;
        }

        /// <remarks/>
        [XmlElement("position", Form = XmlSchemaForm.Unqualified)]
        public float Position { get; set; }

        /// <remarks/>
        [XmlElement("total", Form = XmlSchemaForm.Unqualified)]
        public float Total { get; set; }
    }
}