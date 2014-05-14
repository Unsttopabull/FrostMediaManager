using System.Collections.Generic;
using Frost.Common.Models.Provider;

namespace Frost.Common.Models {

    /// <summary>The common base movie information used with web updater plugins.</summary>
    public interface IMovieInfo {

        /// <summary>Gets or sets the movie title.</summary>
        /// <value>The movie title.</value>
        string Title { get; }

        /// <summary>Gets or sets the original movie title.</summary>
        /// <value>The original movie title.</value>
        string OriginalTitle { get; set; }

        /// <summary>Gets the year the movie was released in.</summary>
        /// <value>The movie release year.</value>
        long? ReleaseYear { get; }

        /// <summary>Gets the imdb identifier of the movie (eg. tt1234567).</summary>
        /// <value>The imdb identifier.</value>
        string ImdbID { get; }

        /// <summary>Gets the TMDB identifier.</summary>
        /// <value>The TMDB identifier.</value>
        string TmdbID { get; }

        /// <summary>Gets the movie hashes of the video files.</summary>
        /// <value>The movie hashes.</value>
        IEnumerable<string> MovieHashes { get; }

        /// <summary>Adds the art to add to the movie.</summary>
        /// <param name="art">The art to add.</param>
        /// <param name="silent">if set to <c>true</c> it does not show any message boxes.</param>
        void AddArt(IArt art, bool silent = false);

        /// <summary>Adds the promotional video to add to the movie.</summary>
        /// <param name="video">The promotional video to add.</param>
        /// <param name="silent">if set to <c>true</c> it does not show any message boxes.</param>
        void AddPromotionalVideo(IPromotionalVideo video, bool silent = false);

    }

}