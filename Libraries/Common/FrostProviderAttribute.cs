using System;

namespace Frost.Common {

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class FrostProviderAttribute : Attribute {

        public FrostProviderAttribute(string systemName, string iconPath = null) {
            SystemName = systemName;
            IconPath = iconPath;
        }

        public string SystemName { get; set; }
        public string IconPath { get; set; }
    }
}
