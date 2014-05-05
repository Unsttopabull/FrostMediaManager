using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using Frost.InfoParsers.Models.Info;
using SharpTrailerAddictAPI.Models;

namespace SharpTrailerAddictAPI {
    public class TrailerAddictClient : IPromotionalVideoClient {
        public const string CLIENT_NAME = "TrailerAddict";

        public TrailerAddictClient() {
            Name = CLIENT_NAME;
            IsImdbSupported = true;
            IsTitleSupported = true;

            string directoryName;
            try {
                 directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            catch {
                return;
            }

            if (directoryName != null) {
                Icon = new Uri(directoryName+"/traileraddict.png");
            }
        }

        public bool IsImdbSupported { get; private set; }
        public bool IsTmdbSupported { get; private set; }
        public bool IsTitleSupported { get; private set; }
        public string Name { get; private set; }
        public Uri Icon { get; private set; }

        public IEnumerable<IParsedVideo> GetMovieVideosFromTitle(string title, int year) {
            Trailers trailers = TrailerAddictApi.GetMovieVideosByTitle(title, 30);

            if (trailers == null || trailers.Trailer == null) {
                return null;
            }

            return trailers.Trailer.Select(t => new ParsedVideo { Type = GetVideoType(t.Title), Url = t.Link, Title = t.Title });
        }

        public IEnumerable<IParsedVideo> GetMovieVideosFromTmdbId(string tmdbId) {
            throw new NotImplementedException();
        }

        public IEnumerable<IParsedVideo> GetMovieVideosFromImdbId(string imdbId) {
            Trailers trailers = TrailerAddictApi.GetMovieVideosByImdbId(imdbId.TrimStart('t'), 30);

            if (trailers == null || trailers.Trailer == null) {
                return null;
            }

            return trailers.Trailer.Select(t => new ParsedVideo { Type = GetVideoType(t.Title), Url = t.Link, Title = t.Title });
        }

        private VideoType GetVideoType(string title) {
            if (title.ContainsIgnoreCase("Review")) {
                return VideoType.Review;
            }

            if (title.ContainsIgnoreCase("trailer")) {
                return VideoType.Trailer;
            }

            if (title.ContainsIgnoreCase("Interview")) {
                return VideoType.Interview;
            }

            if (title.ContainsIgnoreCase("Featurette")) {
                return VideoType.Featurete;
            }

            if (title.Contains("Behind the Scenes")) {
                return VideoType.BehindTheScenes;
            }

            if (title.ContainsIgnoreCase("Clip") || title.ContainsIgnoreCase("Klip") || title.Contains("izrezek")) {
                return VideoType.Clip;
            }

            if (title.ContainsIgnoreCase("TV Spot")) {
                return VideoType.TvSpot;
            }

            return VideoType.Unknown;
        }
    }
}
