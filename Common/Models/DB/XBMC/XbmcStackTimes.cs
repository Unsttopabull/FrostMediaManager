using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Common.Models.DB.XBMC {

    /// <summary>This table stores playing times for files (used for playing multi-file videos).</summary>
    [Table("stacktimes")]
    public class XbmcStackTimes : IEquatable<XbmcStackTimes> {

        /// <summary>Gets or sets the foreign key to the file referenced.</summary>
        /// <value>The foreign key to the file referenced.</value>
        [Key]
        [Column("idFile")]
        [ForeignKey("File")]
        public long FileId { get; set; }

        /// <summary>Gets or sets the number of times the referenced file has been played.</summary>
        /// <value>The number of times the referenced file has been played</value>
        [Column("times")]
        public string Times { get; set; }

        /// <summary>Gets or sets the file referenced.</summary>
        /// <value>The file referenced.</value>
        public virtual XbmcFile File { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcStackTimes other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (FileId != 0 || other.FileId != 0) {
                return FileId == other.FileId;
            }

            return Times == other.Times;
        }

    }

}
