using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Frost.Providers.Xtreamer {

    /// <summary>Represents information about where to resume the movie, ready to be serialized.</summary>
    [Serializable]
    public class XbmcXmlResumeInfo {

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlResumeInfo"/> class.</summary>
        public XbmcXmlResumeInfo() {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlResumeInfo" /> class.</summary>
        /// <param name="position">The position.</param>
        /// <param name="total">The total.</param>
        public XbmcXmlResumeInfo(float position, float total) {
            Position = position;
            Total = total;
        }

        /// <summary>Gets or sets the position.</summary>
        /// <value>The position.</value>
        [XmlElement("position", Form = XmlSchemaForm.Unqualified)]
        public float Position { get; set; }

        /// <summary>Gets or sets the total.</summary>
        /// <value>The total.</value>
        [XmlElement("total", Form = XmlSchemaForm.Unqualified)]
        public float Total { get; set; }

    }

}
