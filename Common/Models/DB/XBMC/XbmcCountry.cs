using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.XBMC {

    /// <summary>This table lists all countries in which the movies were shot and/or produced.</summary>
    [Table("country")]
    public class XbmcCountry : IEquatable<XbmcCountry> {

        /// <summary>Initializes a new instance of the <see cref="XbmcCountry"/> class.</summary>
        public XbmcCountry() {
            Movies = new HashSet<XbmcMovie>();
        }

        /// <summary>Gets or sets the Id of the country in the database.</summary>
        /// <value>The Id of the country in the database.</value>
        [Key]
        [Column("idCountry")]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of the country.</summary>
        /// <value>The name of the country.</value>
        [Column("strCountry")]
        public string Name { get; set; }

        /// <summary>Gets or sets the movies that were shor and/or produced in this country.</summary>
        /// <value>The movies that were shor and/or produced in this country.</value>
        public HashSet<XbmcMovie> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcCountry other) {
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

        internal class Configuration : EntityTypeConfiguration<XbmcCountry> {

            public Configuration() {
                //Join table Movie <--> Country
                HasMany(m => m.Movies)
                    .WithMany(c => c.Countries)
                    .Map(m => {
                        m.ToTable("countrylinkmovie");
                        m.MapLeftKey("idCountry");
                        m.MapRightKey("idMovie");
                    });
            }

        }

    }

}
