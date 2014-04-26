using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Account.SettingsModels {

    public class Viewing {

        [JsonProperty("ratings")]
        public Ratings Ratings { get; set; } ////simple,advanced

        [JsonProperty("shouts")]
        public Shouts Shouts { get; set; }
    }

}