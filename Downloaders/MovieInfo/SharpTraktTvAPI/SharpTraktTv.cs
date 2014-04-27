namespace SharpTraktTvAPI {

    public class SharpTraktTv {
        private readonly string _apiKey;
        internal const string API_URL_BASE = "http://api.trakt.tv/";

        public SharpTraktTv(string apiKey) {
            _apiKey = apiKey;

            Account = new AccountApi(apiKey);
            Movie = new MovieApi(apiKey);
            Search = new SearchApi(apiKey);
        }

        public AccountApi Account { get; private set; }
        public MovieApi Movie { get; private set; }
        public SearchApi Search { get; private set; }
    }

}