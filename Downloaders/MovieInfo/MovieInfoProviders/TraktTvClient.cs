using System.Collections.Generic;
using Frost.InfoParsers;

namespace Frost.MovieInfoProviders {
    public class TraktTvClient : ParsingClient {
        public TraktTvClient() : base("TraktTV", false, false) {
        }

        public override IEnumerable<ParsedMovie> GetByImdbId(string imdbId) {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<ParsedMovie> GetByMovieHash(IEnumerable<string> movieHashes) {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<ParsedMovie> GetByTitle(string title, int releaseYear) {
            throw new System.NotImplementedException();
        }

        public override void Index() {
            throw new System.NotImplementedException();
        }

        public override ParsedMovieInfo ParseMovieInfo(ParsedMovie movie) {
            throw new System.NotImplementedException();
        }
    }
}
