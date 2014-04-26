using SharpTraktTvAPI.Models;
using SharpTraktTvAPI.Models.Account;

namespace SharpTraktTvAPI {

    public class AccountApi {
        private readonly string _apiKey;

        public AccountApi(string apiKey) {
            _apiKey = apiKey;
        }

        public StatusResponse Create(string username, string password, string email) {
            URLBuilder url = new URLBuilder(SharpTraktTv.API_URL_BASE);
            url.AddSegmentPath("account", "create", _apiKey);

            return url.GetPostResponseAs<StatusResponse, AccountCreateInfo>(new AccountCreateInfo(username, password, email));
        }

        public SettingsResponse Settings(string username, string password) {
            URLBuilder url = new URLBuilder(SharpTraktTv.API_URL_BASE);
            url.AddSegmentPath("account", "settings", _apiKey);

            return url.GetPostResponseAs<SettingsResponse, AccountSettingsInfo>(new AccountSettingsInfo(username, password));
        }

        public StatusResponse Test(string username, string password) {
            URLBuilder url = new URLBuilder(SharpTraktTv.API_URL_BASE);
            url.AddSegmentPath("account", "test", _apiKey);

            return url.GetPostResponseAs<StatusResponse, AccountSettingsInfo>(new AccountSettingsInfo(username, password));            
        }
    }

}