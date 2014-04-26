using Newtonsoft.Json;

namespace SharpOmdbAPI.Models {
    public class OmdbSearch {

        public string Title { get; set; }

        public string Year { get; set; }

        [JsonProperty("imdbID")]
        public string ImdbId { get; set; }

        public string Type { get; set; }
    }
}
