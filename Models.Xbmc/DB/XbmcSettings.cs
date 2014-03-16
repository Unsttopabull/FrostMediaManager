using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Providers.Xbmc.DB {

    /// <summary>This table stores XBMC settings for individual files.</summary>
    [Table("settings")]
    public class XbmcSettings {
        #region Properties / Columns

        /// <summary>Gets or sets the id of this setting in the database.</summary>
        /// <value>The id of this setting in the database.</value>
        [Key]
        [Column("idSetting")]
        public long Id { get; set; }

        /// <summary>Gets or sets the foreign key to the file this setting is for.</summary>
        /// <value>The foreign key to the file this setting is for.</value>
        [Column("idFile")]
        [ForeignKey("File")]
        public long FileId { get; set; }

        /// <summary>Gets or sets a value indicating whether the file should be deinterlaced.</summary>
        /// <value>Is <c>true</c> if the file should be deinterlaced; otherwise, <c>false</c>.</value>
        public bool Deinterlace { get; set; }

        /// <summary>Gets or sets the view mode for this file (Zoom / Stretch).</summary>
        /// <value>The view mode for this file  (Zoom / Stretch).</value>
        /// <example>\eg{<c>"16:9"</c>, <c>"4:3"</c>, <c>"Zoom"</c> or <c>"Custom"</c>}</example>
        /// <seealso cref="XbmcViewMode"/>
        public XbmcViewMode ViewMode { get; set; }

        /// <summary>Gets or sets the zoom amount (how much should the video be zoomed in or out).</summary>
        /// <value>The zoom amount (how much should the video be zoomed in or out)</value>
        /// <remarks>Above <c>1.0</c> to zoom in and below <c>1.0</c> to zoom out. Max: <c>2.0</c> and Min: <c>0.5</c>;</remarks>
        /// <example>\eg{ <c>"1.5"</c> to zoom in and <c>"0.6"</c> to zoom out.}</example>
        public double ZoomAmount { get; set; }

        /// <summary>Gets or sets the pixel ratio.</summary>
        /// <value>The pixel ratio.</value>
        public double PixelRatio { get; set; }

        /// <summary>Gets or sets for how much should the video be shifted verticaly when playing (- up, + down).</summary>
        /// <value>For how much should the video be shifted verticaly when playing  (- up, + down).</value>
        public double VerticalShift { get; set; }

        /// <summary>Gets or sets the default audio stream number to use when playing.</summary>
        /// <value>The default audio stream number to use when playing.</value>
        /// <remarks>If the file does not have multiple or any audio streams its set to <c>"-1"</c>.</remarks>
        public long AudioStream { get; set; }

        /// <summary>Gets or sets the default subtitle stream number to use when playing.</summary>
        /// <value>The default subtitle stream number to use when playing.</value>
        /// <remarks>If the file does not have multiple or any streams its set to <c>"-1"</c>.</remarks>
        public long SubtitleStream { get; set; }

        /// <summary>Gets or sets the subtitle delay in seconds.</summary>
        /// <value>The subtitle delay in seconds.</value>
        public double SubtitleDelay { get; set; }

        /// <summary>Gets or sets a value indicating whether to show subtitles by default.</summary>
        /// <value>Is <c>true</c> if the subtitles are to be shown by default; otherwise, <c>false</c>.</value>
        public bool SubtitlesOn { get; set; }

        /// <summary>Gets or sets the brightness.</summary>
        /// <value>The brightness.</value>
        public double Brightness { get; set; }

        /// <summary>Gets or sets the contrast.</summary>
        /// <value>The contrast.</value>
        public double Contrast { get; set; }

        /// <summary>Gets or sets the gamma.</summary>
        /// <value>The gamma.</value>
        public double Gamma { get; set; }

        /// <summary>Gets or sets the volume amplification in Db.</summary>
        /// <value>The volume amplification in Db.</value>
        public double VolumeAmplification { get; set; }

        /// <summary>Gets or sets the audio delay in seconds.</summary>
        /// <value>The audio delay in secondss.</value>
        public double AudioDelay { get; set; }

        /// <summary>Gets or sets a value indicating whether to output stereo to all speakers.</summary>
        /// <value>Is <c>true</c> if to output stereo to all speakers; otherwise, <c>false</c>.</value>
        public bool OutputToAllSpeakers { get; set; }

        /// <summary>Gets or sets the resume time.</summary>
        /// <value>The resume time.</value>
        public long ResumeTime { get; set; }

        /// <summary>Gets or sets a value indicating whether to crop the video.</summary>
        /// <value>Is <c>true</c> if crop should occur; otherwise, <c>false</c>.</value>
        public bool Crop { get; set; }

        /// <summary>Gets or sets the value of how much should be cropped from the left.</summary>
        /// <value>The value of how much should be cropped from the left</value>
        public long CropLeft { get; set; }

        /// <summary>Gets or sets the value of how much should be cropped from the right.</summary>
        /// <value>The value of how much should be cropped from the right</value>
        public long CropRight { get; set; }

        /// <summary>Gets or sets the value of how much should be cropped from the top.</summary>
        /// <value>The value of how much should be cropped from the top</value>
        public long CropTop { get; set; }

        /// <summary>Gets or sets the value of how much should be cropped from the bottom.</summary>
        /// <value>The value of how much should be cropped from the bottom</value>
        public long CropBottom { get; set; }

        /// <summary>Gets or sets the sharpness.</summary>
        /// <value>The sharpness.</value>
        public double Sharpness { get; set; }

        /// <summary>Gets or sets the noise reduction value.</summary>
        /// <value>The noise reduction value.</value>
        public double NoiseReduction { get; set; }

        /// <summary>Gets or sets a value indicating whether the video should be non linearly stretched (different ratios for height and width).</summary>
        /// <value>Is <c>true</c> if the video is to be non linearly stretched; otherwise, <c>false</c>.</value>
        [Column("NonLinStretch")]
        public bool NonLinearStretch { get; set; }

        /// <summary>Gets or sets a value indicating whether the file should be post processed.</summary>
        /// <value>Is <c>true</c> if the file should be post processed; otherwise, <c>false</c>.</value>
        public bool PostProcess { get; set; }

        /// <summary>Gets or sets the scaling method used for this settings.</summary>
        /// <value>The scaling method used for this settings.</value>
        public XbmcScalingMethod ScalingMethod { get; set; }

        /// <summary>Gets or sets the deinterlace mode.</summary>
        /// <value>The deinterlace mode.</value>
        /// <seealso cref="XbmcDeinterlaceMode"/>
        public XbmcDeinterlaceMode DeinterlaceMode { get; set; }

        #endregion

        /// <summary>Gets or sets the file this setting is for.</summary>
        /// <value>The file this setting is for.</value>
        public virtual XbmcFile File { get; set; }

    }

}
