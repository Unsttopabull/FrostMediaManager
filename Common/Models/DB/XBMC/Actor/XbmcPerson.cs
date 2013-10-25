using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.Actor {

    [Table("actors")]
    public class XbmcPerson {

        public XbmcPerson() {
            Movies = new HashSet<XbmcMovie>();
        }

        [Key]
        [Column("idActor")]
        public long Id { get; set; }

        [Column("strActor")]
        public string Name { get; set; }

        [Column("strThumb")]
        public string ThumbXml { get; set; }

        [NotMapped]
        public string ThumbURL {
            get {
                return ThumbXml != null
                               ? ThumbXml.Replace("<thumb>", "").Replace("</thumb>", "")
                               : null;
            }
            set { ThumbXml = "<thumb>" + value + "</thumb>"; }
        }

        public virtual ICollection<XbmcMovie> Movies { get; set; }
    }
}