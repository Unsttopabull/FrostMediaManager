using System;

namespace Frost.Common {

    /// <summary>Assembly level attribute signaling that this assembly contains Frost Media Manager movie provider</summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class FrostProviderAttribute : Attribute {

        /// <summary>Initializes a new instance of the <see cref="FrostProviderAttribute"/> class.</summary>
        /// <param name="systemName">Name of the system this provider is for (eg. XBMC).</param>
        /// <param name="iconPath">The icon file name in the provider directory.</param>
        public FrostProviderAttribute(string systemName, string iconPath = null) {
            SystemName = systemName;
            IconPath = iconPath;
        }

        /// <summary>Gets or sets the name of the system this provider is for (eg. XBMC).</summary>
        /// <value>The name of the system this provider is for.</value>
        public string SystemName { get; set; }


        /// <summary>Gets or sets the icon path.</summary>
        /// <value>The icon file name in the provider directory.</value>
        public string IconPath { get; set; }
    }
}
