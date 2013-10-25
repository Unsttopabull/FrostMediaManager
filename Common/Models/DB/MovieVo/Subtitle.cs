using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo {
    public class Subtitle {
        public Subtitle() {
            Movie = new Movie();
        }

        public Subtitle(string language) : this() {
            Language = language;
        }

        public Subtitle(string language, string fileName) : this(language) {
            FileName = fileName;
        }

        [Key]
        public long Id;

        public string FileName { get; set; }

        public string Language { get; set; }

        public long MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
    }
}