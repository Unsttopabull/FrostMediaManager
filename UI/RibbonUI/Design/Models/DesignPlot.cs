using Frost.Common.Models.Provider;

namespace Frost.RibbonUI.Design.Models {
    public class DesignPlot : IPlot{
        public long Id { get; private set; }

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

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Tagline":
                    case "Summary":
                    case "Full":
                    case "Language":
                        return true;
                    default:
                        return false;
                }
            }
        }

    }
}
