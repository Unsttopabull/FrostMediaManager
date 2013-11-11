namespace Frost.SharpMediaInfo.Output.Properties.BitRate {
    public class GeneralBitRateInfo {
        protected readonly Media MediaStream;
        private readonly bool _general;

        internal GeneralBitRateInfo(Media media, bool general = true) {
            MediaStream = media;
            _general = general;
        }

        /// <summary>Bit rate mode (VBR, CBR)</summary>
        public string Mode { get { return _general ? MediaStream["OverallBitRate_Mode"] : MediaStream["BitRate_Mode"]; } }

        /// <summary>Bit rate mode (Constant, Variable)</summary>
        public string ModeString { get { return _general ? MediaStream["OverallBitRate_Mode/String"] : MediaStream["BitRate_Mode/String"]; } }

        /// <summary>Bit rate (with measurement)</summary>
        public string String { get { return _general ? MediaStream["OverallBitRate/String"] : MediaStream["BitRate/String"]; } }

        /// <summary>Minimum Bit rate in bps</summary>
        public string Minimum { get { return _general ? MediaStream["OverallBitRate_Minimum"] : MediaStream["BitRate_Minimum"]; } }

        /// <summary>Minimum Bit rate (with measurement)</summary>
        public string MinimumString { get { return _general ? MediaStream["OverallBitRate_Minimum/String"] : MediaStream["BitRate_Minimum/String"]; } }

        /// <summary>Nominal Bit rate in bps</summary>
        public string Nominal { get { return _general ? MediaStream["OverallBitRate_Nominal"] : MediaStream["BitRate_Nominal"]; } }

        /// <summary>Nominal Bit rate (with measurement)</summary>
        public string NominalString { get { return _general ? MediaStream["OverallBitRate_Nominal/String"] : MediaStream["BitRate_Nominal/String"]; } }

        /// <summary>Maximum Bit rate in bps</summary>
        public string Maximum { get { return _general ? MediaStream["OverallBitRate_Maximum"] : MediaStream["BitRate_Maximum"]; } }

        /// <summary>Maximum Bit rate (with measurement)</summary>
        public string MaximumString { get { return _general ? MediaStream["OverallBitRate_Maximum/String"] : MediaStream["BitRate_Maximum/String"]; } }
    }
}