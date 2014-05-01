using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.InfoParsers.Models;

namespace RibbonUI.Design.Models {
    public class DesignArt : IArt {

        public DesignArt(ArtType type, string path) {
            Type = type;
            Path = path;
        }

        public DesignArt(IParsedArt parsedArt) {
            switch (parsedArt.Type) {
                case ParsedArtType.Cover:
                    Type = ArtType.Cover;
                    break;
                case ParsedArtType.Poster:
                    Type = ArtType.Poster;
                    break;
                case ParsedArtType.Fanart:
                    Type = ArtType.Fanart;
                    break;
                default:
                    Type = ArtType.Unknown;
                    break;
            }

            Path = parsedArt.FullPath;
            Preview = parsedArt.Preview;
        }

        /// <summary>Unique identifier.</summary>
        public long Id { get; private set; }

        /// <summary>Gets the value whether the property is editable.</summary>
        /// <value>The <see cref="System.Boolean"/> if the value is editable.</value>
        /// <param name="propertyName">Name of the property to check.</param>
        /// <returns>Returns <c>true</c> if property is editable, otherwise <c>false</c> (Not implemented or read-only).</returns>
        public bool this[string propertyName] {
            get { return true; }
        }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        public string Path { get; set; }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        public string Preview { get; set; }
        public ArtType Type { get; private set; }
        public string PreviewOrPath { get; private set; }
    }
}
