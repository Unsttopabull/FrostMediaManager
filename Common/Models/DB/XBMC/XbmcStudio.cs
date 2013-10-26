using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    [Table("studio")]
    public class XbmcStudio {

        public XbmcStudio() {
            Movies = new HashSet<XbmcMovie>();
        }

        [Key]
        [Column("idStudio")]
        public long Id { get; set; }

        [Column("strStudio")]
        public string StudioName { get; set; }

        public ICollection<XbmcMovie> Movies { get; set; }
    }
}