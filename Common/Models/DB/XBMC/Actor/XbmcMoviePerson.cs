using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.Actor {

    [Table("actorlinkmovie")]
    public class XbmcMoviePerson {

        [Key]
        [Column("idActor", Order = 0)]
        public long PersonId { get; set; }

        [ForeignKey("PersonId")]
        public virtual XbmcPerson Person { get; set; }

        [Key]
        [Column("idMovie", Order = 1)]
        public long MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual XbmcMovie Movie { get; set; }

        [Column("strRole")]
        public string Role { get; set; }

        [Column("iOrder")]
        public long Order { get; set; }
    }
}