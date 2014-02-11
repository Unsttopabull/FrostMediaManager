using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>
    /// Represents a movie's rating from a certain critic.
    /// </summary>
    [Table("Ratings")]
    public class Rating {

        public Rating() {
            
        }

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
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
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
    }

}
