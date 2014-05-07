using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Frost.InfoParsers;
using SharpOmdbAPI;
using SharpOmdbAPI.Models;

namespace Frost.MovieInfoProviders.Info.OmdbAPI {

    public class OmdbClient : ParsingClient {
        private const string IMDB_MOVIE_URL = "http://www.imdb.com/title/{0}/";
        public const string CLIENT_NAME = "OmdbAPI";

        public OmdbClient() : base(CLIENT_NAME, false, false, true) {
            string directoryName = GetAssemblyCurrentDirectory();

            if (directoryName != null) {
                Icon = new Uri(directoryName + "/omdb.ico");
            }
        }

        public override IEnumerable<ParsedMovie> GetByImdbId(string imdbId) {
            OmdbMovie movie = SharpOmdb.GetByImdbId(imdbId, PlotLength.Full);
            if (movie != null) {
                return new[] { new OmdbParsedMovie(movie) };
            }
            else {
                return null;
            }
        }

        public override IEnumerable<ParsedMovie> GetByMovieHash(IEnumerable<string> movieHashes) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ParsedMovie> GetByTitle(string title, int releaseYear) {
            IEnumerable<OmdbSearch> search = SharpOmdb.Search(title, releaseYear);

            if (search != null) {
                return search.Select(s => new OmdbParsedMovie(s));
            }
            else {
                return null;
            }
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
                    mov = SharpOmdb.GetByImdbId(omdbMovie.Search.ImdbId, PlotLength.Full);
                }
                else {
                    mov = SharpOmdb.GetByTitle(movie.OriginalName, movie.ReleaseYear, PlotLength.Full);
                }

                return ToParsedMovieInfo(mov);
            }
            return null;
        }

        private ParsedMovieInfo ToParsedMovieInfo(OmdbMovie movie) {
            ParsedMovieInfo movieInfo = new ParsedMovieInfo();

            if (!string.IsNullOrEmpty(movie.Year)) {
                int year;
                if (int.TryParse(movie.Year, NumberStyles.Integer, CultureInfo.InvariantCulture, out year)) {
                    movieInfo.ReleaseYear = year;
                }
            }

            movieInfo.Duration = movie.Runtime;
            movieInfo.Directors = movie.Director
                                       .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(d => new ParsedPerson(d));

            movieInfo.Writers = movie.Writer
                                     .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(d => new ParsedPerson(d));

            movieInfo.Actors = movie.Actors
                                    .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(d => new ParsedActor(d));

            movieInfo.Genres = movie.Genre.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            movieInfo.Plot = movie.Plot;
            movieInfo.Cover = movie.Poster;
            movieInfo.Rating = movie.ImdbRating.TryGetDouble();

            if (!string.IsNullOrEmpty(movie.ImdbId)) {
                movieInfo.ImdbLink = string.Format(IMDB_MOVIE_URL, movie.ImdbId);
            }

            movieInfo.Countries = movie.Country.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            movieInfo.Language = movie.Language;

            return movieInfo;
        }
    }

}