using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Providers.Xtreamer.DB {

    /// <summary>Represents a preson that participated in a movie.</summary>
    [Table("persons")]
    public class XjbPerson : IEquatable<XjbPerson> {

        public XjbPerson() {
            Movies = new HashSet<XjbMoviePerson>();
        }

        /// <summary>Initializes a new instance of the <see cref="XjbPerson"/> class.</summary>
        /// <param name="name">The full name of the person.</param>
        public XjbPerson(string name) {
            Name = name;
        }

        /// <summary>Gets or sets the Id of this person in the database.</summary>
        /// <value>The Id of this person in the database</value>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>Gets or sets the full name of the actor.</summary>
        /// <value>The full name of the person.</value>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the link to the movies that this person worked on.</summary>
        /// <value>The link to the movies that this person worked on</value>
        public virtual HashSet<XjbMoviePerson> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XjbPerson other) {
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
