using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.Jukebox {

    public class MoviesPersons
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("movie_id")]
        public long? MovieID { get; set; }

        [Column("person_id")]
        public long? PersonID { get; set; }

        [Column("character")]
        public string Character { get; set; }

        [Column("job")]
        public string Job { get; set; }
    }
}
