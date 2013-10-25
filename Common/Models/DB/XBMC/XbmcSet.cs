using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    [Table("sets")]
    public class XbmcSet {

        public XbmcSet() {
            Movies = new HashSet<XbmcMovie>();
        }

        public XbmcSet(string name) : this() {
            Name = name;
        }

        [Key]
        [Column("idSet")]
        public long SetId { get; set; }

        [Column("strSet")]
        public string Name { get; set; }

        public virtual ICollection<XbmcMovie> Movies { get; set; }
    }
}