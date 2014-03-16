using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Providers.Xbmc.DB {

    /// <summary>This table stores the database version and compression information.</summary>
    [Table("version")]
    public class XbmcVersion {

        /// <summary>Gets or sets the version of this database</summary>
        /// <value>The Id of this studio in the database</value>
        [Key]
        [Column("idVersion")]
        public long Version { get; set; }

        /// <summary>Gets or sets the number of times database has been compressed</summary>
        /// <value>The number of times database has been compressed</value>
        [Column("idCompressCount")]
        public long CompressCountId { get; set; }

    }

}
