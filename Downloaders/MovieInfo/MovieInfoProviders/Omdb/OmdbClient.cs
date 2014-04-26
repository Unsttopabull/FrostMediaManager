using System;
using System.Collections.Generic;
using System.Linq;
using Frost.InfoParsers;
using SharpOmdbAPI;
using SharpOmdbAPI.Models;

namespace Frost.MovieInfoProviders.Omdb {

    public class OmdbClient : ParsingClient {
        private const string IMDB_MOVIE_URL = "http://www.imdb.com/title/{0}/";

        public OmdbClient() : base("OMDB", false, false) {
        }

        public override IEnumerable<ParsedMovie> GetByImdbId(string imdbId) {
            OmdbMovie movie = SharpOmdbClient.GetByImdbId(imdbId, PlotLength.Full);
            return new[] { new OmdbParsedMovie(movie) };
        }

        public override IEnumerable<ParsedMovie> GetByMovieHash(IEnumerable<string> movieHashes) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ParsedMovie> GetByTitle(string title, int releaseYear) {
            IEnumerable<OmdbSearch> search = SharpOmdbClient.Search(title, releaseYear);
            return search.Select(s => new OmdbParsedMovie(s));
        }

        public override void Index() {
            throw new NotImplementedException();
        }

        public override ParsedMovieInfo ParseMovieInfo(ParsedMovie movie) {
            if (movie is OmdbParsedMovie) {
                OmdbParsedMovie omdbMovie = movie as OmdbParsedMovie;
                if (omdbMovie.Movie != null) {
                    return ToParsedMovieInfo(omdbMovie.Movie);
                }

                OmdbMovie mov;
                if (omdbMovie.Search != null && !string.IsNullOrEmpty(omdbMovie.Search.ImdbId)) {
                    mov = SharpOmdbClient.GetByImdbId(omdbMovie.Search.ImdbId, PlotLength.Full);
                }
                else {
                    mov = SharpOmdbClient.GetByTitle(movie.OriginalName, movie.ReleaseYear, PlotLength.Full);
                }

                return ToParsedMovieInfo(mov);
            }
            return null;
        }

        private ParsedMovieInfo ToParsedMovieInfo(OmdbMovie movie) {
            ParsedMovieInfo movieInfo = new ParsedMovieInfo();

            movieInfo.ReleaseYear = movie.Year;
            movieInfo.Duration = movie.Runtime;
            movieInfo.Directors = movie.Director.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            movieInfo.Writers = movie.Writer.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            movieInfo.Writers = movie.Writer.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            movieInfo.Actors = movie.Actors.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            movieInfo.Genres = movie.Genre.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            movieInfo.Plot = movie.Plot;
            movieInfo.Cover = movie.Poster;
            movieInfo.ImdbRating = movie.ImdbRating;

            if (!string.IsNullOrEmpty(movie.ImdbId)) {
                movieInfo.ImdbLink = string.Format(IMDB_MOVIE_URL, movie.ImdbId);
            }

            movieInfo.Country = movie.Country.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            movieInfo.Language = movie.Language;

            return movieInfo;
        }
    }

}