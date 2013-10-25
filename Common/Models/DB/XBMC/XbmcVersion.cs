using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    [Table("version")]
    public class XbmcVersion {

        [Key]
        [Column("idVersion")]
        public long VersionId { get; set; }

        [Column("idCompressCount")]
        public long CompressCountId { get; set; }
    }
}