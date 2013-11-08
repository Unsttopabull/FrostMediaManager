using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    /// <summary>This table stores playing times for files (used for playing multi-file videos).</summary>
    [Table("stacktimes")]
    public class XbmcStackTimes {

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
    }
}