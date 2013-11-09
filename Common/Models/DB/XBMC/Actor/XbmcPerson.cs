using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Frost.Common.Models.DB.XBMC.Actor {

    /// <summary>Represents a person that worked on a movie.</summary>
    [Table("actors")]
    public class XbmcPerson : IEquatable<XbmcPerson> {

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

        /// <summary>Gets or sets the database person Id.</summary>
        /// <value>The database person Id</value>
        [Key]
        [Column("idActor")]
        public long Id { get; set; }

        /// <summary>Gets or sets the full name of the actor.</summary>
        /// <value>The full name of the actor.</value>
        [Column("strActor")]
        public string Name { get; set; }

        /// <summary>Gets or sets the persons thumbnail image in a serialized xml format.</summary>
        /// <value>The thumbnail image in a serialized xml format.</value>
        [Column("strThumb")]
        public string ThumbXml { get; set; }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        [NotMapped]
        public string ThumbURL {
            get {
                return ThumbXml != null
                    ? ThumbXml.Replace("<thumb>", "").Replace("</thumb>", "")
                    : null;
            }
            set { ThumbXml = "<thumb>" + value + "</thumb>"; }
        }

        /// <summary>Gets or sets movies where this person was a director.</summary>
        /// <value>The movies where this person was a director.</value>
        public virtual HashSet<XbmcMovie> MoviesAsDirector { get; set; }

        /// <summary>Gets or sets movies where this person was an actor.</summary>
        /// <value>The movies where this person was an actor.</value>
        public virtual HashSet<XbmcMovieActor> MoviesAsActor { get; set; }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        public virtual HashSet<XbmcMovie> MoviesAsWriter { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcPerson other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Name == other.Name &&
                   ThumbXml == other.ThumbXml;
        }

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
