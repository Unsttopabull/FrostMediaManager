using System.Text;
using System.Xml;

namespace Common {
    public class XmlIndentedTextWriter : XmlTextWriter {

        public XmlIndentedTextWriter(string filename, Encoding encoding) : base(filename, encoding) {
            Formatting = Formatting.Indented;
        }

        public XmlIndentedTextWriter(string filename) : this(filename, new UTF8Encoding()) {
        }
    }
}