namespace SharpMediaInfo.Output.Properties {
    public class GeneralBitRateInfo {
        protected readonly Media MediaStream;
        private readonly bool _general;

        internal GeneralBitRateInfo(Media media) : this(media, true) {

        }

        internal GeneralBitRateInfo(Media media, bool general) {
            MediaStream = media;
            _general = general;
        }

        /// <summary>Bit rate mode (VBR, CBR)</summary>
        public string Mode { get { return _general ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Bit rate mode (Constant, Variable)</summary>
        public string ModeString { get { return _general ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Bit rate (with measurement)</summary>
        public string String { get { return _general ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Minimum Bit rate in bps</summary>
        public string Minimum { get { return _general ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Minimum Bit rate (with measurement)</summary>
        public string MinimumString { get { return _general ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Nominal Bit rate in bps</summary>
        public string Nominal { get { return _general ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Nominal Bit rate (with measurement)</summary>
        public string NominalString { get { return _general ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Maximum Bit rate in bps</summary>
        public string Maximum { get { return _general ? MediaStream[""] : MediaStream[""]; } }

        /// <summary>Maximum Bit rate (with measurement)</summary>
        public string MaximumString { get { return _general ? MediaStream[""] : MediaStream[""]; } }
    }
}