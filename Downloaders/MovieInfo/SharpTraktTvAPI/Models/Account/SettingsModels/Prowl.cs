using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Account.SettingsModels {

    public class Prowl {

        [JsonProperty("connected")]
        public bool Connected { get; set; }
    }

}