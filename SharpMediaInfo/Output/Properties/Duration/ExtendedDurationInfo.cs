namespace Frost.MediaInfo.Output.Properties.Duration {
    public class ExtendedDurationInfo : DurationInfo {

        public ExtendedDurationInfo(Media media, bool source) : base(media, source) {
        }

        /// <summary>Duration of the first frame if it is longer than others, in ms</summary>
        public string FirstFrame { get { return OriginalDuration ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Duration of the first frame if it is longer than others, in format : XXx YYy only, YYy omited if zero</summary>
        public string FirstFrameString { get { return OriginalDuration ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Duration of the first frame if it is longer than others, in format : HHh MMmn SSs MMMms, XX omited if zero</summary>
        public string FirstFrameString1 { get { return OriginalDuration ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Duration of the first frame if it is longer than others, in format : XXx YYy only, YYy omited if zero</summary>
        public string FirstFrameString2 { get { return OriginalDuration ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Duration of the first frame if it is longer than others, in format : HH:MM:SS.MMM</summary>
        public string FirstFrameString3 { get { return OriginalDuration ? MediaStream[""] : MediaStream[""]; } }



        /// <summary>Duration of the last frame if it is longer than others, in ms</summary>
        public string LastFrame { get { return OriginalDuration ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Duration of the last frame if it is longer than others, in format : XXx YYy only, YYy omited if zero</summary>
        public string LastFrameString { get { return OriginalDuration ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Duration of the last frame if it is longer than others, in format : HHh MMmn SSs MMMms, XX omited if zero</summary>
        public string LastFrameString1 { get { return OriginalDuration ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Duration of the last frame if it is longer than others, in format : XXx YYy only, YYy omited if zero</summary>
        public string LastFrameString2 { get { return OriginalDuration ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Duration of the last frame if it is longer than others, in format : HH:MM:SS.MMM</summary>
        public string LastFrameString3 { get { return OriginalDuration ? MediaStream[""] : MediaStream[""]; } }
    }
}