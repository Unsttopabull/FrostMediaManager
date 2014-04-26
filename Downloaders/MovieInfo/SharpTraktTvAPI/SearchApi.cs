using SharpTraktTvAPI.Models.Search;

namespace SharpTraktTvAPI {
    public class SearchApi {
        private readonly string _apiKey;

        public SearchApi(string apiKey) {
            _apiKey = apiKey;
        }

        public MovieSearchResponse SearchMovies(string query, int limit = 30) {
            URLBuilder url = new URLBuilder(SharpTraktTv.API_URL_BASE);
            url.AddSegmentPath("search", "movies.json");
            url.AddParameter("apikey", _apiKey);
            url.AddParameter("query", query);

            if (limit != 30) {
                url.AddParameter("limit", limit);
            }

            return url.GetResposeAs<MovieSearchResponse>();
        }
    }
}
