using Frost.Common.Models.Provider;
using Frost.InfoParsers.Models.Info;

namespace Frost.RibbonUI.Design.Models {
    class DesignAward : IAward {

        public DesignAward() {
            
        }
        public DesignAward(IParsedAward award) {
            AwardType = award.Award;
            Organization = award.Organization;
            IsNomination = award.IsNomination;
        }

        public long Id { get; private set; }

        public string AwardType { get; set; }
        public bool IsNomination { get; set; }
        public string Organization { get; set; }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "AwardType":
                    case "IsNomination":
                    case "Organization":
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}
