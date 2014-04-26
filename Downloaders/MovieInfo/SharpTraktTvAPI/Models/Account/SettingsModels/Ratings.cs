using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Account.SettingsModels {

    public class Ratings {

        [JsonProperty("mode")]
        public string Mode { get; set; }
    }

}