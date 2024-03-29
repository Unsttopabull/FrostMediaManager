﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Frost.InfoParsers;
using Frost.SharpTraktTvAPI;
using Frost.SharpTraktTvAPI.Models.Movie;
using Frost.SharpTraktTvAPI.Models.Search;

namespace Frost.MovieInfoProviders.Info.TraktTV {

    public class TraktTvClient : ParsingClient {
        private const string IMDB_MOVIE_URL = "http://www.imdb.com/title/{0}/";
        public const string CLIENT_NAME = "Trakt.TV";
        private readonly SharpTraktTv _trakt;

        public TraktTvClient() : base(CLIENT_NAME, false, false, true) {
            _trakt = new SharpTraktTv("dc9b6e2e5526762ae8a050780ef6d04b");

            string directoryName;
            try {
                 directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            catch {
                return;
            }

            if (directoryName != null) {
                Icon = new Uri(directoryName+"/traktTv.png");
            }
        }

        public override IEnumerable<ParsedMovie> GetByImdbId(string imdbId) {
            MovieSummaryResponse summaries = _trakt.Movie.SummaryById(imdbId);

            return new[] { new TraktTvParsedMovie(summaries) };
        }

        public override IEnumerable<ParsedMovie> GetByMovieHash(IEnumerable<string> movieHashes) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ParsedMovie> GetByTitle(string title, int releaseYear) {
            return _trakt.Search.SearchMovies(title, 5).Select(m => new TraktTvParsedMovie(m));
        }

        public override void Index() {
            throw new NotImplementedException();
        }

        public override ParsedMovieInfo ParseMovieInfo(ParsedMovie movie) {
            if (!(movie is TraktTvParsedMovie)) {
                return null;
            }

            TraktTvParsedMovie mv = (TraktTvParsedMovie) movie;
            MovieSummaryResponse summary = mv.Match != null
                                               ? GetFromMatch(mv.Match)
                                               : mv.Summary;


            ParsedMovieInfo info = new ParsedMovieInfo();
            if (!string.IsNullOrEmpty(summary.ImdbID)) {
                info.ImdbLink = string.Format(IMDB_MOVIE_URL, summary.ImdbID);
            }

            info.Rating = summary.Ratings.Percentage / 10.0;
            info.Plot = summary.Overview;
            info.Tagline = summary.Tagline;

            if (summary.ReleaseYear > 1800) {
                info.ReleaseYear = summary.ReleaseYear;
            }

            if (summary.TmdbID > 0) {
                info.TmdbId = summary.TmdbID.ToInvariantString();
            }

            info.TrailerUrl = summary.Trailer;
            info.Genres = summary.Genres;
            info.Fanart = summary.Images.Fanart;
            info.FanartPreview = summary.Images.FanartMedium;
            info.Cover = summary.Images.PosterMedium;
            info.CoverPreview = summary.Images.PosterSmall;

            info.MPAA = summary.Certification;

            info.Writers = summary.People.Writers.Select(w => new ParsedPerson(w.Name, null, w.Images.Headshot == @"http://slurm.trakt.us/images/avatar-large.jpg" ? null : w.Images.Headshot));
            info.Directors = summary.People.Directors.Select(d => new ParsedPerson(d.Name, null, d.Images.Headshot == @"http://slurm.trakt.us/images/avatar-large.jpg" ? null : d.Images.Headshot));
            info.Actors = summary.People.Actors.Select(a => new ParsedActor(a.Name, a.Character, null, a.Images.Headshot == @"http://slurm.trakt.us/images/avatar-large.jpg" ? null : a.Images.Headshot));

            return info;
        }

        private MovieSummaryResponse GetFromMatch(MovieMatch match) {
            if (!string.IsNullOrEmpty(match.ImdbId)) {
                return _trakt.Movie.SummaryById(match.ImdbId);
            }

            if (match.TmdbID > 0) {
                return _trakt.Movie.SummaryById(match.TmdbID.ToInvariantString());
            }

            if (!string.IsNullOrEmpty(match.Url)) {
                int idx = match.Url.LastIndexOf("/", StringComparison.Ordinal);
                string slug = match.Url.Substring(idx, match.Url.Length - idx);

                return _trakt.Movie.Summary(slug);
            }

            return _trakt.Movie.SummaryByTitleAndYear(match.Title, match.Year);
        }
    }

}