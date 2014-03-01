namespace Frost.Models.Frost.DB.Arts {

    public class Art : ArtBase {

        public Art() {
        }

        /// <summary>Initializes a new instance of the <see cref="Art"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        public Art(string path, string preview) : base(path, preview) {
        }

        /// <summary>Initializes a new instance of the <see cref="Art"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Art(string path) : base(path, null) {
        }

    }

}