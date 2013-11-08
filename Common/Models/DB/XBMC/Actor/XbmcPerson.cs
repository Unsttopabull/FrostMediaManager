using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.XBMC.Actor {

    /// <summary>Represents a person that worked on a movie.</summary>
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

        public virtual HashSet<XbmcMovie> MoviesAsDirector { get; set; }
        public virtual HashSet<XbmcMovieActor> MoviesAsActor { get; set; }
        public virtual HashSet<XbmcMovie> MoviesAsWriter { get; set; }

        internal class Configuration : EntityTypeConfiguration<XbmcPerson> {

            public Configuration() {
                HasMany(d => d.MoviesAsDirector)
                    .WithMany(m => m.Directors)
                    .Map(m => {
                        m.MapLeftKey("idDirector");
                        m.MapRightKey("idMovie");
                        m.ToTable("directorlinkmovie");
                    });

                HasMany(d => d.MoviesAsWriter)
                    .WithMany(m => m.Writers)
                    .Map(m => {
                        m.MapLeftKey("idWriter");
                        m.MapRightKey("idMovie");
                        m.ToTable("writerlinkmovie");
                    });
            }

        }

    }

}
