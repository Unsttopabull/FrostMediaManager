using System;

namespace Frost.Common {

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class IsPluginAttribute : Attribute {

        public IsPluginAttribute(string systemName) {
            SystemName = systemName;
        }

        public string SystemName { get; set; }
    }
}
