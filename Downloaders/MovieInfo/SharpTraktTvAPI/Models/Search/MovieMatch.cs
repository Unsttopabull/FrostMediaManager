using SharpTraktTvAPI.Models.Movie.GenrealModels;

namespace SharpTraktTvAPI.Models.Search {

    public class MovieMatch {
        public string title { get; set; }
        public int year { get; set; }
        public int released { get; set; }
        public string url { get; set; }
        public string trailer { get; set; }
        public int runtime { get; set; }
        public string tagline { get; set; }
        public string overview { get; set; }
        public string certification { get; set; }
        public string imdb_id { get; set; }
        public int tmdb_id { get; set; }
        public Images images { get; set; }
        public string[] genres { get; set; }
        public Ratings ratings { get; set; }
    }

}