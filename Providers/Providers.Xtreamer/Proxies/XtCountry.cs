using System.Xml.Schema;
using Frost.Common.Models;
using Frost.Common.Models.ISO;
using Frost.Common.Util.ISO;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtCountry : ICountry {

        public XtCountry(string name) {
            ISOCountryCode isoCode = ISOCountryCodes.Instance.GetByISOCode(name);
            if (isoCode != null) {
                ISO3166 = new ISO3166(isoCode.Alpha2, isoCode.Alpha3);
                Name = isoCode.EnglishName;
            }
        }

        public XtCountry(ISOCountryCode iso) {
            if (iso != null) {
                ISO3166 = new ISO3166(iso.Alpha2, iso.Alpha3);
                Name = iso.EnglishName;
            }            
        }

        public long Id {
            get { return 0; }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Name":
                        return true;
                    default:
                        return false;
                }    
            }
        }

        /// <summary>Gets or sets the country name.</summary>
        /// <value>The name of the country.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the ISO 3166-1 Information.</summary>
        /// <value>The ISO 3166-1 Information.</value>
        public ISO3166 ISO3166 { get; set; }


        public static XtCountry FromIsoCode(string iso) {
            ISOCountryCode isoCode = ISOCountryCodes.Instance.GetByISOCode(iso);
            if (isoCode != null) {
                return new XtCountry(isoCode);
            }            
            return null;
        }
    }
}
