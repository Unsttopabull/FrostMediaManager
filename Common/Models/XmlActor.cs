using System.Xml.Schema;
using System.Xml.Serialization;

namespace Frost.Common.Models {
    public class XmlActor {

        /// <summary>Initializes a new instance of the <see cref="XmlActor"/> class.</summary>
        public XmlActor() {
        }

        /// <summary>Initializes a new instance of the <see cref="XmlActor"/> class.</summary>
        /// <param name="name">The actors full name.</param>
        /// <param name="role">The role or character the actor is portraying.</param>
        /// <param name="thumb">The actors thumbnail (small picture)</param>
        public XmlActor(string name, string role, string thumb = null) {
            Name = name;
            Role = role;
            Thumb = thumb;
        }

        /// <summary>Gets or sets the actors full name.</summary>
        /// <value>The actors full name.</value>
        [XmlElement("name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        /// <summary>Gets or sets the role or character the actor is portraying..</summary>
        /// <value>The role or character the actor is portraying.</value>
        [XmlElement("role", Form = XmlSchemaForm.Unqualified)]
        public string Role { get; set; }

        /// <summary>Gets or sets the actors thumbnail.</summary>
        /// <value>The actors thumbnail (small picture)</value>
        [XmlElement("thumb", Form = XmlSchemaForm.Unqualified, DataType = "anyURI")]
        public string Thumb { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name + " (" + (Role ?? "Unknown") + ")";
        }
    }
}
