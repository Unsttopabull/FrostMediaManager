using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    [Table("stacktimes")]
    public class XbmcStackTimes {

        [Column("idFile")]
        [ForeignKey("File")]
        public long FileId { get; set; }

        [Column("times")]
        public string Times { get; set; }

        public virtual XbmcFile File { get; set; }
    }
}