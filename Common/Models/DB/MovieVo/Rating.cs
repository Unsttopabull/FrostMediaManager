using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo {

    public class Rating {

        public Rating(string critic, double value) {
            Critic = critic;
            Value = value;
        }

        [Key]
        public long Id { get; set; }

        public string Critic { get; set; }

        public double Value { get; set; }

        public long MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
    }
}
