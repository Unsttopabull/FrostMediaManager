using System;
using System.Collections.Generic;
using Frost.Common.Models.Provider;

namespace Frost.Common.Comparers {

    /// <summary>Compares two <see cref="IHasName"/> instances for semantical/value equality</summary>
    public class HasNameEqualityComparer : IEqualityComparer<IHasName> {

        /// <summary>Determines whether the specified objects are equal.</summary>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        /// <param name="x">The first object of type IGenre to compare.</param>
        /// <param name="y">The second object of type IGenre to compare.</param>
        public bool Equals(IHasName x, IHasName y) {
            if (x == null || y == null) {
                return false;
            }

            return string.Equals(x.Name, y.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>Returns a hash code for the specified object.</summary>
        /// <returns>A hash code for the specified object.</returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(IHasName obj) {
            return (obj.Name != null ? obj.Name.GetHashCode() : 0);
        }
    }

}