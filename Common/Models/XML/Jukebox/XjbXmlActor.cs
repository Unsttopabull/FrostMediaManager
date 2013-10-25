using System;
using System.Xml.Serialization;
using Common.Models.XML.XBMC;

namespace Common.Models.XML.Jukebox {

    /// <remarks/>
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot("actor", Namespace = "", IsNullable = false)]
    public class XjbXmlActor : XbmcXmlActor{
        public XjbXmlActor() {
            Role = "";
            Thumb = "";
        }

        public XjbXmlActor(string name, string role, string thumb)
            : base(name, role, thumb ?? ""){
        }

        public XjbXmlActor(string name, string role) : this(name, role, "") {
        }
    }
}