using Frost.Common.Util.ISO;

namespace Frost.Common.Models.FeatureDetector {

    /// <summary>Represents an informationn about a movie certification that has been detected by Feature Detecor.</summary>
    public class CertificationInfo {

        /// <summary>Initializes a new instance of the <see cref="CertificationInfo"/> class.</summary>
        /// <param name="country">The country in which this certification applies.</param>
        /// <param name="certification">The certification rating.</param>
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