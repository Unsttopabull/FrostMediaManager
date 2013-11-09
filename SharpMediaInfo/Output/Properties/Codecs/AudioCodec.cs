namespace Frost.MediaInfo.Output.Properties.Codecs {
    public class AudioCodec : TextCodec {
        public AudioCodec(Media media) : base(media){
        }

        public string Family { get { return MediaStream[""]; } }
        public string Description { get { return MediaStream[""]; } }
        public string Profile { get { return MediaStream[""]; } }

        public string Settings { get { return MediaStream[""]; } }
        public string Settings_Automatic { get { return MediaStream[""]; } }
        public string Settings_Floor { get { return MediaStream[""]; } }
        public string Settings_Firm { get { return MediaStream[""]; } }
        public string Settings_Endianness { get { return MediaStream[""]; } }
        public string Settings_Sign { get { return MediaStream[""]; } }
        public string Settings_Law { get { return MediaStream[""]; } }
        public string Settings_ITU { get { return MediaStream[""]; } }
    }
}