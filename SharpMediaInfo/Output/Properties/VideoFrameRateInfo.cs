
namespace Frost.SharpMediaInfo.Output.Properties {

    public class VideoFrameRateInfo : FrameRateInfo {

        public VideoFrameRateInfo(Media media) : base(media) {
        }

        /// <summary>Original frame rate mode (CFR, VFR)</summary>
        public string ModeOriginal { get { return Media["FrameRate_Mode_Original"]; } }

        /// <summary>Original frame rate mode (Constant, Variable)</summary>
        public string ModeOriginalString { get { return Media["FrameRate_Mode_Original/String"]; } }

    }

}