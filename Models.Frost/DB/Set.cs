using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common.Models;

namespace Frost.Models.Frost.DB {

    /// <summary>Represents a movie set or collection.</summary>
    [Table("Sets")]
    public class Set : IMovieSet {
        /// <summary>Initializes a new instance of the <see cref="Set"/> class.</summary>
        public Set() {
            Movies = new HashSet<Movie>();
        }

        /// <summary>Initializes a new instance of the <see cref="Set"/> class with specified set name.</summary>
        /// <param name="name">The name of this set.</param>
        public Set(string name) : this() {
            Name = name;
        }

        internal Set(IMovieSet value) {
            Name = value.Name;
        }

        /// <summary>Gets or sets the database Id of this set.</summary>
        /// <value>The database Id of this set</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of this set.</summary>
        /// <value>The name of this set.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the movies in this set.</summary>
        /// <value>The movies in this set.</value>
        public ICollection<Movie> Movies { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name ?? base.ToString();
        }
    }

}