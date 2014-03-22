using Frost.Common.Models;

namespace RibbonUI.Design.Models {
    class DesignCertification : ICertification {
        public long Id { get; private set; }

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        public string Rating { get; set; }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        public ICountry Country { get; set; }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Rating":
                    case "Country":
                        return true;
                    default:
                        return false;
                }
            }
        }

    }
}
