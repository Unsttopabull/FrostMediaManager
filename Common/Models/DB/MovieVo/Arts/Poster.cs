using System;

namespace Frost.Common.Models.DB.MovieVo.Arts {

    /// <summary>Represents a movie poster image.</summary>
    public class Poster : Art, IEquatable<Poster> {

        /// <summary>Initializes a new instance of the <see cref="Poster"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Poster(string path) : base(path, ArtType.Poster) {
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Poster other) {
            return base.Equals(other);
        }

    }

}
