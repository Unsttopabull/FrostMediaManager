using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.StreamDetails {
    public class XbmcAudioDetails : XbmcStreamDetails {

        [Column("strAudioCodec")]
        public string AudioCodec { get; set; }

        [Column("iAudioChannels")]
        public long? AudioChannels { get; set; }

        [Column("strAudioLanguage")]
        public string AudioLanguage { get; set; }
    }
}