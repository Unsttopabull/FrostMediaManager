using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;
using Frost.Common.Util.ISO;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtLanguage : ILanguage {

        private XtLanguage(ISOLanguageCode isoCode) {
            if (isoCode != null) {
                ISO639 = new ISO639(isoCode.Alpha2, isoCode.Alpha3);
                Name = isoCode.EnglishName;
            }     
        }

        public long Id { get { return 0; } }

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

        public static XtLanguage FromIsoCode(string iso) {
            ISOLanguageCode isoCode = ISOLanguageCodes.Instance.GetByISOCode(iso);
            if (isoCode != null) {
                return new XtLanguage(isoCode);
            }            
            return null;            
        }

        public static XtLanguage FromEnglishNameCode(string name) {
            ISOLanguageCode isoCode = ISOLanguageCodes.Instance.GetByEnglishName(name);
            if (isoCode != null) {
                return new XtLanguage(isoCode);
            }            
            return null;            
        }
    }
}
