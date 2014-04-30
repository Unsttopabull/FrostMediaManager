using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using Frost.SharpOpenSubtitles.Models.Movies;
using Frost.SharpOpenSubtitles.Models.Movies.Receive;

namespace Frost.MovieInfoProviders.OpenSubtitles {
    public class OSubParsedMovie : ParsedMovie {

        public OSubParsedMovie(ImdbMovieDetails details) : base(details.Title, null, null){
            Details = details;
        }

        public ImdbMovieDetails Details { get; set; }
    }
}
