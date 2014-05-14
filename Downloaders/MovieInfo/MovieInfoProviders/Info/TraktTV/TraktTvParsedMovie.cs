using Frost.InfoParsers;
using Frost.SharpTraktTvAPI.Models.Movie;
using Frost.SharpTraktTvAPI.Models.Search;

namespace Frost.MovieInfoProviders.Info.TraktTV {
    public class TraktTvParsedMovie : ParsedMovie {
        public TraktTvParsedMovie(MovieSummaryResponse summary) : base(summary.Title, null, null) {
            Summary = summary;
        }

        public TraktTvParsedMovie(MovieMatch match) : base(match.Title, null, null) {
            Match = match;
        }

        public MovieMatch Match { get; private set; }

        public MovieSummaryResponse Summary { get; private set; }
    }
}
