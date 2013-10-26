using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    [Table("genre")]
    public class XbmcGenre {

        public XbmcGenre() {
            Movies = new HashSet<XbmcMovie>();
        }
    
        [Key]
        [Column("idGenre")]
        public long Id { get; set; }

        [Column("strGenre")]
        public string GenreName { get; set; }

        public ICollection<XbmcMovie> Movies { get; set; }
    }
}