
namespace Frost.SharpMediaInfo.Output.Properties.FrameRate {

    public class VideoFrameRateInfo : FrameRateInfo {

        public VideoFrameRateInfo(Media media) : base(media) {
        }

        /// <summary>Original frame rate mode (CFR, VFR)</summary>
        public BitOrFrameRateMode ModeOriginal {
            get {
                switch (Media["FrameRate_Mode_Original"]) {
                    case "VBR":
                        return BitOrFrameRateMode.Variable;
                    case "CFR":
                        return BitOrFrameRateMode.Constant;
                    default:
                        return BitOrFrameRateMode.Unknown;
                }
            }
        }

        /// <summary>Original frame rate mode (Constant, Variable)</summary>
        public string ModeOriginalString { get { return Media["FrameRate_Mode_Original/String"]; } }

    }

}