using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Frost.DB {


    /// <summary>Contains information about movie story/plot.</summary>
    [Table("Plots")]
    public class Plot : IPlot {

        public Plot() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="Plot"/> class.</summary>
        /// <param name="full">The full plot.</param>
        /// <param name="summary">A short story summary, the plot outline</param>
        /// <param name="tagline">The tagline (short promotional slogan / one-liner / clarification).</param>
        /// <param name="language">The language of the plot.</param>
        public Plot(string full, string summary, string tagline, string language) {
            Tagline = tagline;
            Summary = summary;
            Full = full;
            Language = language;
        }

        /// <summary>Initializes a new instance of the <see cref="Plot"/> class.</summary>
        /// <param name="fullPlot">The full plot.</param>
        /// <param name="language">The language of the plot.</param>
        public Plot(string fullPlot, string language) {
            Full = fullPlot;
            Language = language;
        }

        /// <summary>Initializes a new instance of the <see cref="Plot"/> class.</summary>
        /// <param name="fullPlot">The full plot.</param>
        /// <param name="summary">A short story summary, the plot outline</param>
        /// <param name="lanugage">The language of the plot.</param>
        public Plot(string fullPlot, string summary, string lanugage) : this(fullPlot, lanugage) {
            if (!string.IsNullOrEmpty(summary)) {
                Summary = summary;
            }
        }

        internal Plot(IPlot plot) {
            //Contract.Requires<ArgumentNullException>(plot != null);

            Tagline = plot.Tagline;
            Summary = plot.Summary;
            Full = plot.Full;
            Language = plot.Language;
        }

        /// <summary>Gets or sets the database plot Id.</summary>
        /// <value>The database plot Id</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the tagline (short one-liner).</summary>
        /// <value>The tagline (short promotional slogan / one-liner / clarification).</value>
        public string Tagline { get; set; }

        /// <summary>Gets or sets the story summary.</summary>
        /// <value>A short story summary, the plot outline</value>
        public string Summary { get; set; }

        /// <summary>Gets or sets the full plot.</summary>
        /// <value>The full plot.</value>
        public string Full { get; set; }

        /// <summary>Gets or sets the language of this plot.</summary>
        /// <value>The language of this plot.</value>
        public string Language { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long? MovieId { get; set; }

        /// <summary>Gets or sets the movie this plot belongs to.</summary>
        /// <value>Gets or sets the movie this plot belongs to.</value>
        //[ForeignKey("MovieId")]
        [InverseProperty("Plots")]
        public virtual Movie Movie { get; set; }

        [NotMapped]
        public string TaglineOrSummary {
            get {
                if (string.IsNullOrEmpty(Tagline)) {
                    return Summary;
                }
                return Tagline;
            }
        }

        bool IMovieEntity.this[string propertyName] {
            get { return true; }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            if (!string.IsNullOrEmpty(Language)) {
                return Language;
            }
            return Id+" Unknown language";
        }

        internal class Configuration : EntityTypeConfiguration<Plot> {
            public Configuration() {
                HasRequired(p => p.Movie)
                    .WithMany(m => m.Plots)
                    .HasForeignKey(p => p.MovieId)
                    .WillCascadeOnDelete();
            }
        }
    }

}
