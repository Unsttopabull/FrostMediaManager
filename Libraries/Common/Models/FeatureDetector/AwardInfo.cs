namespace Frost.Common.Models.FeatureDetector {

    /// <summary>Represents the information about a movie award that has been detected by Feature Detector.</summary>
    public class AwardInfo {

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public AwardInfo(string award, string organization, bool isNomination) {
            Organization = organization;
            IsNomination = isNomination;
            Award = award;
        }

        /// <summary>Gets or sets the organization that awards this award (e.g Oscars).</summary>
        /// <value>The organization that awards this award (e.g Oscars)</value>
        public string Organization { get; set; }

        /// <summary>Gets or sets a value indicating whether this award is a nomination.</summary>
        /// <value><c>true</c> if this award is a nomination only; otherwise, <c>false</c>.</value>
        public bool IsNomination { get; set; }

        /// <summary>Gets or sets the award name or detail (eg. Best leading male role).</summary>
        /// <value>The award name or detail (eg. Best leading male role).</value>
        public string Award { get; set; }
    }

}