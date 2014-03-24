using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace RibbonUI.Design.Classes {

    public class DesignSet : IMovieSet {

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
