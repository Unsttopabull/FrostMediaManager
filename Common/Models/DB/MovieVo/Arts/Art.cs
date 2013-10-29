using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo.Arts {

    public class Art {

        public Art(string path) {
            Path = path;

            Movie = new Movie();
        }

        [Key]
        public long Id { get; set; }

        public string Path { get; set; }

        public string Preview { get; set; }

        public long MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
    }
}
