namespace Frost.InfoParsers.Models.Subtitles {

    public interface ISubtitleInfo {

        /// <summary>Url to the subtitle file to download.</summary>
        /// <value>The subtitle download link.</value>
        string SubtitleDownloadLink { get; }

        /// <summary>Url to the GZip Compressed subtitle file to download.</summary>
        /// <value>The subtitle gzip download link.</value>
        string SubtitleGzipDownloadLink { get; }

        /// <summary>Url to the site where to download the subtitles</summary>
        /// <value>The subtitles link.</value>
        string SubtitlesLink { get; }

        string SubtitleId { get; }

        /// <summary>Gets the subtitle format (sub, srt, idx ...).</summary>
        /// <value>The subtitle format.</value>
        string SubtitleFormat { get; }

        /// <summary>Gets the subtitle MD5 hash.</summary>
        /// <value>The subtitle hash.</value>
        string SubtitleHash { get; }

        string FileName { get; }

        long SubtitleByteSize { get; }

        /// <summary>Gets the subtitle part number (eg. 1 for CD1 etc.).</summary>
        /// <value>The part number.</value>
        int PartNumber { get; }

        int DownloadCount { get; }

        double Rating { get; }

        string UploaderName { get; }

        string UploaderComment { get; }

        string ISO639Language { get; }

        bool IsForHearingImpaired { get; }

        string MovieName { get; }
        string MovieRelease { get; }

        int MovieReleaseYear { get; }
    }

}