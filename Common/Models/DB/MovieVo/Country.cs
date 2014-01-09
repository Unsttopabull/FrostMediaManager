using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Frost.Common.Models.DB.MovieVo.ISO;
using Frost.Common.Util.ISO;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary> Represents a country a movie was shot and/or produced in.</summary>
    [Table("Countries")]
    public class Country : IEquatable<Country> {

        /// <summary>Initializes a new instance of the <see cref="Country"/> class.</summary>
        public Country() {
            Movies = new HashSet<Movie>();
            ISO3166 = new ISO3166();
        }

        /// <summary>Initializes a new instance of the <see cref="Country"/> class.</summary>
        /// <param name="name">The name of the country.</param>
        public Country(string name) {
            Movies = new HashSet<Movie>();

            Name = name;
            ISO3166 = new ISO3166(name);
        }

        /// <summary>Initializes a new instance of the <see cref="Country"/> class.</summary>
        /// <param name="name">The name of the country.</param>
        /// <param name="alpha2">The ISO3166-1 2-letter country code.</param>
        /// <param name="alpha3">The ISO3166-1 3-letter country code.</param>
        public Country(string name, string alpha2, string alpha3) : this() {
            Name = name;
            ISO3166.Alpha2 = alpha2;
            ISO3166.Alpha3 = alpha3;
        }

        /// <summary>Gets or sets the database Country Id.</summary>
        /// <value>The database country Id</value>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the country name.</summary>
        /// <value>The name of the country.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the ISO 3166-1 Information.</summary>
        /// <value>The ISO 3166-1 Information.</value>
        public ISO3166 ISO3166 { get; set; }

        /// <summary>Gets or sets the movies shot in this country.</summary>
        /// <value>The country movies</value>
        public virtual HashSet<Movie> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Country other) {
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

        /// <summary>Converts country names to an <see cref="IEnumerable{T}"/> with elements of type <see cref="Country"/></summary>
        /// <param name="countryNames">The counry names.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Country"/> instances with specified country names</returns>
        public static IEnumerable<Country> GetFromNames(IEnumerable<string> countryNames) {
            return countryNames.Select(countryName => new Country(countryName));
        }

        /// <summary>Get an instance of <see cref="Country"/> from an ISO 3166-1 2 letter code.</summary>
        /// <param name="iso3166">The ISO 3166-1 2 letter code.</param>
        /// <returns>Returns a country information from ISO 3166-1 2 letter code. If an inapropriate string is passed it returns <c>null</c>.</returns>
        public static Country FromISO3166(string iso3166) {
            ISOLanguageCode iso = ISOLanguageCodes.Instance.GetByISOCode(iso3166);
            return iso != null
                ? new Country(iso.EnglishName, iso.Alpha2, iso.Alpha3)
                : null;
        }

        internal class CountryConfiguration : EntityTypeConfiguration<Country> {

            public CountryConfiguration() {
                //Join tabela za Movie <--> Country
                HasMany(m => m.Movies)
                    .WithMany(c => c.Countries)
                    .Map(m => {
                        m.ToTable("MovieCountries");
                        m.MapLeftKey("CountryId");
                        m.MapRightKey("MovieId");
                    });
            }

        }

    }

}
