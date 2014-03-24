using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Xbmc.DB.Actor {

    /// <summary>Represents a person that worked on a movie.</summary>
    [Table("actors")]
    public class XbmcPerson : IPerson {

        public XbmcPerson() {
            MoviesAsDirector = new HashSet<XbmcDbMovie>();
            MoviesAsActor = new HashSet<XbmcMovieActor>();
            MoviesAsWriter = new HashSet<XbmcDbMovie>();
        }

        public XbmcPerson(string name, string thumbXml) : this() {
            Name = name;
            ThumbXml = thumbXml;
        }

        internal XbmcPerson(long id, string name, string thumbXml) : this(name, thumbXml) {
            Id = id;
        }

        internal XbmcPerson(IPerson person) {
            Name = person.Name;
            ThumbURL = person.Thumb;
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
        public virtual HashSet<XbmcDbMovie> MoviesAsDirector { get; set; }

        /// <summary>Gets or sets movies where this person was an actor.</summary>
        /// <value>The movies where this person was an actor.</value>
        public virtual HashSet<XbmcMovieActor> MoviesAsActor { get; set; }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        public virtual HashSet<XbmcDbMovie> MoviesAsWriter { get; set; }

        #region IPerson

        bool IMovieEntity.this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Thumb":
                    case "Id":
                    case "Name":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        string IPerson.Thumb {
            get {
                string thumb = ThumbURL;
                return string.IsNullOrEmpty(thumb) ? null : ThumbURL;
            }
            set { ThumbURL = value; }
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        string IPerson.ImdbID {
            get { return default(string); }
            set { }
        }

        #endregion

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
