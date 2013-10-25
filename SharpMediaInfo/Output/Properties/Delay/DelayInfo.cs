namespace SharpMediaInfo.Output.Properties.Delay {
    public class DelayInfo : GeneralDelayInfo{
        internal DelayInfo(Media media, bool delayOriginal) : base(media, delayOriginal) {
        }

        /// <summary>Delay in format : HH:MM:SS:FF (HH:MM:SS</summary>
        public string String4 { get { return DelayOriginal ? MediaStream["Delay_Original/String4"] : MediaStream["Delay/String4"]; } }


        /// <summary>Delay settings (in case of timecode for example)</summary>
        public string Settings { get { return DelayOriginal ? MediaStream["Delay_Original_Settings"] : MediaStream["Delay_Settings"]; } }

        /// <summary>Delay drop frame</summary>
        public string DropFrame { get { return DelayOriginal ? MediaStream["Delay_Original_DropFrame"] : MediaStream["Delay_DropFrame"]; } }

        /// <summary>Delay source (Container or Stream or empty)</summary>
        public string Source { get { return DelayOriginal ? MediaStream["Delay_Original_Source"] : MediaStream["Delay_Source"]; } }

        /// <summary>Delay source (Container or Stream or empty)</summary>
        public string SourceString { get { return DelayOriginal ? MediaStream["Delay_Original_Source/String"] : MediaStream["Delay_Source/String"]; } }
    }
}