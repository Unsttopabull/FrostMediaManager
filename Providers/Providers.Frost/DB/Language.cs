﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using Frost.Common.Models;
using Frost.Common.Models.ISO;
using Frost.Common.Util.ISO;

namespace Frost.Providers.Frost.DB {

    /// <summary>Represents a language information.</summary>
    [Table("Languages")]
    public class Language : ILanguage {

        /// <summary>Initializes a new instance of the <see cref="Language"/> class.</summary>
        public Language() {
            ISO639 = new ISO639();
        }

        /// <summary>Initializes a new instance of the <see cref="Language"/> class.</summary>
        /// <param name="language">The ILanguage implementation.</param>
        internal Language(ILanguage language) {
            ISO639 = language.ISO639;
            Name = language.Name;
        }

        public Language(ISOLanguageCode isoCode) {
            Name = isoCode.EnglishName;
            ISO639 = new ISO639(isoCode.Alpha2, isoCode.Alpha3);
        }

        /// <summary>Initializes a new instance of the <see cref="Language"/> class.</summary>
        /// <param name="name">The english name of this language.</param>
        public Language(string name) {
            if (!string.IsNullOrEmpty(name)) {
                Name = name.Trim();
                ISO639 = new ISO639(Name);
                if (!string.IsNullOrEmpty(ISO639.EnglishName) && string.Compare(Name, ISO639.EnglishName, StringComparison.OrdinalIgnoreCase) != 0) {
                    Name = ISO639.EnglishName;
                }
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
            int idxName = name.IndexOf('/');
            Name = idxName != -1 ? name.Substring(0, idxName - 1) : name;

            if (!string.IsNullOrEmpty(alpha3)) {
                int idxAlpha3 = alpha3.IndexOf('/');
                ISO639.Alpha3 = idxAlpha3 != -1 ? alpha3.Substring(0, idxAlpha3 - 1) : alpha3;
            }            

            if (!string.IsNullOrEmpty(alpha2)) {
                int idxAlpha2 = alpha2.IndexOf('/');
                ISO639.Alpha2 = idxAlpha2 != -1 ? alpha2.Substring(0, idxAlpha2 - 1) : alpha2;
            }
        }

        /// <summary>Gets or sets the Id of this language in the database.</summary>
        /// <value>The Id of this language in the database</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of this language.</summary>
        /// <value>The name of this language.</value>
        [Required]
        public string Name { get; set; }

        /// <summary>Gets or sets the ISO639 language codes.</summary>
        /// <value>The ISO639 language codes.</value>
        public ISO639 ISO639 { get; set; }

        bool IMovieEntity.this[string propertyName] {
            get { return true; }
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

        internal class Configuration : EntityTypeConfiguration<Language> {
            public Configuration() {
                ToTable("Languages");
            }
        }
    }

}