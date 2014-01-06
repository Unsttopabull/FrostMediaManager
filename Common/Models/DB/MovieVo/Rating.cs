using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>
    /// Represents a movie's rating from a certain critic.
    /// </summary>
    [Table("Ratings")]
    public class Rating : IEquatable<Rating> {

        /// <summary>Initializes a new instance of the <see cref="Rating"/> class.</summary>
        /// <param name="critic">The name of the critic.</param>
        /// <param name="rating">The rating value</param>
        public Rating(string critic, double rating) {
            Critic = critic;
            Value = rating;
        }

        /// <summary>Gets or sets the database rating Id.</summary>
        /// <value>The database rating Id</value>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of the critic.</summary>
        /// <value>The name of the critic.</value>
        public string Critic { get; set; }

        /// <summary>Gets or sets the value of the rating.</summary>
        /// <value>The rating value</value>
        public double Value { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the movie this rating is for.</summary>
        /// <value>The movie this rating is for.</value>
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Rating other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Critic == other.Critic &&
                   Math.Abs(Value - other.Value) < 0.01 &&
                   MovieId == other.MovieId;
        }

    }

}
