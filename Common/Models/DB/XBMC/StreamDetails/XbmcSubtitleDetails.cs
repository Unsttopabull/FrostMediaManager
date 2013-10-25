using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.StreamDetails {
    public class XbmcSubtitleDetails : XbmcStreamDetails {

        [Column("strSubtitleLanguage")]
        public string SubtitleLanguage { get; set; }
    }
}