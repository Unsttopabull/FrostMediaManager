using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    [Table("art")]
    public class XbmcArt {

        [Key]
        [Column("art_id")]
        public long Id { get; set; }

        /// <summary>Id of the Actor/Movie/Set referenced</summary>
        [Column("media_id")]
        public long MediaID { get; set; }

        /// <summary>Media type (Actor/Movie/Set)</summary>
        [Column("media_type")]
        public string MediaType { get; set; }

        /// <summary>thumb/fanart/poster</summary>
        [Column("type")]
        public string Type { get; set; }

        /// <summary>Path to the art (URL or path on drive/network)</summary>
        [Column("url")]
        public string Url { get; set; }
    }
}