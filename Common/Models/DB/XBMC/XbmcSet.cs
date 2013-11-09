using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.XBMC {

    /// <summary>Represents a table that lists movie sets and collections.</summary>
    [Table("sets")]
    public class XbmcSet : IEquatable<XbmcSet> {

        /// <summary>Initializes a new instance of the <see cref="XbmcSet"/> class.</summary>
        public XbmcSet() {
            Movies = new HashSet<XbmcMovie>();
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcSet"/> class.</summary>
        /// <param name="name">The name.</param>
        public XbmcSet(string name) : this() {
            Name = name;
        }

        /// <summary>Gets or sets the id of the set or collection in the database.</summary>
        /// <value>The id of the set or collection in the database</value>
        [Key]
        [Column("idSet")]
        public long Id { get; set; }

        /// <summary>Gets or sets the title of the set or collection</summary>
        /// <value>The title of the set or collection</value>
        [Column("strSet")]
        public string Name { get; set; }

        /// <summary>Gets or sets the movies contained in this set or collection.</summary>
        /// <value>The movies contained in this set or collection</value>
        public virtual HashSet<XbmcMovie> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcSet other) {
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

        internal class Configuration : EntityTypeConfiguration<XbmcSet> {

            public Configuration() {
                HasMany(s => s.Movies)
                    .WithOptional(m => m.Set)
                    .HasForeignKey(s => s.SetId);
            }

        }

    }

}
