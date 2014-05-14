using System.Net;

namespace Frost.SharpFanartTv {

    /// <summary>The type of response to return</summary>
    public enum ResponseType {
        /// <summary>JSON string</summary>
        Json,
        /// <summary>PHP serialize() serialized key-value array</summary>
        PHP
    }

    /// <summary>The type of an image to search for.</summary>
    public enum ImageTypes {
        /// <summary>All types</summary>
        All,

        /// <summary>Only movie logos</summary>
        MovieLogo,

        /// <summary>Only movie arts</summary>
        MovieArt,

        /// <summary>Only movie disc overlays</summary>
        MovieDisc
    }

    /// <summary>How to sort results</summary>
    public enum SortType {
        /// <summary>Most popular first then newest</summary>
        MostPopularThenNewest,
        /// <summary>Newest first</summary>
        Newest,
        /// <summary>Oldest first</summary>
        Oldest
    }

    /// <summary>How many results to return</summary>
    public enum Limit {
        /// <summary>Return only the first match</summary>
        Fist,
        /// <summary>Return all matches</summary>
        All
    }

    /// <summary>A client for communicating with the Fanart.TV API service.</summary>
    public class SharpFanartTvClient {
        private const string URI = @"http://api.fanart.tv/webservice/movie/{0}/{1}/{2}/{3}/{4}/{1}/";
        private const string URI_PLAIN = @"http://api.fanart.tv/webservice/movie/{0}/{1}/{2}/";
        private readonly string _apiKey;
        private readonly ResponseType _responseType;

        /// <summary>Initializes a new instance of the <see cref="SharpFanartTvClient"/> class.</summary>
        /// <param name="apiKey">The API key to use when accessing the service (required).</param>
        /// <param name="responseType">Type of the response to return.</param>
        public SharpFanartTvClient(string apiKey, ResponseType responseType) {
            _apiKey = apiKey;
            _responseType = responseType;
        }

        /// <summary>Gets the by movie art by IMDB identifier.</summary>
        /// <param name="id">The IMDB identifier.</param>
        /// <param name="types">The image types to search for.</param>
        /// <param name="sort">How to sort the results.</param>
        /// <param name="limit">The results limit.</param>
        /// <returns>Either PHP or JSON Response data</returns>
        /// <exception cref="WebException">Throws when there was an error accessing the web API service.</exception>
        public string GetByMovieId(string id, ImageTypes types = ImageTypes.All, SortType sort = SortType.MostPopularThenNewest, Limit limit = Limit.All) {
            if (types == ImageTypes.All && sort == SortType.MostPopularThenNewest && limit == Limit.All) {
                return DownloadUri(string.Format(URI_PLAIN, _apiKey, id, _responseType.ToString().ToLowerInvariant()));
            }

            return DownloadUri(string.Format(URI, _apiKey, id, types.ToString().ToLowerInvariant(), (int) sort + 1, (int) limit + 1));
        }

        private string DownloadUri(string uri) {
            using (WebClient wc = new WebClient()) {
                return wc.DownloadString(uri);
            }
        }
    }

}