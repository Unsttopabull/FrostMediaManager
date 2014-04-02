using System.Text;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;
using Frost.Common.Util.ISO;

namespace Frost.Providers.Xbmc.DB.Proxy {
    public class XbmcLanguage : ILanguage {

        /// <summary>Initializes a new instance of the <see cref="XbmcLanguage"/> class.</summary>
        public XbmcLanguage() {
            ISO639 = new ISO639();
        }

        public XbmcLanguage(ISOLanguageCode isoCode) {

            Name = isoCode.EnglishName;
            ISO639 = new ISO639(isoCode.Alpha2, isoCode.Alpha3);
        }

        public long Id {
            get { return default(long); }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Name":
                    case "ISO639":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the name of this language.</summary>
        /// <value>The name of this language.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the ISO639 language codes.</summary>
        /// <value>The ISO639 language codes.</value>
        public ISO639 ISO639 { get; set; }


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            if (string.IsNullOrEmpty(Name)) {
                return "";
            }

            StringBuilder sb = new StringBuilder(20);
            sb.Append(Name);

            if (!string.IsNullOrEmpty(ISO639.Alpha2)) {
                sb.Append(" (" + ISO639.Alpha2);
            }

            if (!string.IsNullOrEmpty(ISO639.Alpha3)) {
                sb.Append(", " + ISO639.Alpha3 + ")");
            }
            else {
                sb.Append(")");
            }
            return sb.ToString();
        }
    }
}
