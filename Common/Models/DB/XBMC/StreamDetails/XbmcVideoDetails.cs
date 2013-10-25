using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.StreamDetails {
    public class XbmcVideoDetails : XbmcStreamDetails {

        [Column("strVideoCodec")]
        public string VideoCodec { get; set; }

        [Column("fVideoAspect")]
        public double? VideoAspect { get; set; }

        [Column("iVideoWidth")]
        public long? VideoWidth { get; set; }

        [Column("iVideoHeight")]
        public long? VideoHeight { get; set; }

        [Column("iVideoDuration")]
        public long? VideoDuration { get; set; }
    }
}