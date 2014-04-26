using SharpTraktTvAPI.Models.Movie.GenrealModels;
using SharpTraktTvAPI.Models.Movie.SummaryModels;

namespace SharpTraktTvAPI.Models.Movie {

    public class MovieSummaryResponse {
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
        public int rt_id { get; set; }
        public int last_updated { get; set; }
        public Images images { get; set; }
        public string[] genres { get; set; }
        public Top_Watchers[] top_watchers { get; set; }
        public Ratings ratings { get; set; }
        public Stats stats { get; set; }
        public People people { get; set; }
        public bool watched { get; set; }
        public int plays { get; set; }
        public string rating { get; set; }
        public int rating_advanced { get; set; }
        public bool in_watchlist { get; set; }
        public bool in_collection { get; set; }
    }

}
