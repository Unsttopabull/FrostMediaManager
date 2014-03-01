using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Model.Xbmc.DB {

    /// <summary>This table stores the database version and compression information.</summary>
    [Table("version")]
    public class XbmcVersion : IEquatable<XbmcVersion> {

        /// <summary>Gets or sets the version of this database</summary>
        /// <value>The Id of this studio in the database</value>
        [Key]
        [Column("idVersion")]
        public long Version { get; set; }

        /// <summary>Gets or sets the number of times database has been compressed</summary>
        /// <value>The number of times database has been compressed</value>
        [Column("idCompressCount")]
        public long CompressCountId { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcVersion other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return Version == other.Version &&
                   CompressCountId == other.CompressCountId;
        }

    }

}
