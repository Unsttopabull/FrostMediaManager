
namespace Frost.Common.Util.ISO {

    /// <summary>A base class for information about ISO codes.</summary>
    public class ISOCode {

        public ISOCode() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="ISOCode" /> class.</summary>
        /// <param name="englishName">The ISO english language name.</param>
        /// <param name="alpha2">The ISO Two letter code.</param>
        /// <param name="alpha3">The ISO Three letter code.</param>
        public ISOCode(string englishName, string alpha2, string alpha3) {
            Alpha2 = alpha2;
            Alpha3 = alpha3;
            EnglishName = englishName;
        }

        /// <summary>Gets the ISO 2 letter code.</summary>
        /// <value>The ISO 2 letter code.</value>
        public string Alpha2 { get; private set; }

        /// <summary>Gets the ISO 3 letter code.</summary>
        /// <value>The ISO 3 letter code.</value>
        public string Alpha3 { get; private set; }

        /// <summary>Gets the ISO english language name.</summary>
        /// <value>The ISO english language name.</value>
        public string EnglishName { get; private set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format(@"{0}, {1}, {2}", EnglishName, Alpha3, Alpha2);
        }

    }

}