using Frost.Common.Util.ISO;

namespace Frost.Common.Models.FeatureDetector {

    public class CertificationInfo {

        public CertificationInfo(ISOCountryCode country, string certification) {
            Country = country;
            Rating = certification;
        }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        public ISOCountryCode Country { get; set; }

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        public string Rating { get; set; }
    }

}