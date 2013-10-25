using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.Tag {

    [Table("tag")]
    public class XbmcTag {

        [Key]
        [Column("idTag")]
        public long TagId { get; set; }

        [Column("strTag")]
        public string TagName { get; set; }
    }
}