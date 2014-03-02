
namespace Frost.Common.Util.ISO {

    /// <summary>Represents an information about ISO 3166-1 country codes and english name of a country.</summary>
    public class ISOCountryCode : ISOCode {

        /// <summary>Initializes a new instance of the <see cref="ISOCountryCode" /> class.</summary>
        /// <param name="englishName">The ISO 3166-1 english country name.</param>
        /// <param name="alpha2">The ISO 3166-1 Two letter code.</param>
        /// <param name="alpha3">The ISO 3166-1 Three letter code.</param>
        /// <param name="numeric">The ISO 3166-1 Numeric code</param>
        public ISOCountryCode(string englishName, string alpha2, string alpha3, int numeric) : base(englishName, alpha2, alpha3) {
            Numeric = numeric;
        }

        /// <summary>Gets the ISO 3166-1 numeric code.</summary>
        /// <value>The ISO 3166-1 numeric code.</value>
        public int Numeric { get; private set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0}, ({1})", base.ToString(), Numeric);
        }

    }

}