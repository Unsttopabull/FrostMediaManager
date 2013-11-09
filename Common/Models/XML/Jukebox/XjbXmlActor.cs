using System;
using System.Xml.Serialization;
using Common.Models.XML.XBMC;

namespace Common.Models.XML.Jukebox {

    /// <summary>Represents an actor in a movie.</summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot("actor", Namespace = "", IsNullable = false)]
    public class XjbXmlActor : XbmcXmlActor {

        /// <summary>Initializes a new instance of the <see cref="XjbXmlActor"/> class. with <b>Role</b> and <b>Thumb</b> set to empty string</summary>
        public XjbXmlActor() {
            Role = "";
            Thumb = "";
        }

        /// <summary>Initializes a new instance of the <see cref="XjbXmlActor"/> class.</summary>
        /// <param name="name">The actors full name.</param>
        /// <param name="role">The role or character the actor is portraying.</param>
        /// <param name="thumb">The actors thumbnail (small picture)</param>
        public XjbXmlActor(string name, string role, string thumb = "") : base(name, role, thumb ?? "") {
        }

    }

}
