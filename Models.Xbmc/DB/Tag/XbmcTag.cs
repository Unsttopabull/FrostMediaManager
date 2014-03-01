using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Model.Xbmc.DB.Tag {

    /// <summary>Represents a table that lists tags.</summary>
    [Table("tag")]
    public class XbmcTag : IEquatable<XbmcTag> {

        /// <summary>Gets or sets the Id of the Tag in the database.</summary>
        /// <value>The Id of the Tag in the database.</value>
        [Key]
        [Column("idTag")]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of the tag.</summary>
        /// <value>The name of the tag.</value>
        [Column("strTag")]
        public string Name { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcTag other) {
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
