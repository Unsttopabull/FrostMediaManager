using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.InfoParsers;
using Frost.InfoParsers.Models.Info;

namespace RibbonUI.Design.Models {
    public class DesignPromotionalVideo : IPromotionalVideo {

        public DesignPromotionalVideo(IParsedVideo parsedVideo) {
            switch (parsedVideo.Type) {
                case VideoType.Trailer:
                    Type = PromotionalVideoType.Trailer;
                    break;
                case VideoType.Interview:
                    Type = PromotionalVideoType.Interview;
                    break;
                case VideoType.Featurete:
                    Type = PromotionalVideoType.Featurete;
                    break;
                case VideoType.BehindTheScenes:
                    Type = PromotionalVideoType.BehindTheScenes;
                    break;
                case VideoType.TvSpot:
                    Type = PromotionalVideoType.TvSpot;
                    break;
                case VideoType.Review:
                    Type = PromotionalVideoType.Review;
                    break;
                case VideoType.Clip:
                    Type = PromotionalVideoType.Clip;
                    break;
                default:
                    Type = PromotionalVideoType.Unknown;
                    break;
            }

            Title = parsedVideo.Title;
            Duration = parsedVideo.Duration;
            Language = parsedVideo.Language;
            SubtitleLanguage = parsedVideo.SubtitleLanguage;
            Url = parsedVideo.Url;
        }

        /// <summary>Unique identifier.</summary>
        public long Id { get; private set; }

        /// <summary>Gets the value whether the property is editable.</summary>
        /// <value>The <see cref="System.Boolean"/> if the value is editable.</value>
        /// <param name="propertyName">Name of the property to check.</param>
        /// <returns>Returns <c>true</c> if property is editable, otherwise <c>false</c> (Not implemented or read-only).</returns>
        public bool this[string propertyName] {
            get { return true; }
        }

        public PromotionalVideoType Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Duration { get; set; }
        public string Language { get; set; }
        public string SubtitleLanguage { get; set; }
    }
}
