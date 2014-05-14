using Frost.Common.Util.ISO;

namespace Frost.Common.Models.FeatureDetector {

    /// <summary>The information about a movie plot as detected by Feature Detector</summary>
    public class PlotInfo {

        /// <summary>Initializes a new instance of the <see cref="PlotInfo"/> class.</summary>
        /// <param name="full">The movie full description.</param>
        /// <param name="summary">The movie plot summary.</param>
        /// <param name="tagline">The movie tagline.</param>
        /// <param name="language">The plot language.</param>
        public PlotInfo(string full, string summary, string tagline, ISOLanguageCode language) {
            Full = full;

            if (!string.IsNullOrEmpty(summary)) {
                Summary = summary;
            }

            if (!string.IsNullOrEmpty(tagline)) {
                Tagline = tagline;
            }
            Language = language;
        }

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
        public ISOLanguageCode Language { get; set; }
    }

}