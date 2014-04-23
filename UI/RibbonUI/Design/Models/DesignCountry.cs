using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;

namespace RibbonUI.Design.Models {
    public class DesignCountry : ICountry {

        public DesignCountry() {
        }

        public DesignCountry(string name) {
            Name = name;

            ISO3166 = new ISO3166(Name);
        }

        public long Id { get; private set; }

        /// <summary>Gets or sets the country name.</summary>
        /// <value>The name of the country.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the ISO 3166-1 Information.</summary>
        /// <value>The ISO 3166-1 Information.</value>
        public ISO3166 ISO3166 { get; set; }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Name":
                    case "ISO3166":
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}
