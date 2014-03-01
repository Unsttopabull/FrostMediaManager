using System.Data.Entity.ModelConfiguration;

namespace Frost.Models.Frost.DB {

    public enum VideoType {
        Unknown,
        Trailer,
        Interview,
        Featurete,
        BehindTheScenes,
        TvSpot,
        Review,
        Clip
    }

    public class PromotionalVideo {

        public long Id { get; set; }
        public VideoType Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Duration { get; set; }
        public string Language { get; set; }
        public string SubtitleLanguage { get; set; }

        public long MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0}: {1} ({2}{3})", Type, Title, Language, !string.IsNullOrEmpty(SubtitleLanguage) ? ", subs: " + SubtitleLanguage : "");
        }

        internal class Configuration : EntityTypeConfiguration<PromotionalVideo> {

            public Configuration() {
                ToTable("PromotionalVideos");

                //Join table for Movie <--> PromotionalVideo
                HasRequired(m => m.Movie)
                    .WithMany(g => g.PromotionalVideos)
                    .HasForeignKey(pv => pv.MovieId)
                    .WillCascadeOnDelete();
            }

        }
    }

}