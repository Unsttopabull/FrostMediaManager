using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.MovieVo {

    /// <summary>Represents a country a movie was shot or/and produced in.</summary>
    public class Country : IEquatable<Country> {

        /// <summary>Initializes a new instance of the <see cref="Country"/> class.</summary>
        public Country() {
            Movies = new HashSet<Movie>();
        }

        /// <summary>Initializes a new instance of the <see cref="Country"/> class.</summary>
        /// <param name="name">The name of the country.</param>
        public Country(string name) : this() {
            Name = name;
        }

        /// <summary>Gets or sets the database Country Id.</summary>
        /// <value>The database country Id</value>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the country name.</summary>
        /// <value>The name of the country.</value>
        public string Name { get; set; }

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
        public static IEnumerable<Country> GetFromNames(string[] countryNames) {
            int numCountries = countryNames.Length;

            Country[] countries = new Country[numCountries];
            for (int i = 0; i < numCountries; i++) {
                countries[i] = new Country(countryNames[i]);
            }

            return countries;
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
