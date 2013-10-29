using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo {
    public class Subtitle {

        public Subtitle() {
            Movie = new Movie();
            File = new File();
        }

        public Subtitle(string language) : this() {
            Language = language;
        }

        [Key]
        public long Id { get; set; }

        public string Language { get; set; }

        public long MovieId { get; set; }

        public long FileId { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
    }
}