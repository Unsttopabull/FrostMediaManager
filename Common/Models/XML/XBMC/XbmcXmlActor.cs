using System;
using System.Xml.Schema;
using System.Xml.Serialization;
using Common.Models.DB.MovieVo;
using Common.Models.DB.MovieVo.People;

namespace Common.Models.XML.XBMC {
    /// <remarks/>
    [Serializable]
    public class XbmcXmlActor{
        public XbmcXmlActor() {
        }

        public XbmcXmlActor(string name, string role, string thumb) {
            Name = name;
            Role = role;
            Thumb = thumb;
        }

        public XbmcXmlActor(string name, string role) : this(name, role, null){
        }

        /// <remarks/>
        [XmlElement("name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        /// <remarks/>
        [XmlElement("role", Form = XmlSchemaForm.Unqualified)]
        public string Role { get; set; }

        /// <remarks/>
        [XmlElement("thumb", Form = XmlSchemaForm.Unqualified, DataType = "anyURI")]
        public string Thumb { get; set; }

        public static explicit operator Actor(XbmcXmlActor act) {
            return new Actor(
                string.IsNullOrEmpty(act.Name) ? null : act.Name,
                act.Thumb,
                string.IsNullOrEmpty(act.Role) ? null : act.Role
            );
        }
    }
}