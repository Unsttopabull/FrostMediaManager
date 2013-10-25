using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    [Table("bookmark")]
    public class XbmcBookmark {

        [Key]
        [Column("idBookmark")]
        public long Id { get; set; }

        [Column("idFile")]
        [ForeignKey("File")]
        public long FileId { get; set; }
        public virtual XbmcFile File { get; set; }

        [Column("timeInSeconds")]
        public long TimeInSeconds { get; set; }

        [Column("totalTimeInSeconds")]
        public long TotalTimeInSeconds { get; set; }

        [Column("thumbNailImage")]
        public string ThumbnailImage { get; set; }

        [Column("player")]
        public string Player { get; set; }

        [Column("playerState")]
        public string PlayerState { get; set; }

        [Column("type")]
        public long Type { get; set; }
    }
}