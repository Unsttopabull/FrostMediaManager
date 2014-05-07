using System.Collections.Generic;

namespace Frost.InfoParsers.Models.Subtitles {

    public interface ISubtitleClient : IInfoClient {

        bool IsMovieHashSupported { get; }

        IEnumerable<ISubtitleInfo> GetMovieSubtitlesFromTitle(string title, int year, IEnumerable<string> languageAlpha3);
        IEnumerable<ISubtitleInfo> GetMovieSubtitlesFromImdbId(string imdbId, IEnumerable<string> languageAlpha3);

        IEnumerable<ISubtitleInfo> GetSubtitlesByMovieHash(IEnumerable<IMovieHash> movieHashes, IEnumerable<string> languageAlpha3);
        IEnumerable<ISubtitleFile> CheckSubtitleExistsByMD5(IEnumerable<string> hashes, IEnumerable<string> languageAlpha3);

        void UploadSubtitle(ISubtitleUploadInfo info);
    }

}