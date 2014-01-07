using System;

namespace Frost.Common.Models.DB.MovieVo.Arts {

    /// <summary>Represents a movie cover image.</summary>
    public class Cover : Art, IEquatable<Cover> {

        public Cover() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="Cover"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        public Cover(string path, string preview) : base(path, preview, ArtType.Cover) {
        }

        /// <summary>Initializes a new instance of the <see cref="Cover"/> class.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Cover(string path) : base(path, null, ArtType.Cover) {
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Cover other) {
            return base.Equals(other);
        }

    }

}
