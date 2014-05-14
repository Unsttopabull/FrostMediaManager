using Frost.Common.Models.Provider;

namespace Frost.RibbonUI.Design.Models {
    class DesignSpecial : ISpecial {

        public long Id { get; private set; }

        public string Name { get; set; }

        public bool this[string propertyName] {
            get {
                if (propertyName == "Id" || propertyName == "Name") {
                    return true;
                }
                return false;
            }
        }
    }
}
