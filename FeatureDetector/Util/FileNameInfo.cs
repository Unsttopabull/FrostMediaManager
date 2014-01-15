using System;
using System.Collections.Generic;
using System.Globalization;
using Frost.Common;
using Frost.Common.Util.ISO;

namespace Frost.DetectFeatures.Util {



    public class FileNameInfo {
        private string _title;

        public FileNameInfo(string filePath, string fileOrFolderName, IEnumerable<string> segments) {
            FilePath = filePath;
            FileOrFolderName = fileOrFolderName;
            UndetectedSegments = new List<string>(segments);
            Specials = new List<string>();
        }

        public List<string> UndetectedSegments { get; private set; }

        public string FilePath { get; private set; }
        public string FileOrFolderName { get; private set; }

        public string Title {
            get { return _title; }
            internal set {
                _title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim());
            }
        }

        public DateTime ReleaseYear { get; internal set; }
        public DVDRegion DVDRegion { get; internal set; }
        public string Genre { get; internal set; }
        public string Edithion { get; internal set; }
        public string ContentType { get; internal set; }

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
        public List<string> Specials { get; private set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            if (Part != 0) {
                if (ReleaseYear != default(DateTime)) {
                    return string.Format("{0} [{1}] ({2} {3})", Title, ReleaseYear.Year, PartType, Part);
                }
                return string.Format("{0} ({1} {2})", Title, PartType, Part);
            }
            if (ReleaseYear != default(DateTime)) {
                return string.Format("{0} [{1}]", Title, ReleaseYear.Year);
            }
            return Title;
        }
    }
}