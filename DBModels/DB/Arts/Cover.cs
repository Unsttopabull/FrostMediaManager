
namespace Frost.Models.Frost.DB.Arts {

    /// <summary>Represents a movie cover image.</summary>
    public class Cover : ArtBase {

        public Cover() {
        }

        /// <summary>Initializes a new instance of the <see cref="Cover"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        public Cover(string path, string preview) : base(path, preview) {
        }

        /// <summary>Initializes a new instance of the <see cref="Cover"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Cover(string path) : base(path, null) {
        }

    }

}
