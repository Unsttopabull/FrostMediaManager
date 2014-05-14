using Frost.Common.Models.Provider;

namespace Frost.RibbonUI.Design.Models {
    public class DesignGenre : IGenre {

        public DesignGenre() {
        }

        public DesignGenre(string name) {
            Name = name;
        }

        public string Name { get; set; }
        public long Id { get; private set; }

        public bool this[string propertyName] {
            get {
                if (propertyName == "Name" || propertyName == "Id") {
                    return true;
                }
                return false;
            }
        }
    }
}
