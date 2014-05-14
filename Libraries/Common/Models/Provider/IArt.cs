namespace Frost.Common.Models.Provider {

    /// <summary>Represents the information about an art that is accessible by the UI.</summary>
    public interface IArt : IMovieEntity {

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        string Path { get; set; }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        string Preview { get; set; }

        /// <summary>Gets or sets the type of the art.</summary>
        /// <value>The type of the art.</value>
        ArtType Type { get; }

        /// <summary>Gets the preview if it exists or the full path.</summary>
        /// <value>The preview or path.</value>
        string PreviewOrPath { get; }
    }

}