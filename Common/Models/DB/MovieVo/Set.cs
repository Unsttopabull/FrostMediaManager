using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.DB.MovieVo {

    /// <summary>Represents a movie set or collection.</summary>
    public class Set : IEquatable<Set> {

        /// <summary>Initializes a new instance of the <see cref="Set"/> class.</summary>
        private Set() {
            Movies = new HashSet<Movie>();
        }

        /// <summary>Initializes a new instance of the <see cref="Set"/> class with specified set name.</summary>
        /// <param name="name">The name of this set.</param>
        public Set(string name) : this() {
            Name = name;
        }

        /// <summary>Gets or sets the database Id of this set.</summary>
        /// <value>The database Id of this set</value>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of this set.</summary>
        /// <value>The name of this set.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the movies in this set.</summary>
        /// <value>The movies in this set.</value>
        public HashSet<Movie> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Set other) {
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

    }

}
