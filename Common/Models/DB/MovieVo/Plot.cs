using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>Contains information about movie story/plot.</summary>
    [Table("Plots")]
    public class Plot : IEquatable<Plot> {

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

        /// <summary>Gets or sets the database plot Id.</summary>
        /// <value>The database plot Id</value>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
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
        public long MovieId { get; set; }

        /// <summary>Gets or sets the movie this plot belongs to.</summary>
        /// <value>Gets or sets the movie this plot belongs to.</value>
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Plot other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Tagline == other.Tagline &&
                   Summary == other.Summary &&
                   Full == other.Full &&
                   Language == other.Language;
        }

    }

}
