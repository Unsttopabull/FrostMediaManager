using System;

namespace RibbonUI.Util {
    public class Plugin {

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public Plugin(string name) {
            Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public Plugin(string name, Uri iconPath) {
            Name = name;
            IconPath = iconPath;
        }

        public string Name { get; set; }
        public Uri IconPath { get; set; }
    }
}
