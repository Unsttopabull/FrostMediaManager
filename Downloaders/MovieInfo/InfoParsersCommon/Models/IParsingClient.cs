using System.Collections.Generic;

namespace Frost.InfoParsers.Models {

    public interface IParsingClient : IInfoClient {
        bool CanIndex { get; }
        bool SupportsMovieHash { get; }
        IEnumerable<ParsedMovie> AvailableMovies { get; }
        IEnumerable<ParsedMovie> GetByImdbId(string imdbId);
        IEnumerable<ParsedMovie> GetByMovieHash(IEnumerable<string> movieHashes);
        IEnumerable<ParsedMovie> GetByTitle(string title, int releaseYear);
        void Index();
        ParsedMovieInfo ParseMovieInfo(ParsedMovie movie);
    }

}