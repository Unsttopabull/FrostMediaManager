namespace Frost.Common.Models.FeatureDetector {

    public class PromotionalVideoInfo {

        public PromotionalVideoInfo() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public PromotionalVideoInfo(PromotionalVideoType type, string title, string url, string duration, string language, string subtitleLanguage) {
            Type = type;
            Title = title;
            Url = url;
            Duration = duration;
            Language = language;
            SubtitleLanguage = subtitleLanguage;
        }

        public PromotionalVideoType Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Duration { get; set; }
        public string Language { get; set; }
        public string SubtitleLanguage { get; set; }
    }

}