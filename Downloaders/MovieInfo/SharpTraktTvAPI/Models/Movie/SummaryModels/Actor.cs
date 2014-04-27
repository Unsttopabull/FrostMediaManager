using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Movie.SummaryModels {

    public class Actor {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("character")]
        public string Character { get; set; }

        [JsonProperty("images")]
        public HeadshotImage Images { get; set; }
    }

}