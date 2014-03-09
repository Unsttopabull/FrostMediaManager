using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models;

namespace Frost.Models.Frost.DB {

    /// <summary>Represents a movie award.</summary>
    public class Award : IAward {

        public Award() {
            Movies = new HashSet<Movie>();
        }

        public Award(IAward award) {
            //Contract.Requires<ArgumentNullException>(award.Movies != null);

            Organization = award.Organization;
            IsNomination = award.IsNomination;
            AwardType = award.AwardType;
        }

        [Key]
        public long Id { get; set; }

        public string Organization { get; set; }

        public bool IsNomination { get; set; }

        public string AwardType { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

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
