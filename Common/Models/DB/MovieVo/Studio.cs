using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>Represents a studio that prodcuced a movie.</summary>
    public class Studio : IEquatable<Studio> {

        /// <summary>Initializes a new instance of the <see cref="Studio"/> class.</summary>
        public Studio() {
            Movies = new HashSet<Movie>();
        }

        /// <summary>Initializes a new instance of the <see cref="Studio"/> class with the specified studio name.</summary>
        /// <param name="name">The name of the studio.</param>
        public Studio(string name) : this() {
            Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="Studio" /> class with specified name and an URI to the logo image.</summary>
        /// <param name="name">The name of the studio.</param>
        /// <param name="logo">The URI to the studio's logo image</param>
        public Studio(string name, string logo) : this(name) {
            Logo = logo;
        }

        /// <summary>Gets or sets the Id of this studio in the database.</summary>
        /// <value>The Id of this studio in the database</value>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of the studio.</summary>
        /// <value>The name of the studio.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the URI to the studio's logo image.</summary>
        /// <value>The URI to the studio's logo image</value>
        public string Logo { get; set; }

        /// <summary>Gets or sets the movies this studio has produced.</summary>
        /// <value>The movies this studio has produced.</value>
        public virtual HashSet<Movie> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Studio other) {
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

        /// <summary>Converts studio names to an <see cref="IEnumerable{T}"/> with elements of type <see cref="Studio"/></summary>
        /// <param name="studioNames">The studio names.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Studio"/> instances with specified studio names</returns>
        public static IEnumerable<Studio> GetFromNames(IEnumerable<string> studioNames) {
            return studioNames.Select(studioName => new Studio(studioName));
        }

        /// <summary>Converts the studio name to a <see cref="Studio"/> instance</summary>
        /// <param name="studioName">Name of the studio.</param>
        /// <returns>An instance of <see cref="Studio"/> with string as a studio name</returns>
        public static implicit operator Studio(string studioName) {
            return new Studio(studioName);
        }

        internal class Configuration : EntityTypeConfiguration<Studio> {

            public Configuration() {
                //Join tabela za Movie <--> Studio
                HasMany(m => m.Movies)
                    .WithMany(g => g.Studios)
                    .Map(m => {
                        m.ToTable("MovieStudios");
                        m.MapLeftKey("StudioId");
                        m.MapRightKey("MovieId");
                    });
            }

        }

    }

}
