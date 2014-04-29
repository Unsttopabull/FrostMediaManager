using System;
using System.Collections.Generic;
using Frost.InfoParsers;

namespace Frost.MovieInfoProviders {

    public class OpenSubtitlesInfoClient : ParsingClient {

        public OpenSubtitlesInfoClient() : base("OSub", false, false, true) {
        }

        public override IEnumerable<ParsedMovie> GetByImdbId(string imdbId) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ParsedMovie> GetByMovieHash(IEnumerable<string> movieHashes) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ParsedMovie> GetByTitle(string title, int releaseYear) {
            throw new NotImplementedException();
        }

        public override void Index() {
            throw new NotImplementedException();
        }

        public override ParsedMovieInfo ParseMovieInfo(ParsedMovie movie) {
            throw new NotImplementedException();
        }
    }

}