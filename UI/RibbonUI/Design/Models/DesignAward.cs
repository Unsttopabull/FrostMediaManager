using Frost.Common.Models.Provider;

namespace RibbonUI.Design.Models {
    class DesignAward : IAward {

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
