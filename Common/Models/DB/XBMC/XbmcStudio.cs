using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.XBMC {

    /// <summary>Represents a studio that prodcuced a movie.</summary>
    [Table("studio")]
    public class XbmcStudio : IEquatable<XbmcStudio> {

        /// <summary>Initializes a new instance of the <see cref="XbmcStudio"/> class.</summary>
        public XbmcStudio() {
            Movies = new HashSet<XbmcMovie>();
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
        public HashSet<XbmcMovie> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcStudio other) {
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
