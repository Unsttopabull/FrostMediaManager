using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models;

namespace Frost.Providers.Xbmc.DB {

    /// <summary>Represents a studio that prodcuced a movie.</summary>
    [Table("studio")]
    public class XbmcStudio : IStudio {

        /// <summary>Initializes a new instance of the <see cref="XbmcStudio"/> class.</summary>
        public XbmcStudio() {
            Movies = new HashSet<XbmcDbMovie>();
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcStudio"/> class.</summary>
        /// <param name="name">The name of this studio</param>
        public XbmcStudio(string name) {
            Name = name;
        }

        /// <summary>Gets or sets the Id of this studio in the database.</summary>
        /// <value>The Id of this studio in the database</value>
        [Key]
        [Column("idStudio")]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of this studio.</summary>
        /// <value>The name of this studio</value>
        [Column("strStudio")]
        public string Name { get; set; }

        /// <summary>Gets or sets the movies this studio has produced.</summary>
        /// <value>The movies this studio has produced.</value>
        public HashSet<XbmcDbMovie> Movies { get; set; }

        public bool this[string propertyName] {
            get { return true; }
        }

        internal class Configuration : EntityTypeConfiguration<XbmcStudio> {

            public Configuration() {
                //Join table Movie <--> Country
                HasMany(m => m.Movies)
                    .WithMany(s => s.Studios)
                    .Map(m => {
                        m.ToTable("studiolinkmovie");
                        m.MapLeftKey("idStudio");
                        m.MapRightKey("idMovie");
                    });
            }

        }

    }

}
