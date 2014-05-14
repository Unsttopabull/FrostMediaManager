using System.IO;
using Frost.Common.Models.Provider;

namespace Frost.RibbonUI.Design.Models {
    public class DesignStudio : IStudio {

        public DesignStudio() {
        }

        public DesignStudio(string filePath) {
            Name = Path.GetFileNameWithoutExtension(filePath);
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
