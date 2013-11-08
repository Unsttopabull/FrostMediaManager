using Common.Models.DB.MovieVo;

namespace Common.Models.XML.XBMC {

    /// <summary>Holds movie certifications information to be serialized</summary>
    public class XbmcXmlCertification : CertificationBase {

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlCertification"/> class.</summary>
        public XbmcXmlCertification() {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlCertification"/> class.</summary>
        /// <param name="country">The coutry this certification applies to.</param>
        /// <param name="rating">The rating in the specified country.</param>
        public XbmcXmlCertification(string country, string rating) {
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

        /// <summary>Parses the certifications string and returns certifications as an array of <see cref="XbmcXmlCertification"/> instances.</summary>
        /// <param name="certStr">The certification string to parse.</param>
        /// <returns>An array of <see cref="XbmcXmlCertification"/> instances parsed from the certifications string</returns>
        public static XbmcXmlCertification[] ParseCertificationsString(string certStr) {
            return ParseCertificationsString<XbmcXmlCertification>(certStr);
        }

        /// <summary>Gets an instance of <see cref="XbmcXmlCertification"/> from the Country name and its rating</summary>
        /// <param name="country">The country name.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>An instance of <see cref="XbmcXmlCertification"/> from the Country name and its rating</returns>
        protected override T FromCountyRating<T>(string country, string rating) {
            return new XbmcXmlCertification(country, rating) as T;
        }

        /// <summary>Converts <see cref="XbmcXmlCertification"/> to an instance of <see cref="Common.Models.DB.MovieVo.Certification">Certification</see></summary>
        /// <param name="cert">The instance of <see cref="XbmcXmlCertification"/> to convert</param>
        /// <returns>An instance of <see cref="Common.Models.DB.MovieVo.Certification">Certification</see> converted from <see cref="XbmcXmlCertification"/></returns>
        public static explicit operator Certification(XbmcXmlCertification cert) {
            return new Certification(cert.Country, cert.Rating);
        }
    }
}