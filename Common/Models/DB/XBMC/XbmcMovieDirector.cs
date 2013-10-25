using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models.DB.XBMC.Actor;

namespace Common.Models.DB.XBMC {

    [Table("directorlinkmovie")]
    public class XbmcMovieDirector {

        [Key]
        [Column("idDirector", Order = 0)]
        [ForeignKey("Director")]
        public long DirectorId { get; set; }
        public virtual XbmcPerson Director { get; set; }

        [Key]
        [Column("idMovie", Order = 1)]
        [ForeignKey("Movie")]
        public long MovieId { get; set; }
        public virtual XbmcMovie Movie { get; set; }
    }
}