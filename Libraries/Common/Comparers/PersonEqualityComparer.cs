using System;
using System.Collections.Generic;
using Frost.Common.Models.Provider;

namespace Frost.Common.Comparers {

    /// <summary>Compares two <see cref="IPerson"/> instances for semantical/value equality.</summary>
    public class PersonEqualityComparer : IEqualityComparer<IPerson> {

        /// <summary>Determines whether the specified objects are equal.</summary>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        /// <param name="lhs">The first object of type IGenre to compare.</param>
        /// <param name="rhs">The second object of type IGenre to compare.</param>
        public bool Equals(IPerson lhs, IPerson rhs) {
            if (ReferenceEquals(lhs, rhs)) {
                return true;
            }

            if (lhs == null || rhs == null) {
                return false;
            }

            if (lhs.Id > 0 && rhs.Id > 0) {
                return lhs.Id == rhs.Id;
            }

            if (!string.IsNullOrEmpty(lhs.ImdbID) && !string.IsNullOrEmpty(rhs.ImdbID)) {
                return string.Equals(lhs.ImdbID, rhs.ImdbID, StringComparison.OrdinalIgnoreCase);
            }

            return string.Equals(lhs.Name, rhs.Name);
        }

        /// <summary>Returns a hash code for the specified object.</summary>
        /// <returns>A hash code for the specified object.</returns>
        /// <param name="person">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="person"/> is a reference type and <paramref name="person"/> is null.</exception>
        public int GetHashCode(IPerson person) {
            if (person == null) {
                return 0;
            }

            return person.Name != null
                ? person.Name.GetHashCode()
                : 0;
        }
    }
}
