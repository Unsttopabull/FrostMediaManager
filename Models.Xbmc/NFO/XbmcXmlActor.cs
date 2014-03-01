using System;
using Frost.Common.Models;

namespace Frost.Model.Xbmc.NFO {

    /// <summary>Represents a serialized actor in a movie</summary>
    [Serializable]
    public class XbmcXmlActor : XmlActor {

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlActor"/> class.</summary>
        public XbmcXmlActor() {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlActor"/> class.</summary>
        /// <param name="name">The actors full name.</param>
        /// <param name="role">The role or character the actor is portraying.</param>
        /// <param name="thumb">The actors thumbnail (small picture)</param>
        public XbmcXmlActor(string name, string role, string thumb = null) : base(name, role, thumb){
        }

        ///// <summary>Converts <see cref="XbmcXmlActor"/> to an instance of <see cref="Actor"/></summary>
        ///// <param name="act">The instance of <see cref="XbmcXmlActor"/> to convert</param>
        ///// <returns>An instance of <see cref="Actor"/> converted from <see cref="XbmcXmlActor"/></returns>
        //public static explicit operator Actor(XbmcXmlActor act) {
        //    return new Actor(
        //        string.IsNullOrEmpty(act.Name) ? null : act.Name,
        //        act.Thumb,
        //        string.IsNullOrEmpty(act.Role) ? null : act.Role
        //    );
        //}

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name + " (" + (Role ?? "Unknown") + ")";
        }
    }
}