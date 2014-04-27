using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Movie.SummaryModels {

    public class Director {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("images")]
        public HeadshotImage Images { get; set; }
    }

}