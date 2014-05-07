using Frost.InfoParsers.Models.Subtitles;

namespace Frost.InfoParsers {
    public class SubtitleInfo : ISubtitleInfo {

        /// <summary>Url to the subtitle file to download.</summary>
        /// <value>The subtitle download link.</value>
        public string SubtitleFileDownloadLink { get; private set; }

        /// <summary>Url to the subtitle file to download.</summary>
        /// <value>The subtitle download link.</value>
        public string SubtitleZipDownloadLink { get; set; }

        /// <summary>Url to the GZip Compressed subtitle file to download.</summary>
        /// <value>The subtitle gzip download link.</value>
        public string SubtitleGZipDownloadLink { get; set; }

        /// <summary>Url to the site where to download the subtitles</summary>
        /// <value>The subtitles link.</value>
        public string SubtitlesLink { get; set; }
        public string SubtitleId { get; set; }

        /// <summary>Gets the subtitle format (sub, srt, idx ...).</summary>
        /// <value>The subtitle format.</value>
        public string SubtitleFormat { get; set; }

        /// <summary>Gets the subtitle MD5 hash.</summary>
        /// <value>The subtitle hash.</value>
        public string SubtitleHash { get; set; }
        public string FileName { get; set; }
        public long? SubtitleByteSize { get; set; }

        /// <summary>Gets the subtitle part number (eg. 1 for CD1 etc.).</summary>
        /// <value>The part number.</value>
        public int? PartNumber { get; set; }

        public int? NumberOfParts { get; private set; }
        public int? DownloadCount { get; set; }
        public double? Rating { get; set; }
        public string UploaderName { get; set; }
        public string UploaderComment { get; set; }
        public string ISO639Language { get; set; }
        public string LanguageName { get; private set; }
        public bool? IsForHearingImpaired { get; set; }
        public string MovieName { get; set; }
        public string MovieRelease { get; set; }
        public int? MovieReleaseYear { get; set; }
    }
}
