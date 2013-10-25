namespace SharpMediaInfo.Output.Properties.Delay {
    public class GeneralDelayInfo {
        protected readonly Media MediaStream;
        protected readonly bool DelayOriginal;

        public GeneralDelayInfo(Media media) : this(media, false){
        }

        internal GeneralDelayInfo(Media media, bool delayOriginal) {
            MediaStream = media;
            DelayOriginal = delayOriginal;
        }

        /// <summary>Delay with measurement</summary>
        public string String { get { return DelayOriginal ? MediaStream["Delay_Original/String"] : MediaStream["Delay/String"]; } }

        /// <summary>Delay with measurement</summary>
        public string String1 { get { return DelayOriginal ? MediaStream["Delay_Original/String1"] : MediaStream["Delay/String1"]; } }

        /// <summary>Delay with measurement</summary>
        public string String2 { get { return DelayOriginal ? MediaStream["Delay_Original/String2"] : MediaStream["Delay/String2"]; } }

        /// <summary>Delay in format: HH:MM:SS.MMM</summary>
        public string String3 { get { return DelayOriginal ? MediaStream["Delay_Original/String3"] : MediaStream["Delay/String3"]; } }         
    }
}