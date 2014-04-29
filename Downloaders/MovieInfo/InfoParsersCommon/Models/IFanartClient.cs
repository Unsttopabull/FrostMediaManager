namespace Frost.InfoParsers.Models {

    public interface IFanartClient : IInfoClient {

        IParsedArts GetMovieArtFromTitle(string title, int year);
        IParsedArts GetMovieArtFromTmdbId(string tmdbId);
        IParsedArts GetMovieArtFromImdbId(string imdbId);
    }

}