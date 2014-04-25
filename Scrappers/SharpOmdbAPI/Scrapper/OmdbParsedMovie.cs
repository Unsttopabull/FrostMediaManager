using System.Globalization;
using Frost.InfoParsers;
using SharpOmdbAPI.Models;

namespace SharpOmdbAPI.Scrapper {

    public class OmdbParsedMovie : ParsedMovie {
        private readonly OmdbMovie _movie;
        private readonly OmdbSearch _search;

        public OmdbParsedMovie(OmdbSearch search) : base(search.Title, null, null) {
            int release;
            if (int.TryParse(search.Year, NumberStyles.Integer, CultureInfo.InvariantCulture, out release)) {
                ReleaseYear = release;
            }

            _search = search;
        }

        public OmdbParsedMovie(OmdbMovie movie) : base(movie.Title, null, null) {
            _movie = movie;
        }

        public OmdbSearch Search {
            get { return _search; }
        }

        public OmdbMovie Movie {
            get { return _movie; }
        }
    }
}
