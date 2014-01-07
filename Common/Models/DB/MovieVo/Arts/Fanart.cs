using System;

namespace Frost.Common.Models.DB.MovieVo.Arts {

    /// <summary>Represents a movie fanart image (backround / backdrop).</summary>
    public class Fanart : Art, IEquatable<Fanart> {

        public Fanart() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="Fanart"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        public Fanart(string path, string preview) : base(path, preview, ArtType.Fanart) {
        }

        /// <summary>Initializes a new instance of the <see cref="Fanart"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Fanart(string path) : base(path, null, ArtType.Fanart) {
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Fanart other) {
            return base.Equals(other);
        }

    }

}
