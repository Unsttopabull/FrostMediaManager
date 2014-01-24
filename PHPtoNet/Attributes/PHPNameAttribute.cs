using System;

namespace Frost.PHPtoNET.Attributes {
    public class PHPNameAttribute : Attribute {

        public PHPNameAttribute(string phpName) {
            PHPName = phpName;
        }

        public string PHPName { get; private set; }
    }
}
