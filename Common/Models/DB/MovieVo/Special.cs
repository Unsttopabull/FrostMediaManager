using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>Represents a special information about a movie's release or type.</summary>
    public class Special : IEquatable<Special> {

        /// <summary>Initializes a new instance of the <see cref="Special"/> class.</summary>
        /// <param name="value">The value of the special</param>
        public Special(string value) {
            Value = value;
        }

        /// <summary>Gets or sets the database Specials Id.</summary>
        /// <value>The database Special Id</value>
        public long Id { get; set; }

        ///<summary>Gets or sets special addithions or types</summary>
        ///<value>Special addithions or types</value> 
        ///<example>\eg{ <c>INTERNAL, DUBBED, LIMITED, PROPER, REPACK, RERIP, SUBBED</c>}</example>
        public string Value { get; set; }

        /// <summary>Gets or sets the movies that this special applies to</summary>
        /// <value>The movies this special applies to.</value>
        public HashSet<Movie> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Special other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Value == other.Value;
        }

        /// <summary>Converts specials as string to an <see cref="IEnumerable{T}"/> with elements of type <see cref="Special"/></summary>
        /// <param name="specials">The specials values.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Special"/> instances with specified specials values.</returns>
        public static IEnumerable<Special> FromValues(IEnumerable<string> specials) {
            return specials.Select(special => (Special) special).ToList();
        }

        /// <summary>Converts a <see cref="string"/> to an instance of <see cref="Special"/></summary>
        /// <param name="specialName">The value of the special</param>
        /// <returns>An instance of <see cref="Special"/> converted from <see cref="string"/></returns>
        public static explicit operator Special(string specialName) {
            return new Special(specialName);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Value;
        }

        internal class Configuration : EntityTypeConfiguration<Special> {

            public Configuration() {
                HasMany(s => s.Movies)
                    .WithMany(m => m.Specials)
                    .Map(m => {
                        m.ToTable("MovieSpecials");
                        m.MapLeftKey("SpecialId");
                        m.MapRightKey("MovieId");
                    });
            }

        }

    }

}
