namespace Common.Models.DB.MovieVo.Arts {

    /// <summary>Represents a movie poster image.</summary>
    public class Poster : Art {

        /// <summary>Initializes a new instance of the <see cref="Poster"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Poster(string path) : base(path, ArtType.Poster) {
        }

    }

}
