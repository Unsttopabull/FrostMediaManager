using System.Collections.Generic;
using Frost.InfoParsers.Models.Info;

namespace Frost.InfoParsers.Models {

    public interface IPromotionalVideoClient : IInfoClient {

        IEnumerable<IParsedVideo> GetMovieVideosFromTitle(string title, int year);
        IEnumerable<IParsedVideo> GetMovieVideosFromTmdbId(string tmdbId);
        IEnumerable<IParsedVideo> GetMovieVideosFromImdbId(string imdbId);
         
    }

}