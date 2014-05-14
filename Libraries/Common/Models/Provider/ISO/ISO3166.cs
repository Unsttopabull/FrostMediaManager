﻿using System;
using Frost.Common.Util.ISO;

namespace Frost.Common.Models.Provider.ISO {

    /// <summary>Holds an ISO country code information</summary>
    public class ISO3166 : IEquatable<ISO3166> {
        /// <summary>Initializes a new instance of the <see cref="ISO3166" /> class.</summary>
        public ISO3166() {
        }

        /// <summary>Initializes a new instance of the <see cref="ISO3166" /> class.</summary>
        /// <param name="countryName">The english country name.</param>
        public ISO3166(string countryName) {
            ISOCountryCode icc = ISOCountryCodes.Instance.GetByEnglishName(countryName);
            if (icc != null) {
                EnglishName = icc.EnglishName;
                Alpha2 = icc.Alpha2;
                Alpha3 = icc.Alpha3;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ISO3166" /> class.</summary>
        /// <param name="alpha2">The ISO3166-1 2-letter country code.</param>
        /// <param name="alpha3">The ISO3166-1 3-letter country code.</param>
        public ISO3166(string alpha2, string alpha3) {
            Alpha2 = alpha2;
            Alpha3 = alpha3;
        }

        /// <summary>Gets or sets the ISO639-2 2-letter country code.</summary>
        /// <value>The ISO3166-1 2-letter country code.</value>
        public string Alpha2 { get; set; }

        /// <summary>Gets or sets the ISO639-2 3-letter country code.</summary>
        /// <value>The ISO3166-1 3-letter country code.</value>
        public string Alpha3 { get; set; }

        /// <summary>Gets the english name of the country</summary>
        /// <value>The english country name.</value>
        public string EnglishName { get; private set; }

        #region IEquatable

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ISO3166 other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (string.Equals(Alpha3, other.Alpha3)) {
                return true;
            }
            return string.Equals(Alpha2, other.Alpha2);
        }

        /// <summary>Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            return Equals(obj as ISO3166);
        }

        /// <summary>Serves as a hash function for a particular type. </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
        public override int GetHashCode() {
            unchecked {
                return ((Alpha2 != null ? Alpha2.GetHashCode() : 0) * 397) ^ (Alpha3 != null ? Alpha3.GetHashCode() : 0);
            }
        }

        #endregion
    }

}