using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    [Table("settings")]
    public class XbmcSettings {

        [Key]
        [Column("idSetting")]
        public long Id { get; set; }

        [Column("idFile")]
        [ForeignKey("File")]
        public long FileId { get; set; }

        public bool Deinterlace { get; set; }
        public long ViewMode { get; set; }
        public double ZoomAmount { get; set; }
        public double PixelRatio { get; set; }
        public double VerticalShift { get; set; }
        public long AudioStream { get; set; }
        public long SubtitleStream { get; set; }
        public double SubtitleDelay { get; set; }
        public double SubtitlesOn { get; set; }
        public double Brightness { get; set; }
        public double Contrast { get; set; }
        public double Gamma { get; set; }
        public double VolumeAmplification { get; set; }
        public double AudioDelay { get; set; }
        public bool OutputToAllSpeakers { get; set; }
        public long ResumeTime { get; set; }
        public bool Crop { get; set; }
        public long CropLeft { get; set; }
        public long CropRight { get; set; }
        public long CropTop { get; set; }
        public long CropBottom { get; set; }
        public double Sharpness { get; set; }
        public double NoiseReduction { get; set; }

        [Column("NonLinStretch")]
        public bool NonLinearStretch { get; set; }

        public bool PostProcess { get; set; }
        public long ScalingMethod { get; set; }
        public long DeinterlaceMode { get; set; }

        public virtual XbmcFile File { get; set; }
    }
}