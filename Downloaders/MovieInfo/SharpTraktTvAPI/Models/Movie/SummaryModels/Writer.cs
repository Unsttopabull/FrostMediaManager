using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Movie.SummaryModels {

    public class Writer {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("images")]
        public HeadshotImage Images { get; set; }
    }

}