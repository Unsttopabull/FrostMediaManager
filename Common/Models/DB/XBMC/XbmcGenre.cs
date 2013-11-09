using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.XBMC {

    /// <summary>This table list genres of the movies in the XBMC library.</summary>
    [Table("genre")]
    public class XbmcGenre : IEquatable<XbmcGenre> {

        /// <summary>Initializes a new instance of the <see cref="XbmcGenre"/> class.</summary>
        public XbmcGenre() {
            Movies = new HashSet<XbmcMovie>();
        }

        /// <summary>Gets or sets the database Genre Id.</summary>
        /// <value>The database genre Id</value>
        [Key]
        [Column("idGenre")]
        public long Id { get; set; }

        /// <summary>Gets or sets the genre name.</summary>
        /// <value>The name of the genre.</value>
        [Column("strGenre")]
        public string Name { get; set; }

        /// <summary>Gets or sets the movies of this genre.</summary>
        /// <value>The movies of this genre.</value>
        public HashSet<XbmcMovie> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcGenre other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }
            return Name == other.Name;
        }

        internal class Configuration : EntityTypeConfiguration<XbmcGenre> {

            public Configuration() {
                //Join table Movie <--> Genre
                HasMany(m => m.Movies)
                    .WithMany(g => g.Genres)
                    .Map(m => {
                        m.ToTable("genrelinkmovie");
                        m.MapLeftKey("idGenre");
                        m.MapRightKey("idMovie");
                    });
            }

        }

    }

}
