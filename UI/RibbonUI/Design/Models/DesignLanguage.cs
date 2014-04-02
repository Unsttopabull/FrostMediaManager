using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;

namespace RibbonUI.Design.Models {

    public class DesignLanguage : ILanguage {

        public long Id { get; private set; }

        /// <summary>Gets or sets the name of this language.</summary>
        /// <value>The name of this language.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the ISO639 language codes.</summary>
        /// <value>The ISO639 language codes.</value>
        public ISO639 ISO639 { get; set; }

        public bool this[string propertyName] {
            get {
                if (propertyName == "Name" || propertyName == "Id" || propertyName == "ISO639") {
                    return true;
                }
                return false;
            }
        }
    }
}
