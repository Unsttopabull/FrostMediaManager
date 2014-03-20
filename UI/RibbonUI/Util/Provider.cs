using System.IO;

namespace RibbonUI.Util {

    public class Provider {

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public Provider(string name, string iconPath) {
            Name = name;
            IconPath = iconPath;
        }

        public string Name { get; set; }
        public string IconPath { get; set; }

        public string AssemblyPath { get; set; }

        public string IconImage {
            get {
                string path = Directory.GetCurrentDirectory() + "\\plugins\\" + IconPath;
                if (File.Exists(path)) {
                    return path;
                }
                return null;
            }
        }
    }
}
