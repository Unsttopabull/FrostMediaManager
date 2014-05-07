using Frost.InfoParsers;
using Frost.SharpOpenSubtitles.Models.Movies;

namespace Frost.MovieInfoProviders.Info.OpenSubtitles {
    public class OSubParsedMovie : ParsedMovie {

        public OSubParsedMovie(ImdbMovieDetails details) : base(details.Title, null, null){
            Details = details;
        }

        public ImdbMovieDetails Details { get; set; }
    }
}
