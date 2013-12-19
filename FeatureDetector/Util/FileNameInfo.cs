using System;
using System.Collections.Generic;
using Frost.Common.Util.ISO;

namespace Frost.DetectFeatures.Util {

    public enum DVDRegion {
        Unknown,
        R0,
        R1,
        R2,
        R3,
        R4,
        R5,
        R6,
        R7,
        R8,
    }

    public class FileNameInfo {
        public FileNameInfo(IEnumerable<string> segments) {
            UndetectedSegments = new List<string>(segments);
            Specials = new List<string>();
        }

        public List<string> UndetectedSegments { get; internal set; }

        public string Title { get; internal set; }
        public DateTime ReleaseYear { get; internal set; }
        public DVDRegion DVDRegion { get; internal set; }
        public string Genre { get; internal set; }
        public string Edithion { get; internal set; }
        public string ContentType { get; set; }

        #region Video

        public string VideoSource { get; internal set; }
        public string VideoQuality { get; internal set; }
        public string VideoCodec { get; internal set; }

        #endregion

        #region Audio

        public string AudioSource { get; internal set; }
        public string AudioCodec { get; internal set; }
        public string AudioQuality { get; internal set; }

        #endregion

        public int Part { get; internal set; }
        public string PartType { get; internal set; }

        #region Language / Subtitles

        public bool HasSubtitles { get; internal set; }
        public ISOLanguageCode SubtitleLanguage { get; internal set; }
        public ISOLanguageCode Language { get; internal set; }

        #endregion

        public string ReleaseGroup { get; internal set; }
        public List<string> Specials { get; internal set; }
    }
}