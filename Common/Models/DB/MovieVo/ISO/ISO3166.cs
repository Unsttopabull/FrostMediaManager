using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common.Util;
using Frost.Common.Util.ISO;

namespace Frost.Common.Models.DB.MovieVo.ISO {

    /// <summary>Holds an ISO country code information</summary>
    [ComplexType]
    public class ISO3166 {

        /// <summary>Initializes a new instance of the <see cref="ISO3166" /> class.</summary>
        public ISO3166() {
        }

        /// <summary>Initializes a new instance of the <see cref="ISO3166" /> class.</summary>
        /// <param name="countryName">The english country name.</param>
        public ISO3166(string countryName) {
            ISOCountryCode icc = ISOCountryCodes.Instance.GetByEnglishName(countryName);
            if (icc != null) {
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
    }

}