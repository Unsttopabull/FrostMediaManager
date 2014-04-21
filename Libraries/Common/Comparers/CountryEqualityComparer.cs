using System;
using System.Collections.Generic;
using Frost.Common.Models.Provider;

namespace Frost.Common.Comparers {

    /// <summary>Compares two <see cref="ICountry"/> instances for semantical/value equality.</summary>
    public class CountryEqualityComparer : IEqualityComparer<ICountry> {

        /// <summary>Determines whether the specified objects are equal.</summary>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        /// <param name="lhs">The first object of type <paramref name="T"/> to compare.</param>
        /// <param name="rhs">The second object of type <paramref name="T"/> to compare.</param>
        public bool Equals(ICountry lhs, ICountry rhs) {
            if (ReferenceEquals(lhs, rhs)) {
                return true;
            }

            if (lhs == null || rhs == null) {
                return false;
            }

            if (lhs.Id > 0 && rhs.Id > 0) {
                return lhs.Id == rhs.Id;
            }

            if (lhs.ISO3166 != null && rhs.ISO3166 != null) {
                return lhs.ISO3166.Equals(rhs.ISO3166);
            }

            return string.Equals(lhs.Name, rhs.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Returns a hash code for the specified object.</summary>
        /// <returns>A hash code for the specified object.</returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(ICountry obj) {
            if (ReferenceEquals(null, obj)) {
                throw new ArgumentNullException("obj");
            }

            unchecked {
                return ((obj.Name != null ? obj.Name.GetHashCode() : 0) * 397) ^ (obj.ISO3166 != null ? obj.ISO3166.GetHashCode() : 0);
            }
        }
    }
}
