using Frost.InfoParsers.Models;

namespace Frost.InfoParsers {
    public class ParsedArt : IParsedArt {

        /// <summary>Initializes a new instance of the <see cref="ParsedArt"/> class.</summary>
        public ParsedArt(string preview, string fullUrl) {
            Preview = preview;
            FullUrl = fullUrl;
        }

        /// <summary>Initializes a new instance of the <see cref="ParsedArt"/> class.</summary>
        public ParsedArt(string type, string preview, string fullUrl) {
            Type = type;
            Preview = preview;
            FullUrl = fullUrl;
        }

        public string Preview { get; private set; }
        public string FullUrl { get; private set; }

        public string Type { get; private set; }
    }
}
