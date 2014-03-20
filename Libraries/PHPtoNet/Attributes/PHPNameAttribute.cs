using System;

namespace Frost.PHPtoNET.Attributes {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct, AllowMultiple = false)]
    public class PHPNameAttribute : Attribute {

        public PHPNameAttribute(string phpName) {
            PHPName = phpName;
        }

        public string PHPName { get; private set; }
    }
}
