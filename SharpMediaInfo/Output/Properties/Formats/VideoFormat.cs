namespace SharpMediaInfo.Output.Properties.Formats {
    public class VideoFormat : Format {
        internal VideoFormat(Media media) : base(media) {
        }

        public string BVOP { get { return MediaStream[""]; } }
        public string BVOPString { get { return MediaStream[""]; } }
        public string QPel { get { return MediaStream[""]; } }
        public string QPelString { get { return MediaStream[""]; } }
        public string GMC { get { return MediaStream[""]; } }
        public string GMCString { get { return MediaStream[""]; } }
        public string Matrix { get { return MediaStream[""]; } }
        public string MatrixString { get { return MediaStream[""]; } }
        /// <summary>Matrix, in binary format encoded BASE64. Order = intra, non-intra, gray intra, gray non-intra</summary>
        public string MatrixData { get { return MediaStream[""]; } }
        public string CABAC { get { return MediaStream[""]; } }
        public string CABACString { get { return MediaStream[""]; } }
        public string RefFrames { get { return MediaStream[""]; } }
        public string RefFramesString { get { return MediaStream[""]; } }
        public string Pulldown { get { return MediaStream[""]; } }
        public string FrameMode { get { return MediaStream[""]; } }

        /// <summary>detailled (M=x N=y)</summary>
        public string GOP { get { return MediaStream[""]; } }
        public string FrameStructures { get { return MediaStream[""]; } }

        /// <summary>Wrapping mode (Frame wrapped or Clip wrapped)</summary>
        public string Wrapping { get { return MediaStream[""]; } }
    }
}