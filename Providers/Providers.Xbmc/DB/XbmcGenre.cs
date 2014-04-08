using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Xbmc.DB {

    /// <summary>This table list genres of the movies in the XBMC library.</summary>
    [Table("genre")]
    public class XbmcGenre : IGenre {

        /// <summary>Initializes a new instance of the <see cref="XbmcGenre"/> class.</summary>
        public XbmcGenre() {
            Movies = new HashSet<XbmcDbMovie>();
        }

        internal XbmcGenre(IGenre genre) {
            Name = genre.Name;
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
        public HashSet<XbmcDbMovie> Movies { get; set; }

        #region IGenre

        bool IMovieEntity.this[string propertyName] {
            get { return true; }
        }

        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name;
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
