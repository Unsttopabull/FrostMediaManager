using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Movie.SummaryModels {

    public class People {

        [JsonProperty("directors")]
        public Director[] Directors { get; set; }

        [JsonProperty("writers")]
        public Writer[] Writers { get; set; }

        [JsonProperty("producers")]
        public Producer[] Producers { get; set; }

        [JsonProperty("actors")]
        public Actor[] Actors { get; set; }
    }

}