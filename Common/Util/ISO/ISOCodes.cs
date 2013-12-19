using System;
using System.Collections.Generic;
using System.Linq;

namespace Frost.Common.Util.ISO {

    /// <summary>Represents a mapping between english language names and their officialy assigned ISO codes.</summary>
    public abstract class ISOCodes<T> where T : ISOCode {

        protected static readonly Dictionary<string, T> Codes = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Gets all known ISO codes.</summary>
        /// <returns>An information about all known ISO codes.</returns>
        public static IEnumerable<T> GetAllKnownCodes() {
            return Codes.Values;
        }

        /// <summary>Gets the ISO code with specified english name.</summary>
        /// <param name="name">The english name.</param>
        /// <returns>An instance of <see cref="ISOCode"/> if found; otherwise <c>null</c>.</returns>
        public virtual T GetByEnglishName(string name) {
            return Codes.FirstOrDefault(dict => dict.Value.EnglishName == name).Value;
        }

        /// <summary>Gets the ISO codes with specified ISO 2 or 3 letter code.</summary>
        /// <param name="isoCode">The 2 or 3 letter ISO code.</param>
        /// <returns>An instance of <see cref="ISOCode"/> if found; otherwise <c>null</c>.</returns>
        public T GetByISOCode(string isoCode) {
            return isoCode != null && Codes.ContainsKey(isoCode)
                ? Codes[isoCode]
                : null;
        }

        /// <summary>Determines whether the specified string is an ISO code.</summary>
        /// <param name="isoCode">The string to check.</param>
        /// <returns>Returns <c>true</c> if a valid ISO code; otherwise <c>false</c>.</returns>
        public bool IsAnISOCode(string isoCode) {
            return Codes.ContainsKey(isoCode);
        }

        public T this[string isoCode] { get { return GetByISOCode(isoCode); } }
    }

}