using Frost.InfoParsers.Models;

namespace Frost.InfoParsers {
    public class ParsedArt : IParsedArt {

        /// <summary>Initializes a new instance of the <see cref="ParsedArt"/> class.</summary>
        public ParsedArt(string preview, string fullPath) {
            Preview = preview;
            FullPath = fullPath;
        }

        /// <summary>Initializes a new instance of the <see cref="ParsedArt"/> class.</summary>
        public ParsedArt(ParsedArtType type, string preview, string fullPath) {
            Type = type;
            Preview = preview;
            FullPath = fullPath;
        }

        public string Preview { get; private set; }
        public string FullPath { get; private set; }

        public ParsedArtType Type { get; private set; }
    }
}
