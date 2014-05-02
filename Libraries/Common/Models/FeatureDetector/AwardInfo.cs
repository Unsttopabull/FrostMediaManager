namespace Frost.Common.Models.FeatureDetector {

    public class AwardInfo {

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public AwardInfo(string award, string organization, bool isNomination) {
            Organization = organization;
            IsNomination = isNomination;
            Award = award;
        }

        public string Organization { get; set; }
        public bool IsNomination { get; set; }
        public string Award { get; set; }
    }

}