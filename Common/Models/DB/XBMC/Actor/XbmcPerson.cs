using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.Actor {

    [Table("actors")]
    public class XbmcPerson {
        public XbmcPerson() {
            MoviesAsDirector = new HashSet<XbmcMovie>();
            MoviesAsActor = new HashSet<XbmcMovieActor>();
            MoviesAsWriter = new HashSet<XbmcMovie>();
        }

        public XbmcPerson(string name, string thumbXml) : this() {
            Name = name;
            ThumbXml = thumbXml;
        }

        internal XbmcPerson(long id, string name, string thumbXml) : this(name, thumbXml) {
            Id = id;
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

        public virtual ICollection<XbmcMovie> MoviesAsDirector { get; set; }
        public virtual ICollection<XbmcMovieActor> MoviesAsActor { get; set; }
        public virtual ICollection<XbmcMovie> MoviesAsWriter { get; set; }
    }
}