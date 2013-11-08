namespace Common.Models.DB.MovieVo.Arts {

    /// <summary>Represents a movie cover image.</summary>
    public class Cover : Art {

        /// <summary>Initializes a new instance of the <see cref="Cover"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Cover(string path) : base(path, ArtType.Cover) {
        }
    }
}