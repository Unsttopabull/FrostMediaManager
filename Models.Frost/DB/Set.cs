using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using Frost.Common.Models;

namespace Frost.Models.Frost.DB {

    /// <summary>Represents a movie set or collection.</summary>
    [Table("Sets")]
    public class Set : IMovieSet<Movie> {
        /// <summary>Initializes a new instance of the <see cref="Set"/> class.</summary>
        public Set() {
            Movies = new HashSet<Movie>();
        }

        /// <summary>Initializes a new instance of the <see cref="Set"/> class with specified set name.</summary>
        /// <param name="name">The name of this set.</param>
        public Set(string name) : this() {
            Name = name;
        }

        public Set(IMovieSet value) {
            Contract.Requires<ArgumentNullException>(value != null);
            Contract.Requires<ArgumentNullException>(value.Movies != null);

            Name = value.Name;
            Movies = new HashSet<Movie>(value.Movies.Select(m => new Movie(m)));
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

        /// <summary>Gets or sets the movies in this set.</summary>
        /// <value>The movies in this set.</value>
        ICollection<IMovie> IMovieSet.Movies {
            get { return new HashSet<IMovie>(Movies); }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name ?? base.ToString();
        }
    }

}