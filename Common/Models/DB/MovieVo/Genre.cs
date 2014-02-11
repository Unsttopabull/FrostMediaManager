using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Globalization;
using System.Linq;
using Frost.Common.Models.DB.Jukebox;
using Frost.Common.Util;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>Represents a movie genre.</summary>
    public partial class Genre {

        /// <summary>Initializes a new instance of the <see cref="Genre"/> class.</summary>
        public Genre() {
            Movies = new ObservableHashSet<Movie>();
        }

        /// <summary>Initializes a new instance of the <see cref="Genre"/> class.</summary>
        /// <param name="name">The name of the genre.</param>
        public Genre(string name) : this() {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            Name = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name);
        }

        /// <summary>Gets or sets the database Genre Id.</summary>
        /// <value>The database genre Id</value>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the genre name.</summary>
        /// <value>The name of the genre.</value>
        [Required]
        public string Name { get; set; }

        /// <summary>Gets or sets the movies of this genre.</summary>
        /// <value>The movies of this genre.</value>
        public virtual ObservableHashSet<Movie> Movies { get; set; }

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

        internal class Configuration : EntityTypeConfiguration<Genre> {

            public Configuration() {
                ToTable("Genres");

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
