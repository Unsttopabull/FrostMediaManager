using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Account {
    public class AccountSettingsInfo {

        /// <summary>Initializes a new instance of the <see cref="AccountCreateInfo"/> class.</summary>
        public AccountSettingsInfo(string username, string password) {
            Username = username;
            Password = GetSha1Digest(password);
        }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        private string GetSha1Digest(string password) {
            using (SHA1 sha1 = SHA1.Create()) {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(password))
                           .Aggregate("", (s, b) => s + b.ToString("X2"));
            }
        }
    }
}
