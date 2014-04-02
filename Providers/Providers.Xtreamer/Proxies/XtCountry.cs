using System;
using System.Collections.Generic;
using Frost.Common.Comparers;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;
using Frost.Common.Util.ISO;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtCountry : ICountry, IEquatable<XtCountry>{
        private readonly IEqualityComparer<ICountry> _comparer;

        public XtCountry() {
            _comparer = new CountryEqualityComparer();
        }

        public XtCountry(string name) : this() {
            ISOCountryCode isoCode = ISOCountryCodes.Instance.GetByISOCode(name);
            if (isoCode != null) {
                ISO3166 = new ISO3166(isoCode.Alpha2, isoCode.Alpha3);
                Name = isoCode.EnglishName;
            }
        }

        public XtCountry(ISOCountryCode iso) : this() {
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


        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XtCountry other) {
            return _comparer.Equals(this, other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public override bool Equals(object other) {
            return Equals(other as XtCountry);
        }

        /// <summary>Serves as a hash function for a particular type.</summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
        public override int GetHashCode() {
            return _comparer.GetHashCode(this);
        }
    }
}
