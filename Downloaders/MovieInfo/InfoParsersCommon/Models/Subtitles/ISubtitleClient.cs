using System.Collections.Generic;

namespace Frost.InfoParsers.Models.Subtitles {

    public interface ISubtitleClient : IInfoClient {
        IEnumerable<ISubtitleInfo> GetMovieSubtitlesFromTitle(string title, int year, string languageAlpha3);
        IEnumerable<ISubtitleInfo> GetMovieSubtitlesFromImdbId(string imdbId, string languageAlpha3);

        IEnumerable<ISubtitleInfo> GetSubtitlesByMovieHash(IEnumerable<IMovieHash> movieHashes, string languageAlpha3);
        IEnumerable<ISubtitleFile> CheckSubtitleExistsByMD5(IEnumerable<IMovieHash> movieHashes, string languageAlpha3);

        void UploadSubtitle(ISubtitleUploadInfo info);
    }

}