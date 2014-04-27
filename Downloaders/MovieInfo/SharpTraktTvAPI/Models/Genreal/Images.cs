using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Genreal {

    public class Images {

        [JsonProperty("poster")]
        public string Poster { get; set; }

        [JsonProperty("fanart")]
        public string Fanart { get; set; }
    }

}