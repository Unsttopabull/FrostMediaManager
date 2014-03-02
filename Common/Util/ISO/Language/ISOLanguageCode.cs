
namespace Frost.Common.Util.ISO {

    /// <summary>Represents an information about ISO 639 language codes and its english name.</summary>
    public class ISOLanguageCode : ISOCode {

        /// <summary>Initializes a new instance of the <see cref="ISOCountryCode" /> class.</summary>
        /// <param name="englishName">The ISO 3166-1 english language name.</param>
        /// <param name="alpha2">The ISO 639-1 Two letter code.</param>
        /// <param name="alpha3B">The ISO 639-2 Three letter code (Bibliographic).</param>
        /// <param name="alpha3T">The ISO 639-2 Three letter code (Terminology).</param>
        public ISOLanguageCode(string englishName, string alpha2, string alpha3B, string alpha3T = null) : base(englishName, alpha2, alpha3B) {
            Alpha3Terminology = alpha3T;

            //Languages = englishName.SplitWithoutEmptyEntries(',');
            Languages = new[] { englishName };
        }

        /// <summary>Initializes a new instance of the <see cref="ISOCountryCode" /> class.</summary>
        /// <param name="alpha2">The ISO 639-1 Two letter code.</param>
        /// <param name="alpha3B">The ISO 639-2 Three letter code (Bibliographic).</param>
        /// <param name="alpha3T">The ISO 639-2 Three letter code (Terminology).</param>
        /// <param name="languageNames">The ISO 3166-1 language names in various languages or variants.</param>
        public ISOLanguageCode(string alpha2, string alpha3B, string alpha3T, params string[] languageNames) : base(languageNames.Length != 0 ? languageNames[0] : null, alpha2, alpha3B) {
            Alpha3Terminology = alpha3T;
            Languages = languageNames;
        }

        /// <summary>Gets the ISO 3 letter code (Terminology).</summary>
        /// <value>The ISO 3 letter code (Terminology).</value>
        public string Alpha3Terminology { get; private set; }

        /// <summary>Gets all the languages represented by this code in english.</summary>
        /// <value>All the languages represented by this code in english.</value>
        public string[] Languages { get; private set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            if (string.IsNullOrEmpty(Alpha3Terminology)) {
                return base.ToString();
            }
            return base.ToString() + ", " + Alpha3Terminology;
        }

    }

}