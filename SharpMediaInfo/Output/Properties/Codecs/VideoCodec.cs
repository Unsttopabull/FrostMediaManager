namespace Frost.MediaInfo.Output.Properties.Codecs {
    public class VideoCodec : TextCodec{
        public VideoCodec(Media media) : base(media){
        }

        public string Family { get { return MediaStream[""]; } }
        public string Profile { get { return MediaStream[""]; } }
        public string Description { get { return MediaStream[""]; } }

        public string Settings { get { return MediaStream[""]; } }
        public string PacketBitStream { get { return MediaStream[""]; } }
        public string BVOP { get { return MediaStream[""]; } }
        public string QPel { get { return MediaStream[""]; } }
        public string GMC { get { return MediaStream[""]; } }
        public string GMCString { get { return MediaStream[""]; } }
        public string Matrix { get { return MediaStream[""]; } }
        public string MatrixData { get { return MediaStream[""]; } }
        public string CABAC { get { return MediaStream[""]; } }
        public string RefFrames { get { return MediaStream[""]; } }
    }
}