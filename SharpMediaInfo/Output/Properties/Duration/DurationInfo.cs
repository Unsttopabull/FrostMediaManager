namespace SharpMediaInfo.Output.Properties.Duration {
    public class DurationInfo {
        protected readonly Media MediaStream;
        protected readonly bool OriginalDuration;

        public DurationInfo(Media media, bool originalDuration) {
            MediaStream = media;
            OriginalDuration = originalDuration;
        }
        
        /// <summary>Play time in format : XXx YYy only, YYy omited if zero</summary>
        public string String { get { return OriginalDuration ? MediaStream["Source_Duration/String"] : MediaStream["Duration/String"]; } }

        /// <summary>Play time in format : HHh MMmn SSs MMMms, XX omited if zero</summary>
        public string String1 { get { return OriginalDuration ? MediaStream["Source_Duration/String1"] : MediaStream["Duration/String1"]; } }

        /// <summary>Play time in format : XXx YYy only, YYy omited if zero</summary>
        public string String2 { get { return OriginalDuration ? MediaStream["Source_Duration/String2"] : MediaStream["Duration/String2"]; } }

        /// <summary>Play time in format : HH:MM:SS.MMM</summary>
        public string String3 { get { return OriginalDuration ? MediaStream["Source_Duration/String3"] : MediaStream["Duration/String3"]; } }
    }
}