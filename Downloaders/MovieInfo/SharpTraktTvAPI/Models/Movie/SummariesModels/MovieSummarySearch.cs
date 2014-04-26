namespace SharpTraktTvAPI.Models.Movie.SummariesModels {
    public class MovieSummarySearch {

        /// <summary>Initializes a new instance of the <see cref="MovieSummarySearch"/> class.</summary>
        public MovieSummarySearch(string title, int releaseYear) {
            Title = title;
            ReleaseYear = releaseYear;
        }

        public string Title { get; set; }
        public int ReleaseYear { get; set; }

        public string GetSlug() {
            return Title.Replace(" ", "-") + "-" + ReleaseYear;
        }
    }
}
