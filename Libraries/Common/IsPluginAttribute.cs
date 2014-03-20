using System;

namespace Frost.Common {

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class IsPluginAttribute : Attribute {

        public IsPluginAttribute(string systemName, string iconPath = null) {
            SystemName = systemName;
            IconPath = iconPath;
        }

        public string SystemName { get; set; }
        public string IconPath { get; set; }
    }
}
