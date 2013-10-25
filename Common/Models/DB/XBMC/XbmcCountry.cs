using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    [Table("country")]
    public class XbmcCountry {
        public XbmcCountry() {
            Movies = new HashSet<XbmcMovie>();
        }

        [Key]
        [Column("idCountry")]
        public long Id { get; set; }

        [Column("strCountry")]
        public string Name { get; set; }

        public ICollection<XbmcMovie> Movies { get; set; }
    }
}