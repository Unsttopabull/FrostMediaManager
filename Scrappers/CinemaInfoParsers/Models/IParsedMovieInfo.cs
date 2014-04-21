using System.Collections.Generic;

namespace Frost.InfoParsers.Models {

    public interface IParsedMovieInfo {
        bool IsFinished { get; }
        string Distribution { get; }
        string Duration { get; }
        string ReleaseYear { get; }
        string Country { get; }
        string Language { get; }
        IEnumerable<string> Writers { get; }
        IEnumerable<string> Directors { get; }
        IEnumerable<string> Actors { get; }
        string OfficialSite { get; }
        string ImdbLink { get; }
        string ImdbRating { get; }
        string Summary { get; }
        string TrailerUrl { get; }
        ICollection<IParsedVideo> Videos { get; }
        ParsedAward Awards { get; }
        IEnumerable<string> Genres { get; }
    }

}