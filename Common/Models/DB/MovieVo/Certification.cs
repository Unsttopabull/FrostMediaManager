using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo {

    public class Certification {

        public Certification(string country, string rating) {
            Country = country;
            Rating = rating;
        }

        [Key]
        public long Id { get; set; }

        public string Country { get; set; }

        public string Rating { get; set; }

        public long MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        public override string ToString() {
            return Country + ":" + Rating;
        }
    }
}
