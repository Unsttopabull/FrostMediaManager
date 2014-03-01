namespace Frost.Models.Frost.DB.Arts {

    /// <summary>Represents a movie fanart image (backround / backdrop).</summary>
    public class Fanart : ArtBase {

        public Fanart() {
        }

        /// <summary>Initializes a new instance of the <see cref="Fanart"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        public Fanart(string path, string preview) : base(path, preview) {
        }

        /// <summary>Initializes a new instance of the <see cref="Fanart"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Fanart(string path) : base(path, null) {
        }

    }

}
