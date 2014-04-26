using SharpTraktTvAPI.Models.Movie.GenrealModels;

namespace SharpTraktTvAPI.Models.Movie.SummariesModels {

    public class MovieSummary {
        public string title { get; set; }
        public int year { get; set; }
        public string imdb_id { get; set; }
        public string tmdb_id { get; set; }
        public int last_updated { get; set; }
        public int released { get; set; }
        public string trailer { get; set; }
        public int runtime { get; set; }
        public string tagline { get; set; }
        public string overview { get; set; }
        public string certification { get; set; }
        public string url { get; set; }
        public Images images { get; set; }
        public string[] genres { get; set; }
    }

}