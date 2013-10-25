#pragma warning disable 1591

namespace SharpMediaInfo.Output.Properties.Formats {
    public class AudioFormat : Format {
        internal AudioFormat(Media media) : base(media) {
        }

        public string SBR { get { return MediaStream[""]; } }
        public string SBRString { get { return MediaStream[""]; } }
        public string PS { get { return MediaStream[""]; } }
        public string PSString { get { return MediaStream[""]; } }
        public string Mode { get { return MediaStream[""]; } }
        public string ModeExtension { get { return MediaStream[""]; } }
        public string Emphasis { get { return MediaStream[""]; } }
        public string Floor { get { return MediaStream[""]; } }
        public string Firm { get { return MediaStream[""]; } }
        public string Endianness { get { return MediaStream[""]; } }
        public string Sign { get { return MediaStream[""]; } }
        public string Law { get { return MediaStream[""]; } }
        public string ITU { get { return MediaStream[""]; } }
        public string Wrapping { get { return MediaStream[""]; } }
    }
}