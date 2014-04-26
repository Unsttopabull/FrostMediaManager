using Frost.InfoParsers.Models;

namespace Frost.InfoParsers {

    public enum VideoType {
        Unknown,
        Trailer,
        Interview,
        Featurete,
        BehindTheScenes,
        TvSpot,
        Review,
        Clip
    }

    public class ParsedVideo : IParsedVideo {

        public VideoType Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Duration { get; set; }
        public string Language { get; set; }
        public string SubtitleLanguage { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0}: {1} ({2}{3})", Type, Title, Language, !string.IsNullOrEmpty(SubtitleLanguage) ? ", subs: " + SubtitleLanguage : "");
        }
    }

}