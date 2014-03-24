using System.Data.Entity.ModelConfiguration;
using Frost.Common;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Frost.DB {

    public class PromotionalVideo : IPromotionalVideo {

        public PromotionalVideo() {
            
        }

        internal PromotionalVideo(IPromotionalVideo promotionalVideo) {
            //Contract.Requires<ArgumentNullException>(promotionalVideo != null);

            Type = promotionalVideo.Type;
            Title = promotionalVideo.Title;
            Url = promotionalVideo.Url;
            Duration = promotionalVideo.Duration;
            Language = promotionalVideo.Language;
            SubtitleLanguage = promotionalVideo.Language;
        }

        public long Id { get; set; }
        public PromotionalVideoType Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Duration { get; set; }
        public string Language { get; set; }
        public string SubtitleLanguage { get; set; }

        public long MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        bool IMovieEntity.this[string propertyName] {
            get { return true; }
        }

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