using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Frost.InfoParsers;
using Frost.InfoParsers.Models.Info;
using Frost.MovieInfoProviders.OpenSubtitles;
using Frost.SharpOpenSubtitles;
using Frost.SharpOpenSubtitles.Models.Checking.Receive;
using Frost.SharpOpenSubtitles.Models.Movies;
using Frost.SharpOpenSubtitles.Models.Movies.Receive;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace Frost.MovieInfoProviders {

    public class OpenSubtitlesInfoClient : ParsingClient {
        private const string USER_AGENT = "Frost Media Manager v1";
        public const string CLIENT_NAME = "OpenSubtitles.org";
        private const string IMDB_TITLE_URI = "http://www.imdb.com/title/tt{0}";

        public OpenSubtitlesInfoClient() : base(CLIENT_NAME, false, false, true) {
            string directoryName = GetAssemblyCurrentDirectory();

            if (directoryName != null) {
                Icon = new Uri(directoryName + "/opensubtitles.ico");
            }
        }

        public override IEnumerable<ParsedMovie> GetByImdbId(string imdbId) {
            OpenSubtitlesClient cli = new OpenSubtitlesClient(false);

            ImdbMovieDetailsInfo movieDetails;
            try {
                LogInInfo logIn = cli.LogInAnonymous("en", USER_AGENT);
                if (logIn.Status != "200 OK") {
                    return null;
                }

                movieDetails = cli.Movie.GetImdbDetails(imdbId.TrimStart('t'));
                if (movieDetails.Status != "200 OK" || movieDetails.Data == null) {
                    movieDetails = null;
                }
            }
            catch {
                throw;
            }
            finally {
                cli.LogOut();
            }

            return movieDetails != null
                       ? new[] { new OSubParsedMovie(movieDetails.Data) }
                       : null;
        }

        public override IEnumerable<ParsedMovie> GetByMovieHash(IEnumerable<string> movieHashes) {
            OpenSubtitlesClient cli = new OpenSubtitlesClient(false);
            LogInInfo info = cli.LogInAnonymous("en", USER_AGENT);
            if (info.Status != "200 OK") {
                return null;
            }

            MovieHashInfo movieHashInfo = cli.Movie.CheckHash(movieHashes.ToArray());
            cli.LogOut();

            if (movieHashInfo == null || movieHashInfo.Status != "200 OK" || movieHashInfo.Data == null) {
                return null;
            }


            return movieHashInfo.Data
                                .Where(movie => movie != null)
                                .Select(movie => new ParsedMovie(movie.Title, movie.ReleaseYear, movie.ImdbId))
                                .ToList();
        }

        public override IEnumerable<ParsedMovie> GetByTitle(string title, int releaseYear) {
            OpenSubtitlesClient cli = new OpenSubtitlesClient(false);
            LogInInfo info = cli.LogInAnonymous("en", USER_AGENT);
            if (info.Status != "200 OK") {
                return null;
            }

            ImdbSearchInfo imdbSearchInfo = cli.Movie.SearchOnIMDB(title);
            cli.LogOut();

            if (imdbSearchInfo.Status != "200 OK" || imdbSearchInfo.Data == null) {
                return null;
            }

            return imdbSearchInfo.Data
                                 .Where(imdbMovie => imdbMovie != null)
                                 .Select(imdbMovie => new ParsedMovie(imdbMovie.Title, 0, imdbMovie.ID))
                                 .ToList();
        }

        public override void Index() {
            throw new NotImplementedException();
        }

        public override ParsedMovieInfo ParseMovieInfo(ParsedMovie movie) {
            if (movie is OSubParsedMovie) {
                return GetParsedInfo(movie as OSubParsedMovie);
            }

            if (!string.IsNullOrEmpty(movie.ImdbID)) {
                IEnumerable<ParsedMovie> byImdbId = GetByImdbId(movie.ImdbID);
                if (byImdbId != null) {
                    return GetParsedInfo(byImdbId.FirstOrDefault() as OSubParsedMovie);
                }
            }
            return null;
        }

        private static ParsedMovieInfo GetParsedInfo(OSubParsedMovie movie) {
            if (movie == null) {
                return null;
            }

            ParsedMovieInfo info = new ParsedMovieInfo();

            ImdbMovieDetails details = movie.Details;
            info.Countries = details.Countries;
            info.Cover = details.Cover;

            if (details.Awards != null) {
                info.Awards = details.Awards
                                     .Where(award => award != null)
                                     .Select(award => new ParsedAward { Award = award })
                                     .Cast<IParsedAward>()
                                     .ToList();
            }

            if (details.Directors != null) {
                info.Directors = details.Directors
                                        .Where(director => director != null)
                                        .Select(director => new ParsedPerson(director.Name, director.ImdbId))
                                        .Cast<IParsedPerson>()
                                        .ToList();
            }

            if (details.Writers != null) {
                info.Writers = details.Writers
                                      .Where(w => w != null)
                                      .Select(writer => new ParsedPerson(writer.Name, writer.ImdbId))
                                      .Cast<IParsedPerson>()
                                      .ToList();
            }

            if (details.Actors != null) {
                info.Actors = details.Actors
                                     .Where(a => a != null)
                                     .Select(actor => new ParsedActor(actor.Name, null, string.IsNullOrEmpty(actor.ImdbId) ? null : "nm" + actor.ImdbId, null))
                                     .Cast<IParsedActor>()
                                     .ToList();
            }

            info.Duration = details.Duration;

            if (details.Genres != null) {
                info.Genres = details.Genres.Select(g => g.Trim());
            }

            if (!string.IsNullOrEmpty(details.Id)) {
                info.ImdbLink = string.Format(IMDB_TITLE_URI, details.Id);
            }

            if (details.Languages != null) {
                info.Language = details.Languages.FirstOrDefault();
            }

            info.Plot = details.Plot;
            info.Tagline = details.Tagline;

            if (!string.IsNullOrEmpty(details.Year)) {
                int releaseYear;
                if (int.TryParse(details.Year, NumberStyles.Integer, CultureInfo.InvariantCulture, out releaseYear)) {
                    info.ReleaseYear = releaseYear;
                }
            }

            return info;
        }
    }

}