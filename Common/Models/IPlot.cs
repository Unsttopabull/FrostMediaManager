namespace Frost.Common.Models {

    public interface IPlot : IMovieEntity {
        /// <summary>Gets or sets the tagline (short one-liner).</summary>
        /// <value>The tagline (short promotional slogan / one-liner / clarification).</value>
        string Tagline { get; set; }

        /// <summary>Gets or sets the story summary.</summary>
        /// <value>A short story summary, the plot outline</value>
        string Summary { get; set; }

        /// <summary>Gets or sets the full plot.</summary>
        /// <value>The full plot.</value>
        string Full { get; set; }

        /// <summary>Gets or sets the language of this plot.</summary>
        /// <value>The language of this plot.</value>
        string Language { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        long MovieId { get; set; }

        /// <summary>Gets or sets the movie this plot belongs to.</summary>
        /// <value>Gets or sets the movie this plot belongs to.</value>
        IMovie Movie { get; set; }
    }

    public interface IPlot<TMovie> : IPlot where TMovie : IMovie {
        new TMovie Movie { get; set; }
    }

}