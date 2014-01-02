using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Frost.Common.Models.DB.Jukebox;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>Represents a movie genre.</summary>
    public partial class Genre : IEquatable<Genre> {

        /// <summary>Initializes a new instance of the <see cref="Genre"/> class.</summary>
        public Genre() {
            Movies = new HashSet<Movie>();
        }

        /// <summary>Initializes a new instance of the <see cref="Genre"/> class.</summary>
        /// <param name="name">The name of the genre.</param>
        public Genre(string name) : this() {
            Name = name;
        }

        /// <summary>Gets or sets the database Genre Id.</summary>
        /// <value>The database genre Id</value>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the genre name.</summary>
        /// <value>The name of the genre.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the movies of this genre.</summary>
        /// <value>The movies of this genre.</value>
        public virtual HashSet<Movie> Movies { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Genre other) {
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

        /// <summary>Converts genre names to an <see cref="IEnumerable{T}"/> with elements of type <see cref="Genre"/></summary>
        /// <param name="genreNames">The genre names.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Genre"/> instances with specified genre names</returns>
        public static IEnumerable<Genre> GetFromNames(IEnumerable<string> genreNames) {
            return genreNames.Select(genreName => new Genre(genreName));
        }

        /// <summary>Converts the genre name to a <see cref="Genre"/> instance</summary>
        /// <param name="genreName">Name of the genre.</param>
        /// <returns>An instance of <see cref="Genre"/> with string as a genre name</returns>
        public static implicit operator Genre(string genreName) {
            return new Genre(genreName);
        }

        /// <summary>Converts a <see cref="Genre"/> instance to an instance of <see cref="Frost.Common.Models.DB.Jukebox.XjbGenre">XjbGenre</see>.</summary>
        /// <param name="genre">The genre to convert.</param>
        /// <returns>An instance of <see cref="Frost.Common.Models.DB.Jukebox.XjbGenre">XjbGenre</see> converted from <see cref="Genre"/>.</returns>
        public static explicit operator XjbGenre(Genre genre) {
            string genreName = genre.Name.ToLower();

            return GenreTags.ContainsKey(genreName)
                    ? new XjbGenre(GenreTags[genreName])
                    : new XjbGenre(genre.Name);
        }

        internal class GenreConfiguration : EntityTypeConfiguration<Genre> {

            public GenreConfiguration() {
                //Join table for Movie <--> Genre
                HasMany(m => m.Movies)
                    .WithMany(g => g.Genres)
                    .Map(m => {
                        m.ToTable("MovieGenres");
                        m.MapLeftKey("GenreId");
                        m.MapRightKey("MovieId");
                    });
            }

        }

    }

}
