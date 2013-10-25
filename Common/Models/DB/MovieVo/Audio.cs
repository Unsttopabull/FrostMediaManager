using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo {

    public class Audio {

        public Audio(string source, string type, string channels, string codec, string language) {
            Source = source;
            Type = type;
            Channels = channels;
            Codec = codec;
            Language = language;
        }

        public Audio(string source, string type, string channels, string codec)
            : this(source, type, channels, codec, null) {
        }

        public Audio(string codec, string channels, string language) {
            Codec = codec;
            Channels = channels;
            Language = language;
        }

        [Key]
        public long Id { get; set; }

        public string Source { get; set; }

        public string Type { get; set; }

        public string Channels { get; set; }

        public string Codec { get; set; }

        public string Language { get; set; }

        public long MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        public static explicit operator XbmcXmlAudioInfo(Audio audio) {
            return new XbmcXmlAudioInfo(audio.Codec,audio.Channels, audio.Language);
        }
    }
}
