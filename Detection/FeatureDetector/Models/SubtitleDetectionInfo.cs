using Frost.Common.Util.ISO;

namespace Frost.DetectFeatures.Models {

    public class SubtitleDetectionInfo {

        public SubtitleDetectionInfo() {
            
        }

        public SubtitleDetectionInfo(ISOLanguageCode lang, string mediaFormat) {
            Language = lang;
            Format = mediaFormat;
        }

        public SubtitleDetectionInfo(ISOLanguageCode lang) {
            Language = lang;
        }

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

        public ISOLanguageCode Language { get; set; }
    }

}