using System.Net;

namespace SharpFanartTv {

    public enum ResponseType {
        Json,
        PHP
    }

    public enum ImageTypes {
        All,
        MovieLogo,
        MovieArt,
        MovieDisc
    }

    public enum SortType {
        MostPopularThenNewest,
        Newest,
        Oldest
    }

    public enum Limit {
        Fist,
        All
    }

    public class SharpFanartTvClient {
        private const string URI = @"http://api.fanart.tv/webservice/movie/{0}/{1}/{2}/{3}/{4}/{1}/";
        private const string URI_PLAIN = @"http://api.fanart.tv/webservice/movie/{0}/{1}/{2}/";
        private readonly string _apiKey;
        private readonly ResponseType _responseType;

        public SharpFanartTvClient(string apiKey, ResponseType responseType) {
            _apiKey = apiKey;
            _responseType = responseType;
        }

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