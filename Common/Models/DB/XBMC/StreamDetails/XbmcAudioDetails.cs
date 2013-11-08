using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.StreamDetails {

    /// <summary>Represents information about an audio stream in a file.</summary>
    public class XbmcAudioDetails : XbmcStreamDetails {

        /// <summary>Initializes a new instance of the <see cref="XbmcAudioDetails"/> class.</summary>
        /// <param name="codec">The codec this audio is encoded in.</param>
        /// <param name="channels">The number of audio channels.</param>
        /// <param name="language">The language of this audio.</param>
        public XbmcAudioDetails(string codec, long? channels, string language = null) : this(new XbmcFile(), codec, channels, language) {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcAudioDetails"/> class.</summary>
        /// <param name="file">The file that contains this audio stream.</param>
        /// <param name="codec">The codec this audio is encoded in.</param>
        /// <param name="channels">The number of audio channels.</param>
        /// <param name="language">The language of this audio.</param>
        public XbmcAudioDetails(XbmcFile file, string codec, long? channels, string language = null) {
            File = file;

            Codec = codec;
            Channels = channels;
            Language = language;            
        }

        /// <summary>Gets or sets the codec this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        [Column("strAudioCodec")]
        public string Codec { get; set; }

        /// <summary>Gets or sets the number of audio channels.</summary>
        /// <value>The number of audio channels.</value>
        [Column("iAudioChannels")]
        public long? Channels { get; set; }

        /// <summary>Gets or sets the language of this audio.</summary>
        /// <value>The language of this audio.</value>
        [Column("strAudioLanguage")]
        public string Language { get; set; }
    }
}