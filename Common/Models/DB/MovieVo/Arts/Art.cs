using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.MovieVo.Arts {

    public class Art {

        public class ArtConfiguration : EntityTypeConfiguration<Art> {

            public ArtConfiguration() {
                Map<Cover>(m => m.Requires("Type").HasValue(1));
                Map<Poster>(m => m.Requires("Type").HasValue(2));
                Map<Fanart>(m => m.Requires("Type").HasValue(3));
            }
        }

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
