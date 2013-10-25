using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.Jukebox {

    public class MoviesGenres
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("movie_id")]
        public long? MovieID { get; set; }

        [Column("genre_id")]
        public string GenreID { get; set; }
    }
}
