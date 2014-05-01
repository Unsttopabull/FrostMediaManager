using System.Collections.Generic;

namespace Frost.InfoParsers.Models {

    public interface IPromotionalVideoClient : IInfoClient {

        IEnumerable<IParsedVideo> GetMovieArtFromTitle(string title, int year);
        IEnumerable<IParsedVideo> GetMovieArtFromTmdbId(string tmdbId);
        IEnumerable<IParsedVideo> GetMovieArtFromImdbId(string imdbId);
         
    }

}