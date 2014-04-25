using System;
using System.Linq;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using Newtonsoft.Json.Linq;

namespace SharpFanartTv {

    public class FanartTvArtClient : IFanartClient {
        private const string API_KEY = @"99a04fb5ed7cf203c26c9989dc534ea5";

        public IParsedArts GetMovieArtFromTitle(string title) {
            throw new NotSupportedException("Obtaining art through movie title is not supported");
        }

        public IParsedArts GetMovieArtFromTmdbId(string tmdbId) {
            return GetById(tmdbId);
        }

        public IParsedArts GetMovieArtFromImdbId(string imdbId) {
            return GetById(imdbId);
        }

        private static IParsedArts GetById(string imdbId) {
            SharpFanartTvClient cli = new SharpFanartTvClient(API_KEY, ResponseType.Json);
            string json = cli.GetByMovieId(imdbId);

            JToken first;
            try {
                JObject jObject = JObject.Parse(json);
                first = jObject.First.FirstOrDefault();
            }
            catch (Exception e) {
                return null;
            }

            if (first != null) {
                ParsedArts arts = new ParsedArts();

                JToken covers = first.SelectToken("movieposter");
                if (covers != null) {
                    arts.Covers = covers.Children()
                                        .Select(t => t.SelectToken("url").ToString())
                                        .Select(s => new ParsedArt(s + "/preview", s));
                }

                JToken fanarts = first.SelectToken("moviebackground");
                if (fanarts != null) {
                    arts.Fanart = fanarts.Children()
                                         .Select(t => t.SelectToken("url").ToString())
                                         .Select(s => new ParsedArt(s + "/preview", s));
                }

                return arts;
            }

            return null;
        }
    }

}