using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Common.Models.DB.Jukebox {

    /// <summary>Represents a harddisk drive on which the movies are stored.</summary>
    [Table("drives")]
    public class XjbDrive {

        /// <summary>Initializes a new instance of the <see cref="XjbDrive"/> class.</summary>
        public XjbDrive() {
        }

        /// <summary>Initializes a new instance of the <see cref="XjbDrive"/> class.</summary>
        /// <param name="size">The size of the drive.</param>
        /// <param name="label">The drive's label.</param>
        /// <param name="lastSeenTs">The time when was this drive last scanned in UNIX timestamp (seconds since epoch).</param>
        public XjbDrive(string size, string label, long? lastSeenTs) {
            Size = size;
            Label = label;
            LastSeenTs = lastSeenTs;
        }

        /// <summary>Gets or sets the unique identifier (GUID) of this drive in the database.</summary>
        /// <value>The unique identifier (GUID) of this drive in the database.</value>
        [Key]
        [Column("id")]
        public string Id { get; set; }

        /// <summary>Gets or sets the size of the drive.</summary>
        /// <value>The size of the drive.</value>
        [Column("size")]
        public string Size { get; set; }

        /// <summary>Gets or sets the drive's label.</summary>
        /// <value>The drive's label.</value>
        [Column("label")]
        public string Label { get; set; }

        /// <summary>Gets or sets the when was this drive last scanned in UNIX timestamp (seconds since epoch).</summary>
        /// <value>The time when was this drive last scanned in UNIX timestamp (seconds since epoch).</value>
        [Column("last_seen_ts")]
        public long? LastSeenTs { get; set; }

    }

}
