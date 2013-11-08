using System.Linq;

namespace Common.Models {

    /// <summary>Represents a base certification class of which information can be parsed from a certification string.</summary>
    public abstract class CertificationBase {

        /// <summary>Separator between country and its rating.</summary>
        protected const char COUNTRY_RATING_SEPARATOR = ':';

        /// <summary>Separator between individual ratings.</summary>
        protected const string CERTIFICATIONS_SEPARATOR = " / ";

        /// <summary>Parses the certifications string and returns certifications as an array of <c>T</c> instances.</summary>
        /// <param name="certStr">The certification string to parse.</param>
        /// <returns>An array of <c>T</c> instances parsed from the certifications string</returns>
        protected static T[] ParseCertificationsString<T>(string certStr) where T : CertificationBase, new() {
            if (string.IsNullOrEmpty(certStr)) {
                return null;
            }

            T certObj = new T();
            //Split on separator " / " to get individual certifications
            return certStr.SplitWithoutEmptyEntries(CERTIFICATIONS_SEPARATOR)
                //For each certification
                .Select(cert => {
                    //Split on ":" to separate country and the the rating
                    string[] kvp = cert.SplitWithoutEmptyEntries(COUNTRY_RATING_SEPARATOR);

                    //add new certification with
                    // kvp[0] => country
                    // kvp[1] => rating

                    //or if the split resulted in less than 2 entries
                    //we discard it
                    return kvp.Length < 2
                        ? null
                        : certObj.FromCountyRating<T>(kvp[0], kvp[1]);
                })
                .Where(cert => cert != null)
                .ToArray();
        }

        /// <summary>Gets an instance of Certification from the Country name and its rating</summary>
        /// <param name="country">The country name.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>An instance of Certification from the Country name and its rating</returns>
        protected abstract T FromCountyRating<T>(string country, string rating) where T : CertificationBase;

    }

}
