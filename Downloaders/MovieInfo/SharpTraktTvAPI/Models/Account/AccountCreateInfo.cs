using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Account {

    internal class AccountCreateInfo : AccountSettingsInfo {

        /// <summary>Initializes a new instance of the <see cref="AccountCreateInfo"/> class.</summary>
        public AccountCreateInfo(string username, string password, string email) : base(username, password){
            Email = email;
        }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

}