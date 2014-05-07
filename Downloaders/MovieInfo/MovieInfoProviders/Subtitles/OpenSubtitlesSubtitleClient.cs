using System;
using System.Collections.Generic;
using System.Linq;
using Frost.InfoParsers.Models.Subtitles;
using Frost.InfoParsers;
using Frost.SharpOpenSubtitles;
using Frost.SharpOpenSubtitles.Models.Search;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.MovieInfoProviders.Subtitles {

    public class OpenSubtitlesSubtitleClient : ISubtitleClient {
        private const string USER_AGENT = "Frost Media Manager v1";
        public const string CLIENT_NAME = "OpenSubtitles.org";

        public OpenSubtitlesSubtitleClient() {
            string directoryName = ParsingClient.GetAssemblyCurrentDirectory();

            if (directoryName != null) {
                Icon = new Uri(directoryName + "/opensubtitles.ico");
            }

            Name = CLIENT_NAME;
            IsImdbSupported = true;
            IsTmdbSupported = false;
            IsTitleSupported = false;
            IsMovieHashSupported = true;
        }

        public bool IsImdbSupported { get; private set; }
        public bool IsTmdbSupported { get; private set; }
        public bool IsTitleSupported { get; private set; }
        public bool IsMovieHashSupported { get; private set; }

        public string Name { get; private set; }
        public Uri Icon { get; private set; }

        public IEnumerable<ISubtitleInfo> GetMovieSubtitlesFromTitle(string title, int year, IEnumerable<string> languageAlpha3) {
            throw new NotImplementedException();
        }

        public IEnumerable<ISubtitleInfo> GetMovieSubtitlesFromImdbId(string imdbId, IEnumerable<string> languageAlpha3) {
            if (languageAlpha3 == null) {
                languageAlpha3 = new[] { "all" };
            }

            OpenSubtitlesClient cli = new OpenSubtitlesClient(false);

            LogInInfo status = cli.LogIn(null, null, "en", USER_AGENT);
            if (status.Status != "200 OK") {
                return null;
            }

            SearchSubtitleInfo subsInfo;
            try {
                SubtitleImdbLookupInfo lookup = new SubtitleImdbLookupInfo(imdbId.TrimStart('t'), languageAlpha3);
                subsInfo = cli.Subtitle.Search(new[] { lookup });
            }
            finally {
                cli.LogOut();
            }

            if (subsInfo != null && subsInfo.Data != null) {
                return subsInfo.Data.Select(s => new OSubSubtileInfo(s));
            }
            return null;
        }

        public IEnumerable<ISubtitleInfo> GetSubtitlesByMovieHash(IEnumerable<IMovieHash> movieHashes, IEnumerable<string> languageAlpha3) {
            if (languageAlpha3 == null) {
                languageAlpha3 = new[] { "all" };
            }

            OpenSubtitlesClient cli = new OpenSubtitlesClient(false);

            LogInInfo status = cli.LogIn(null, null, "en", USER_AGENT);
            if (status.Status != "200 OK") {
                return null;
            }

            SearchSubtitleInfo subsInfo;
            try {
                List<SubtitleLookupInfo> lookupinfo = new List<SubtitleLookupInfo>();
                string[] languages = languageAlpha3.ToArray();

                foreach (IMovieHash hash in movieHashes) {
                    SubtitleLookupInfo lookup = new SubtitleLookupInfo(
                        hash.MovieHashDigest,
                        hash.FileByteSize,
                        languages
                        );

                    lookupinfo.Add(lookup);
                }

                try {
                    subsInfo = cli.Subtitle.Search(lookupinfo.ToArray());
                }
                catch (Exception e) {
                    Exception ex = e;
                    throw;
                }
            }
            finally {
                cli.LogOut();
            }

            if (subsInfo != null && subsInfo.Data != null) {
                return subsInfo.Data.Select(s => new OSubSubtileInfo(s));
            }
            return null;
        }

        public IEnumerable<ISubtitleFile> CheckSubtitleExistsByMD5(IEnumerable<string> hashes, IEnumerable<string> languageAlpha3) {
            throw new NotImplementedException();
        }

        public void UploadSubtitle(ISubtitleUploadInfo info) {
            throw new NotImplementedException();
        }
    }

}