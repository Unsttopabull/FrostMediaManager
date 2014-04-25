using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using SharpOmdbAPI.Models;

namespace SharpOmdbAPI {

    public enum PlotLength {
        Short,
        Full
    }

    public static class SharpOmdbClient {
        private const string SEARCH_URI = "http://www.omdbapi.com/?s={0}{1}";
        private const string GET_IMDB_URI = "http://www.omdbapi.com/?i={0}&plot={1}&tomatoes={2}";
        private const string GET_TITLE_URI = "http://www.omdbapi.com/?t={0}{1}&plot={2}&tomatoes={3}";

        public static OmdbMovie GetByImdbId(string id, PlotLength plotLength = PlotLength.Short, bool includeTomatoesInfo = false) {
            return !string.IsNullOrEmpty(id) 
                ? Download<OmdbMovie>(string.Format(GET_IMDB_URI, id.ToString(CultureInfo.InvariantCulture), plotLength, includeTomatoesInfo)) 
                : null;
        }

        public static OmdbMovie GetByTitle(string title, PlotLength plotLength = PlotLength.Short, bool includeTomatoesInfo = false) {
            return !string.IsNullOrEmpty(title) 
                ? Download<OmdbMovie>(string.Format(GET_TITLE_URI, WebUtility.UrlEncode(title), null, plotLength, includeTomatoesInfo)) 
                : null;
        }

        public static OmdbMovie GetByTitle(string title, int releaseYear, PlotLength plotLength = PlotLength.Short, bool includeTomatoesInfo = false) {
            return !string.IsNullOrEmpty(title) 
                ? Download<OmdbMovie>(string.Format(GET_TITLE_URI, WebUtility.UrlEncode(title), "&y=" + releaseYear.ToString(CultureInfo.InvariantCulture), plotLength, includeTomatoesInfo))
                : null;
        }

        public static IEnumerable<OmdbSearch> Search(string title, int releaseYear = 0) {
            return !string.IsNullOrEmpty(title)
                ? Download<OmdbSearchInfo>(string.Format(SEARCH_URI, WebUtility.UrlEncode(title), releaseYear > 0 ? "&y=" + releaseYear.ToString(CultureInfo.InvariantCulture) : null)).Search
                : null;
        }

        private static T Download<T>(string uri) {
            using (WebClient cli = new WebClient()) {
                string json = cli.DownloadString(uri);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }

}