namespace Frost.Models.Frost.DB.Arts {

    /// <summary>Represents a movie poster image.</summary>
    public class Poster : ArtBase {

        public Poster() {
        }

        /// <summary>Initializes a new instance of the <see cref="Poster"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        public Poster(string path, string preview) : base(path, preview) {
        }

        /// <summary>Initializes a new instance of the <see cref="Poster"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Poster(string path) : base(path, null) {
        }

    }

}
