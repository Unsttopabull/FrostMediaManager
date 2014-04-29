using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Frost.InfoParsers;

namespace Frost.MovieInfoProviders {

    public class OpenSubtitlesInfoClient : ParsingClient {
        public const string CLIENT_NAME = "OpenSubtitles.ORG";

        public OpenSubtitlesInfoClient() : base(CLIENT_NAME, false, false, true) {
            string directoryName;
            try {
                 directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            catch {
                return;
            }

            if (directoryName != null) {
                Icon = new Uri(directoryName+"/opensubtitles.ico");
            }
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