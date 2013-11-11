namespace Frost.SharpMediaInfo.Output.Properties.Delay {
    public class VideoDelayInfo : GeneralDelayInfo {
        internal VideoDelayInfo(Media media) : base(media) {
        }

        /// <summary>Delay in format : HH:MM:SS:FF (HH:MM:SS</summary>
        public string String4 { get { return DelayOriginal ? MediaStream["Delay_Original/String4"] : MediaStream["Delay/String4"]; } }
    }
}