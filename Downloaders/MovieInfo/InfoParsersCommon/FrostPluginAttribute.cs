using System;

namespace Frost.InfoParsers {

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class FrostPluginAttribute : Attribute {
    }
}
