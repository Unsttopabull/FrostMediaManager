using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.MovieVo {
    public class Special {

        public class SpecialConfiguration : EntityTypeConfiguration<Special> {

            public SpecialConfiguration() {
                HasMany(s => s.Movies)
                .WithMany(m => m.Specials)
                .Map(m => {
                    m.ToTable("MovieSpecials");
                    m.MapLeftKey("SpecialId");
                    m.MapRightKey("MovieId");
                });
            }
        }

        public Special(string value) {
            Value = value;
        }

        public long Id { get; set; }

        public string Value { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}