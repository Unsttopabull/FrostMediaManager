using WPFLocalizeExtension.Providers;

namespace RibbonUI.Translation {
    public class GettextFullyQualifiedResourceKeyBase : FullyQualifiedResourceKeyBase {

        public GettextFullyQualifiedResourceKeyBase(string key) {
            Key = key;
        }

        public string Key { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Key;
        }
    }
}
