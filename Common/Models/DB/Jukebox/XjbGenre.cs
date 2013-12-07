using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models.DB.MovieVo;

namespace Frost.Common.Models.DB.Jukebox {

    /// <summary>Represents a Xtreamer Movie Jukebox genre.</summary>
    [Table("genres")]
    public partial class XjbGenre : IEquatable<XjbGenre> {

        /// <summary>Initializes a new instance of the <see cref="XjbGenre"/> class.</summary>
        /// <param name="name">The name of the genre abbreviation.</param>
        public XjbGenre(string name) {
            Name = name;
        }

        /// <summary>Gets or sets the Id of this option in the database.</summary>
        /// <value>The Id of this option in the database</value>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>Gets or sets the genre 4 letter abbreviation.</summary>
        /// <value>The name of the genre 4 letter abbreviation.</value>
        /// <example>\eg{ <c>"docu"</c> for documentary, <c>"come"</c> for comedy.}</example>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the movies of this genre in Xtreamer Movie Jukebox library.</summary>
        /// <value>The movies of this genre in Xtreamer Movie Jukebox library</value>
        public virtual HashSet<XjbMovie> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XjbGenre other) {
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

        /// <summary>Converts a <see cref="XjbGenre"/> instance to an instance of <see cref="Frost.Common.Models.DB.MovieVo.Genre">Genre</see>.</summary>
        /// <param name="genre">The genre to convert.</param>
        /// <returns>An instance of <see cref="Frost.Common.Models.DB.MovieVo.Genre">Genre</see> converted from <see cref="XjbGenre"/>.</returns>
        public static explicit operator Genre(XjbGenre genre) {
            return GenreTags.ContainsKey(genre.Name)
                    ? new Genre(GenreTags[genre.Name])
                    : new Genre(genre.Name);
        }

        internal class Configuration : EntityTypeConfiguration<XjbGenre> {

            public Configuration() {
                HasMany(g => g.Movies)
                    .WithMany(m => m.Genres)
                    .Map(m => {
                        m.ToTable("movies_genres");
                        m.MapLeftKey("genre_id");
                        m.MapRightKey("movie_id");
                    });
            }

        }

    }

}
