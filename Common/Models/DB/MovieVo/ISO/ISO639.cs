﻿using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common.Util.ISO;

namespace Frost.Common.Models.DB.MovieVo.ISO {

    /// <summary>Holds an ISO language code information</summary>
    [ComplexType]
    public class ISO639 {
        [NotMapped]
        internal readonly string EnglishName;

        /// <summary>Initializes a new instance of the <see cref="ISO639" /> class.</summary>
        public ISO639() {
        }

        /// <summary>Initializes a new instance of the <see cref="ISO639" /> class.</summary>
        /// <param name="englishName">The english name of the language.</param>
        public ISO639(string englishName) {
            ISOLanguageCode iso = ISOLanguageCodes.Instance.GetByEnglishName(englishName);
            if (iso != null) {
                EnglishName = iso.EnglishName;
                Alpha2 = iso.Alpha2;
                Alpha3 = iso.Alpha3;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ISO639" /> class.</summary>
        /// <param name="alpha2">The ISO639-1 2-letter language code.</param>
        /// <param name="alpha3">The ISO639-2 3-letter language code.</param>
        public ISO639(string alpha2, string alpha3) {
            Alpha2 = alpha2;
            Alpha3 = alpha3;
        }

        /// <summary>Gets or sets the ISO639-1 2-letter language code.</summary>
        /// <value>The ISO639-1 2-letter language code.</value>
        public string Alpha2 { get; set; }

        /// <summary>Gets or sets the ISO639-2 3-letter language code.</summary>
        /// <value>The ISO639-2 3-letter language code.</value>
        public string Alpha3 { get; set; }
    }

}