using System.Text;
using System.Xml;

namespace Common {

    /// <summary>Represents an xml writer of which tags are indented.</summary>
    public class XmlIndentedTextWriter : XmlTextWriter {

        /// <summary>Initializes a new instance of the <see cref="XmlIndentedTextWriter"/> class.</summary>
        /// <param name="filename">The filename to write to. If the file exists, it truncates it and overwrites it with the new content.</param>
        /// <param name="encoding">The encoding to generate. If encoding is null it writes the file out as UTF-8, and omits the encoding attribute from the ProcessingInstruction.</param>
        public XmlIndentedTextWriter(string filename, Encoding encoding = null) : base(filename, encoding) {
            Formatting = Formatting.Indented;
        }

    }

}
