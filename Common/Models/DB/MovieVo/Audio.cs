using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models.XML.XBMC;

namespace Common.Models.DB.MovieVo {

    public class Audio {

        public Audio(string source, string type, string codec, string channels, string language) {
            File = new File();
            Movie = new Movie();

            Source = source;
            Type = type;
            Channels = channels;
            Codec = codec;
            Language = language;
        }

        public Audio(string source, string type, string channels, string codec) : this(source, type, codec, channels, null) {
        }

        public Audio(string codec, string channels, string language) : this(null, null, codec, channels, language){
        }

        [Key]
        public long Id { get; set; }

        /// <summary>The source of the audio</summary>
        /// <example>LD MD LINE MIC</example>
        public string Source { get; set; }

        ///<summary>The type of the audio</summary>
        ///<example>AC3 DTS</example>
        public string Type { get; set; }

        public string Channels { get; set; }

        public string ChannelPositions { get; set; }

        public string Codec { get; set; }

        /// <summary>Gets or sets the audio bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public long BitRate { get; set; }

        /// <summary>Gets or sets the audio bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        /// <example>Constant, Variable</example>
        public string BitRateMode { get; set; }

        /// <summary>Gets or sets the audio sampling rate.</summary>
        /// <value>The sampling rate in KHz.</value>
        public long SamplingRate { get; set; }

        /// <summary>Gets or sets the audio bit depth.</summary>
        /// <value>The audio depth in bits.</value>
        public long BitDepth { get; set; }

        public string CompressionMode { get; set; }

        /// <summary>Gets or sets the audio duration.</summary>
        /// <value>The audio duration in miliseconds.</value>
        public long Duration { get; set; }

        public string Language { get; set; }

        public long MovieId { get; set; }

        public long FileId { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        public static explicit operator XbmcXmlAudioInfo(Audio audio) {
            return new XbmcXmlAudioInfo(audio.Codec,audio.Channels, audio.Language);
        }
    }
}
