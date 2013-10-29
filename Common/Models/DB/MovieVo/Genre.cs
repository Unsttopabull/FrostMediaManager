using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.MovieVo {

    public class Genre {

        public class GenreConfiguration : EntityTypeConfiguration<Genre> {
            public GenreConfiguration() {
                //Join tabela za Movie <--> Genre
                HasMany(m => m.Movies)
                .WithMany(g => g.Genres)
                .Map(m => {
                    m.ToTable("MovieGenres");
                    m.MapLeftKey("GenreId");
                    m.MapRightKey("MovieId");
                });
            }
        }

        public Genre() {
            Movies = new HashSet<Movie>();
        }

        public Genre(string name)
            : this() {
            Name = name;
        }

        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public static Genre[] GetFromNames(string[] genreNames) {
            int genreLength = genreNames.Length;
            Genre[] genres = new Genre[genreLength];

            for (int i = 0; i < genreLength; i++) {
                genres[i] = genreNames[i];
            }
            return genres;
        }

        public static implicit operator Genre(string genreName) {
            return new Genre(genreName);
        }
    }
}
