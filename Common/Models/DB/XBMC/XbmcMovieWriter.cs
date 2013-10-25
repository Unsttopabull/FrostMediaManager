using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models.DB.XBMC.Actor;

namespace Common.Models.DB.XBMC {
    [Table("writerlinkmovie")]
    public class XbmcMovieWriter {

        [Key]
        [Column("idWriter", Order = 0)]
        [ForeignKey("Writer")]
        public long WriterId { get; set; }
        public virtual XbmcPerson Writer { get; set; }

        [Key]
        [Column("idMovie", Order = 1)]
        [ForeignKey("Movie")]
        public long MovieId { get; set; }
        public virtual XbmcMovie Movie { get; set; }
    }
}