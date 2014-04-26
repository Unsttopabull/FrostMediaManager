using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Movie.SummaryModels {

    [JsonObject(Title = "images")]
    public class HeadshotImage {

        [JsonProperty("headshot")]
        public string Headshot { get; set; }
    }

}