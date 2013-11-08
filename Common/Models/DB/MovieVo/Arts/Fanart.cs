namespace Common.Models.DB.MovieVo.Arts {

    /// <summary>Represents a movie fanart image (backround / backdrop).</summary>
    public class Fanart : Art {

        /// <summary>Initializes a new instance of the <see cref="Fanart"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Fanart(string path) : base(path, ArtType.Fanart) {
        }

    }

}
