namespace SharpMediaInfo.Output.Properties {
    public class FrameRateInfo {
        private readonly Media _media;

        public FrameRateInfo(Media media) {
            _media = media;
        }

        /// <summary>Frames per second (with measurement)</summary>
        public string String { get { return _media[""]; } }

        /// <summary>Original (in the raw stream) frames per second</summary>
        public string Original { get { return _media[""]; } }
        /// <summary>Original (in the raw stream) frames per second</summary>
        public string OriginalString { get { return _media[""]; } }

        /// <summary>Frame rate mode (CFR, VFR)</summary>
        public string Mode { get { return _media[""]; } }
        /// <summary>Frame rate mode (Constant, Variable)</summary>
        public string ModeString { get { return _media[""]; } }

        /// <summary>Original frame rate mode (CFR, VFR)</summary>
        public string ModeOriginal { get { return _media[""]; } }
        /// <summary>Original frame rate mode (Constant, Variable)</summary>
        public string ModeOriginalString { get { return _media[""]; } }

        /// <summary>Minimum Frames per second</summary>
        public string Minimum { get { return _media[""]; } }
        /// <summary>Minimum Frames per second (with measurement)</summary>
        public string MinimumString { get { return _media[""]; } }

        /// <summary>Nominal Frames per second</summary>
        public string Nominal { get { return _media[""]; } }
        /// <summary>Nominal Frames per second (with measurement)</summary>
        public string NominalString { get { return _media[""]; } }

        /// <summary>Maximum Frames per second</summary>
        public string Maximum { get { return _media[""]; } }
        /// <summary>Maximum Frames per second (with measurement)</summary>
        public string MaximumString { get { return _media[""]; } }
    }
}