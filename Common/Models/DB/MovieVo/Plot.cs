using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo {

    public class Plot{
        public Plot(string full, string summary, string tagline, string language) {
            Tagline = tagline;
            Summary = summary;
            Full = full;
            Language = language;
        }

        public Plot(string fullPlot, string language) {
            Full = fullPlot;
            Language = language;
        }

        public Plot(string fullPlot, string summary, string lanugage) : this(fullPlot, lanugage) {
            if (!string.IsNullOrEmpty(summary)) {
                Summary = summary;
            }
        }

        [Key]
        public long Id { get; set; }

        public string Tagline { get; set; }

        public string Summary { get; set; }

        public string Full { get; set; }

        public string Language { get; set; }

        public long MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
    }
}
