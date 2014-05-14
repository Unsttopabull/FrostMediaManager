namespace Frost.Common.Models.FeatureDetector {

    /// <summary>The information about a promotional video clip as detected by Feature Detector</summary>
    public class PromotionalVideoInfo {

        /// <summary>Initializes a new instance of the <see cref="PromotionalVideoInfo"/> class.</summary>
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

        /// <summary>Gets or sets the clip type.</summary>
        /// <value>The clip type.</value>
        public PromotionalVideoType Type { get; set; }

        /// <summary>Gets or sets the clip title.</summary>
        /// <value>The clip title.</value>
        public string Title { get; set; }

        /// <summary>Gets or sets the clip URL.</summary>
        /// <value>The clip URL.</value>
        public string Url { get; set; }

        /// <summary>Gets or sets the clip duration (eg. 5:30 meaning 5 minutes 30 seconds).</summary>
        /// <value>The clip duration.</value>
        public string Duration { get; set; }

        /// <summary>Gets or sets the name of the language used in the clip.</summary>
        /// <value>The language used in the clip.</value>
        public string Language { get; set; }

        /// <summary>Gets or sets the langauge of the subtitles used in the clip.</summary>
        /// <value>The langauge of the subtitles used in the clip</value>
        public string SubtitleLanguage { get; set; }
    }

}