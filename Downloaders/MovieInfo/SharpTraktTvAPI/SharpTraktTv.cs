namespace SharpTraktTvAPI {

    public class SharpTraktTv {
        internal readonly string ApiKey;
        internal const string API_URL_BASE = "http://api.trakt.tv/";

        public SharpTraktTv(string apiKey) {
            ApiKey = apiKey;

            Account = new AccountApi(apiKey);
            Movie = new MovieApi(apiKey);
        }

        public AccountApi Account { get; private set; }
        public MovieApi Movie { get; private set; }
    }

}