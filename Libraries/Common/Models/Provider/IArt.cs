namespace Frost.Common.Models.Provider {

    public interface IArt : IMovieEntity {

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        string Path { get; set; }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        string Preview { get; set; }

        ///// <summary>Gets or sets the movie this art is for.</summary>
        ///// <value>The movie this art is for</value>
        //IMovie Movie { get; set; }

        ArtType Type { get; }

        string PreviewOrPath { get; }
    }

}