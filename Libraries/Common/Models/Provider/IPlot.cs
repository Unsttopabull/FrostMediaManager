namespace Frost.Common.Models.Provider {

    /// <summary>Represents the information about a movie plot that is accessible by the UI</summary>
    public interface IPlot : IMovieEntity {

        /// <summary>Gets or sets the tagline/motto (short one-liner).</summary>
        /// <value>The tagline (short promotional slogan / one-liner / clarification).</value>
        string Tagline { get; set; }

        /// <summary>Gets or sets the story summary.</summary>
        /// <value>A short story summary, the plot outline</value>
        string Summary { get; set; }

        /// <summary>Gets or sets the plot full description.</summary>
        /// <value>The full plot.</value>
        string Full { get; set; }

        /// <summary>Gets or sets the language of this plot.</summary>
        /// <value>The language of this plot.</value>
        string Language { get; set; }
    }

}