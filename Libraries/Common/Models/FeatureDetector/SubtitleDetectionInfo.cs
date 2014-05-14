using Frost.Common.Util.ISO;

namespace Frost.Common.Models.FeatureDetector {

    /// <summary>The information about a subtitle as detected by Feature Detector</summary>
    public class SubtitleDetectionInfo {

        /// <summary>Initializes a new instance of the <see cref="SubtitleDetectionInfo"/> class.</summary>
        public SubtitleDetectionInfo() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="SubtitleDetectionInfo"/> class.</summary>
        /// <param name="lang">The language of the subtitle.</param>
        /// <param name="mediaFormat">The subtitle format.</param>
        public SubtitleDetectionInfo(ISOLanguageCode lang, string mediaFormat) {
            Language = lang;
            Format = mediaFormat;
        }

        /// <summary>Initializes a new instance of the <see cref="SubtitleDetectionInfo"/> class.</summary>
        /// <param name="lang">The language of the subtitle.</param>
        public SubtitleDetectionInfo(ISOLanguageCode lang) {
            Language = lang;
        }

        /// <summary>Gets or sets the MD5 Hash of the file.</summary>
        /// <value>The MD5 hash of the file.</value>
        public string MD5 { get; set; }

        /// <summary>Gets or sets the type or format of the subtitle.</summary>
        /// <value>The type or format of the subtitle.</value>
        public string Format { get; set; }

        /// <summary>Gets or sets the character set this subtitle is encoded in.</summary>
        /// <value>The character set this subtitle is encoded in</value>
        public string Encoding { get; set; }

        /// <summary>Gets or sets a value indicating whether this subtitle is embeded in the movie video.</summary>
        /// <value>Is <c>true</c> if this subtitle is embeded in the movie video; otherwise, <c>false</c>.</value>
        public bool EmbededInVideo { get; set; }

        /// <summary>Gets or sets a value indicating whether this subtitle is for people that are hearing impaired.</summary>
        /// <value>Is <c>true</c> if this subtitle is for people that are hearing impaired; otherwise, <c>false</c>.</value>
        public bool ForHearingImpaired { get; set; }

        /// <summary>Gets or sets the language of the subtitle.</summary>
        /// <value>The subtitle language.</value>
        public ISOLanguageCode Language { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Language != null
                ? Language.EnglishName
                : base.ToString();
        }
    }

}