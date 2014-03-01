using Frost.Common.Util.ISO;

namespace Frost.DetectFeatures.Models {

    public class PlotInfo {
        public PlotInfo(string full, string summary, string tagline, ISOLanguageCode language) {
            Full = full;
            Summary = summary;
            Tagline = tagline;
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