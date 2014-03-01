using Frost.Common.Models;
using Frost.Common.Util.ISO;

namespace Frost.Models.Xtreamer {
    public class XjbCertification : CertificationBase {

        public XjbCertification() {
            
        }

        public XjbCertification(ISOCountryCode country, string rating) {
            Country = country;
            Rating = rating;
        }

        public ISOCountryCode Country { get; set; }

        public string Rating { get; set; }


        public static XjbCertification[] ParseCertificationsString(string certString) {
            return ParseCertificationsString<XjbCertification>(certString);
        }

        /// <summary>Gets an instance of Certification from the Country name and its rating</summary>
        /// <param name="country">The country name.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>An instance of Certification from the Country name and its rating</returns>
        protected override T FromCountyRating<T>(string country, string rating) {
            ISOCountryCode iso = ISOCountryCodes.Instance.GetByEnglishName(country);
            if (iso == null) {
                return null;
            }

            return new XjbCertification(iso, rating) as T;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Country.Alpha3 + COUNTRY_RATING_SEPARATOR + Rating;
        }
    }
}
