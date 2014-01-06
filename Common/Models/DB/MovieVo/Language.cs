using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Frost.Common.Models.DB.MovieVo.ISO;
using Frost.Common.Util.ISO;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>Represents a language information.</summary>
    [Table("Languages")]
    public class Language : IEquatable<Language> {

        /// <summary>Initializes a new instance of the <see cref="Language"/> class.</summary>
        public Language() {
            ISO639 = new ISO639();
        }

        public Language(ISOLanguageCode isoCode) {
            Name = isoCode.EnglishName;
            ISO639 = new ISO639(isoCode.Alpha2, isoCode.Alpha3);
        }

        /// <summary>Initializes a new instance of the <see cref="Language"/> class.</summary>
        /// <param name="name">The english name of this language.</param>
        public Language(string name) {
            if (!string.IsNullOrEmpty(name)) {
                Name = name;
                ISO639 = new ISO639(name);
            }
            else {
                ISO639 = new ISO639();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Language"/> class.</summary>
        /// <param name="name">The english name of this language.</param>
        /// <param name="alpha2">The ISO639-2 2-letter language code.</param>
        /// <param name="alpha3">The ISO639-2 3-letter language code.</param>
        public Language(string name, string alpha2, string alpha3) : this() {
            Name = name;
            ISO639.Alpha2 = alpha2;
            ISO639.Alpha3 = alpha3;
        }

        /// <summary>Gets or sets the Id of this language in the database.</summary>
        /// <value>The Id of this language in the database</value>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of this language.</summary>
        /// <value>The name of this language.</value>
        [Required]
        public string Name { get; set; }

        /// <summary>Gets or sets the ISO639 language codes.</summary>
        /// <value>The ISO639 language codes.</value>
        public ISO639 ISO639 { get; set; }

        /// <summary>Gets or sets the foreign key to the language's country.</summary>
        /// <value>The foreign key to the language's country</value>
        public long? CountryId { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Language other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Name == other.Name;
        }

        /// <summary>Get an instance of <see cref="Language"/> from an ISO 639 2 or 3 letter code.</summary>
        /// <param name="iso639">The ISO 639 2 or 3 letter code.</param>
        /// <returns>Returns a language information from ISO 639 2 or 3 letter code. If an inapropriate string is passed it returns <c>null</c>.</returns>
        public static Language FromISO639(string iso639) {
            ISOLanguageCode iso = ISOLanguageCodes.Instance.GetByISOCode(iso639);
            return iso != null
                ? new Language(iso.EnglishName, iso.Alpha2, iso.Alpha3)
                : null;
        }

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