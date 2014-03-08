using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using Frost.Common.Models;

namespace Frost.Models.Frost.DB {

    /// <summary>Represents a movie's rating from a certain critic.</summary>
    [Table("Ratings")]
    public class Rating : IRating<Movie> {

        public Rating() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="Rating"/> class.</summary>
        /// <param name="critic">The name of the critic.</param>
        /// <param name="rating">The rating value</param>
        public Rating(string critic, double rating) {
            Critic = critic;
            Value = rating;
        }

        public Rating(IRating rating) {
            Contract.Requires<ArgumentNullException>(rating != null);

            Critic = rating.Critic;
            Value = rating.Value;
            if (rating.Movie != null) {
                Movie = new Movie(rating.Movie);
            }
        }

        /// <summary>Gets or sets the database rating Id.</summary>
        /// <value>The database rating Id</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of the critic.</summary>
        /// <value>The name of the critic.</value>
        [Required]
        public string Critic { get; set; }

        /// <summary>Gets or sets the value of the rating.</summary>
        /// <value>The rating value</value>
        [Required]
        public double Value { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the movie this rating is for.</summary>
        /// <value>The movie this rating is for.</value>
        public virtual Movie Movie { get; set; }

        /// <summary>Gets or sets the movie this rating is for.</summary>
        /// <value>The movie this rating is for.</value>
        IMovie IRating.Movie {
            get { return Movie; }
            set { Movie = new Movie(value); }
        }
    }

}
