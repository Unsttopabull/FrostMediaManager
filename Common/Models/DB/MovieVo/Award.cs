using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Util;

namespace Frost.Common.Models.DB.MovieVo {
    public class Award {

        public Award() {
            Movies = new ObservableHashSet<Movie>();
        }

        public long Id { get; set; }

        public string Organization { get; set; }

        public bool IsNomination { get; set; }

        public string AwardType { get; set; }

        public virtual ObservableHashSet<Movie> Movies { get; set; }

        internal class Configuration : EntityTypeConfiguration<Award> {

            public Configuration() {
                ToTable("Awards");

                //Join table for Movie <--> Genre
                HasMany(m => m.Movies)
                    .WithMany(g => g.Awards)
                    .Map(m => {
                        m.ToTable("MovieAwards");
                        m.MapLeftKey("AwardId");
                        m.MapRightKey("MovieId");
                    });
            }

        }
    }
}
