using System.Collections.Generic;
using Frost.InfoParsers.Models;

namespace SharpFanartTv {

    public interface IFanartClient {

        IParsedArts GetMovieArtFromTitle(string title);
        IParsedArts GetMovieArtFromTmdbId(string tmdbId);
        IParsedArts GetMovieArtFromImdbId(string imdbId);
    }

}