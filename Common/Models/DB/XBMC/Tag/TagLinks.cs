using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.Tag {

    [Table("taglinks")]
    public class TagLinks {

        [Column("idTag")]
        [ForeignKey("Tag")]
        public long TagId { get; set; }
        public virtual XbmcTag Tag { get; set; }

        [Column("idMedia")]
        public long MediaId { get; set; }

        [Column("media_type")]
        public string MediaType { get; set; }
    }
}