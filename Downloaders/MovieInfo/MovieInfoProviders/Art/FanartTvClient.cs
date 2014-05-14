using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Frost.InfoParsers;
using Frost.InfoParsers.Models.Art;
using Frost.InfoParsers.Models.Info;
using Frost.SharpFanartTv;
using Newtonsoft.Json.Linq;

namespace Frost.MovieInfoProviders.Art {

    public class FanartTvClient : IFanartClient {
        public const string CLIENT_NAME = "Fanart.TV";
        private const string API_KEY = @"99a04fb5ed7cf203c26c9989dc534ea5";

        public FanartTvClient() {
            IsImdbSupported = true;
            IsTitleSupported = true;
            IsTitleSupported = false;
            Name = CLIENT_NAME;

            string directoryName;
            try {
                 directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            catch {
                return;
            }

            if (directoryName != null) {
                Icon = new Uri(directoryName+"/fanart.ico");
            }
        }

        public IParsedArts GetMovieArtFromTitle(string title, int year) {
            throw new NotSupportedException("Obtaining art through movie title is not supported");
        }

        public IParsedArts GetMovieArtFromTmdbId(string tmdbId) {
            return GetById(tmdbId);
        }

        public IParsedArts GetMovieArtFromImdbId(string imdbId) {
            return GetById(imdbId);
        }

        public bool IsImdbSupported { get; private set; }
        public bool IsTmdbSupported { get; private set; }
        public bool IsTitleSupported { get; private set; }

        public Uri Icon { get; private set; }
        public string Name { get; private set; }

        private static IParsedArts GetById(string imdbId) {
            SharpFanartTvClient cli = new SharpFanartTvClient(API_KEY, ResponseType.Json);
            string json = cli.GetByMovieId(imdbId);

            JToken first;
            try {
                JObject jObject = JObject.Parse(json);
                first = jObject.First.FirstOrDefault();
            }
            catch (Exception) {
                return null;
            }

            if (first != null) {
                ParsedArts arts = new ParsedArts();

                JToken covers = first.SelectToken("movieposter");
                if (covers != null) {
                    arts.Covers = covers.Children()
                                        .Select(t => t.SelectToken("url").ToString())
                                        .Select(s => new ParsedArt(ParsedArtType.Cover, s + "/preview", s));
                }

                JToken fanarts = first.SelectToken("moviebackground");
                if (fanarts != null) {
                    arts.Fanart = fanarts.Children()
                                         .Select(t => t.SelectToken("url").ToString())
                                         .Select(s => new ParsedArt(ParsedArtType.Fanart, s + "/preview", s));
                }

                return arts;
            }

            return null;
        }
    }

}