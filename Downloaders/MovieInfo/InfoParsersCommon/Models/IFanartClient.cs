namespace Frost.InfoParsers.Models {

    public interface IFanartClient {

        IParsedArts GetMovieArtFromTitle(string title);
        IParsedArts GetMovieArtFromTmdbId(string tmdbId);
        IParsedArts GetMovieArtFromImdbId(string imdbId);
    }

}