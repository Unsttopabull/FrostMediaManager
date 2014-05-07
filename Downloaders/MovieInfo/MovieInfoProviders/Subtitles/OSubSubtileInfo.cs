using Frost.InfoParsers;
using Frost.InfoParsers.Models.Subtitles;
using SubtitleInfo = Frost.SharpOpenSubtitles.Models.Search.SubtitleInfo;

namespace Frost.MovieInfoProviders.Subtitles {

    public class OSubSubtileInfo : ISubtitleInfo {
        private readonly SubtitleInfo _info;
        private long? _byteSize;
        private int? _partNum;
        private int? _dlCount;
        private double? _rating;
        private int? _releaseYear;
        private bool? _hearingImpaired;
        private int? _numParts;

        public OSubSubtileInfo(SubtitleInfo info) {
            _info = info;
        }


        /// <summary>Url to the subtitle file to download.</summary>
        /// <value>The subtitle download link.</value>
        public string SubtitleFileDownloadLink { get; private set; }

        /// <summary>Url to the subtitle file to download.</summary>
        /// <value>The subtitle download link.</value>
        public string SubtitleZipDownloadLink {
            get { return _info.SubtitleZipDownloadLink; }
        }

        /// <summary>Url to the GZip Compressed subtitle file to download.</summary>
        /// <value>The subtitle gzip download link.</value>
        public string SubtitleGZipDownloadLink {
            get { return _info.SubtitleGGzipDownloadLink; }
        }

        /// <summary>Url to the site where to download the subtitles</summary>
        /// <value>The subtitles link.</value>
        public string SubtitlesLink {
            get { return _info.SubtitlesLink; }
        }

        public string SubtitleId {
            get { return _info.SubtitleId; }
        }

        /// <summary>Gets the subtitle format (sub, srt, idx ...).</summary>
        /// <value>The subtitle format.</value>
        public string SubtitleFormat {
            get { return _info.SubtitleFormat; }
        }

        /// <summary>Gets the subtitle MD5 hash.</summary>
        /// <value>The subtitle hash.</value>
        public string SubtitleHash {
            get { return _info.SubtitleHash; }
        }

        public string FileName {
            get { return _info.SubtitleFileName; }
        }

        public long? SubtitleByteSize {
            get {
                if (_byteSize.HasValue) {
                    return _byteSize;
                }
                _byteSize = _info.SubtitleSize.TryGetLong();
                return _byteSize;
            }
        }

        /// <summary>Gets the subtitle part number (eg. 1 for CD1 etc.).</summary>
        /// <value>The part number.</value>
        public int? PartNumber {
            get {
                if (_partNum.HasValue) {
                    return _partNum;
                }
                _partNum = _info.PartNumber.TryGetInt();
                return _partNum;
            }
        }

        public int? NumberOfParts {
            get {
                if (_numParts.HasValue) {
                    return _numParts;
                }
                _numParts = _info.NumParts.TryGetInt();
                return _numParts;
            }
        }

        public int? DownloadCount {
            get {
                if (_dlCount.HasValue) {
                    return _dlCount;
                }
                _dlCount = _info.DownloadCount.TryGetInt();
                return _dlCount;
            }
        }

        public double? Rating {
            get {
                if (_rating.HasValue) {
                    return _rating;
                }

                _rating = _info.Rating.TryGetDouble();
                //if (rating.HasValue) {
                //    _rating = rating / 2.0;
                //}

                return _rating;
            }
        }

        public string UploaderName {
            get { return string.IsNullOrEmpty(_info.UploaderName) ? null : _info.UploaderName; }
        }

        public string UploaderComment {
            get { return string.IsNullOrEmpty(_info.AuthorComment) ? null : _info.AuthorComment; }
        }

        public string ISO639Language {
            get { return _info.ISO639; }
        }

        public string LanguageName {
            get { return _info.LanguageName; }
        }

        public bool? IsForHearingImpaired {
            get {
                if (_hearingImpaired.HasValue) {
                    return _hearingImpaired;
                }
                _hearingImpaired = _info.IsForHearingImpaired.TryGetBool();
                return _hearingImpaired;
            }
        }

        public string MovieName {
            get { return _info.MovieName; }
        }

        public string MovieRelease {
            get { return string.IsNullOrEmpty(_info.MovieReleaseName) ? null : _info.MovieReleaseName; }
        }

        public int? MovieReleaseYear {
            get {
                if (_releaseYear.HasValue) {
                    return _releaseYear;
                }
                _releaseYear = _info.ReleaseYear.TryGetInt();
                return _releaseYear;
            }
        }
    }

}