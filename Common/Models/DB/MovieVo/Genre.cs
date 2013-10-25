using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.DB.MovieVo {

    public class Genre {

        public Genre() {
            Movies = new HashSet<Movie>();
        }

        public Genre(string name) : this() {
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
