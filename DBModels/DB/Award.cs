using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Util;

namespace Frost.Models.Frost.DB {

    /// <summary>Represents a movie award.</summary>
    public class Award {

        public Award() {
            Movies = new ObservableHashSet<Movie>();
        }

        [Key]
        public long Id { get; set; }

        public string Organization { get; set; }

        public bool IsNomination { get; set; }

        public string AwardType { get; set; }

        public virtual ObservableHashSet<Movie> Movies { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("Organization: {0},{1} AwardType: {2}", Organization, IsNomination ? " Nomination," : "", AwardType);
        }

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
