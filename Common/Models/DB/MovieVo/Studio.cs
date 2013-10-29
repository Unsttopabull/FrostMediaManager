using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.MovieVo {
    public class Studio {

        public class StudioConfiguration : EntityTypeConfiguration<Studio> {
            public StudioConfiguration() {
                //Join tabela za Movie <--> Studio
                HasMany(m => m.Movies)
                .WithMany(g => g.Studios)
                .Map(m => {
                    m.ToTable("MovieStudios");
                    m.MapLeftKey("StudioId");
                    m.MapRightKey("MovieId");
                });
            }
        }

        public Studio() {
            Movies = new HashSet<Movie>();
        }

        public Studio(string name)
            : this() {
            Name = name;
        }

        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public static Studio[] GetFromNames(string[] studioNames) {
            int numStudios = studioNames.Length;

            Studio[] studios = new Studio[numStudios];
            for (int i = 0; i < numStudios; i++) {
                studios[i] = new Studio(studioNames[i]);
            }

            return studios;
        }
    }
}