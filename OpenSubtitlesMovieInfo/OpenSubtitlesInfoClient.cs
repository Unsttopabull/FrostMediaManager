using System;
using System.Collections.Generic;
using System.Linq;
using Frost.InfoParsers;
using Frost.SharpOpenSubtitles;
using Frost.SharpOpenSubtitles.Models.Checking;
using Frost.SharpOpenSubtitles.Models.Checking.Receive;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace OpenSubtitlesMovieInfo {

    public class OpenSubtitlesInfoClient : ParsingClient {

        public OpenSubtitlesInfoClient() : base("OSub") {
            CanIndex = false;
        }

        public override IEnumerable<ParsedMovie> Parse(string imdbId, string title) {
            return Parse(imdbId, title, null);
        }

        public override IEnumerable<ParsedMovie> Parse(string imdbId, string title, IEnumerable<string> movieHashes) {
            OpenSubtitlesClient cli = new OpenSubtitlesClient(false);
            LogInInfo info = cli.Session.LogIn("", "", "", Session.DEBUG_UA);

            if (info.Status != "200 OK") {
                return null;
            }

            string[] hashes = movieHashes as string[] ?? movieHashes.ToArray();
            if (movieHashes != null && hashes.Length > 0) {
                MovieHashInfo movieHashInfo = cli.Movie.CheckHash(hashes);
                List<MovieInfo> movieInfos = movieHashInfo.Data.ToList();
            }
            else {
                
            }

            cli.Session.LogOut();
            return null;
        }

        public override void Parse() {
            throw new NotImplementedException();
        }

        public override ParsedMovieInfo ParseMovieInfo(ParsedMovie movie) {
            throw new NotImplementedException();
        }
    }

}