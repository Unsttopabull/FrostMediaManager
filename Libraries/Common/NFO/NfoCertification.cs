using Frost.Common.Models.Provider;

namespace Frost.Common.NFO {

    /// <summary>Holds movie certifications information to be serialized</summary>
    public class NfoCertification : CertificationBase {

        /// <summary>Initializes a new instance of the <see cref="NfoCertification"/> class.</summary>
        public NfoCertification() {
        }

        /// <summary>Initializes a new instance of the <see cref="NfoCertification"/> class.</summary>
        /// <param name="country">The coutry this certification applies to.</param>
        /// <param name="rating">The rating in the specified country.</param>
        public NfoCertification(string country, string rating) {
            Country = country;
            Rating = rating;
        }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        public string Country { get; set; }

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        public string Rating { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Country + COUNTRY_RATING_SEPARATOR + Rating;
        }

        /// <summary>Parses the certifications string and returns certifications as an array of <see cref="NfoCertification"/> instances.</summary>
        /// <param name="certStr">The certification string to parse.</param>
        /// <returns>An array of <see cref="NfoCertification"/> instances parsed from the certifications string</returns>
        public static NfoCertification[] ParseCertificationsString(string certStr) {
            return ParseCertificationsString<NfoCertification>(certStr);
        }

        /// <summary>Gets an instance of <see cref="NfoCertification"/> from the Country name and its rating</summary>
        /// <param name="country">The country name.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>An instance of <see cref="NfoCertification"/> from the Country name and its rating</returns>
        protected override T FromCountyRating<T>(string country, string rating) {
            return new NfoCertification(country, rating) as T;
        }
    }

}
