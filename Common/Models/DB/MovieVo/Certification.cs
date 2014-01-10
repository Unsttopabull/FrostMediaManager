using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>Represents a movie certification/restriction in a certain country.</summary>
    [Table("Certifications")]
    public class Certification : CertificationBase, IEquatable<Certification> {
        /// <summary>Initializes a new instance of the <see cref="Certification"/> class.</summary>
        public Certification() {
        }

        /// <summary>Initializes a new instance of the <see cref="Certification"/> class.</summary>
        /// <param name="countryName">The coutry name this certification applies to.</param>
        /// <param name="rating">The rating value.</param>
        public Certification(string countryName, string rating) : this(new Country(countryName), rating) {
        }

        /// <summary>Initializes a new instance of the <see cref="Certification"/> class.</summary>
        /// <param name="country">The coutry this certification applies to.</param>
        /// <param name="rating">The rating value.</param>
        public Certification(Country country, string rating) {
            Country = country;

            Rating = rating;
        }

        /// <summary>Gets or sets the database certification Id.</summary>
        /// <value>The database certification Id</value>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        [Required]
        public string Rating { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the country foreign key.</summary>
        /// <value>The country foreign key.</value>
        public long CountryId { get; set; }

        /// <summary>Gets or sets the movie this certification applies to.</summary>
        /// <value>The movie this certification applies to.</value>
        public virtual Movie Movie { get; set; }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        [Required]
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Certification other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Rating == other.Rating &&
                   CountryId == other.CountryId &&
                   MovieId == other.MovieId;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Country.Name + COUNTRY_RATING_SEPARATOR + Rating;
        }

        /// <summary>Parses the certifications string and returns certifications as an array of <see cref="Certification"/> instances.</summary>
        /// <param name="certStr">The certification string to parse.</param>
        /// <returns>An array of <see cref="Certification"/> instances parsed from the certifications string</returns>
        public static Certification[] ParseCertificationsString(string certStr) {
            return ParseCertificationsString<Certification>(certStr);
        }

        /// <summary>Gets an instance of <see cref="Certification"/> from the Country name and its rating</summary>
        /// <param name="country">The country name.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>An instance of <see cref="Certification"/> from the Country name and its rating</returns>
        protected override T FromCountyRating<T>(string country, string rating) {
            return new Certification(country, rating) as T;
        }

        internal class Configuration : EntityTypeConfiguration<Certification> {
            public Configuration() {
                ToTable("Certifications");

                HasRequired(c => c.Movie)
                    .WithMany(m => m.Certifications)
                    .HasForeignKey(c => c.MovieId)
                    .WillCascadeOnDelete();

                HasRequired(c => c.Country);
            }
        }
    }

}