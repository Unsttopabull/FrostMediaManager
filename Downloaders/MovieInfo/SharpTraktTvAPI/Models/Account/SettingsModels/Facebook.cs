using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Account.SettingsModels {

    public class Facebook {

        [JsonProperty("connected")]
        public bool Connected { get; set; }
    }

}