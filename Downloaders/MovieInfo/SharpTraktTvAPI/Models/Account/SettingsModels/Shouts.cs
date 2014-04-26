using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Account.SettingsModels {

    public class Shouts {

        [JsonProperty("show_badges")]
        public bool ShowBadges { get; set; }

        [JsonProperty("show_spoilers")]
        public bool ShowSpoilers { get; set; }
    }

}