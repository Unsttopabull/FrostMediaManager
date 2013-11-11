namespace Frost.SharpMediaInfo.Output.Properties.Codecs {
    public class AudioCodec : TextCodec {
        public AudioCodec(Media media) : base(media){
        }

        public string Family { get { return MediaStream["Codec/Family"]; } }
        public string Description { get { return MediaStream["Codec_Description"]; } }
        public string Profile { get { return MediaStream["Codec_Profile"]; } }

        public string Settings { get { return MediaStream["Codec_Settings"]; } }
        public string Settings_Automatic { get { return MediaStream["Codec_Settings_Automatic"]; } }
        public string Settings_Floor { get { return MediaStream["Codec_Settings_Floor"]; } }
        public string Settings_Firm { get { return MediaStream["Codec_Settings_Firm"]; } }
        public string Settings_Endianness { get { return MediaStream["Codec_Settings_Endianness"]; } }
        public string Settings_Sign { get { return MediaStream["Codec_Settings_Sign"]; } }
        public string Settings_Law { get { return MediaStream["Codec_Settings_Law"]; } }
        public string Settings_ITU { get { return MediaStream["Codec_Settings_ITU"]; } }
    }
}