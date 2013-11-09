namespace Frost.MediaInfo.Output.Properties.Codecs {
    public class GeneralCodec : Codec {
        public GeneralCodec(Media mediaMenu) : base(mediaMenu) {
        }

        public string CodecExtensions { get { return MediaStream["Codec/Extensions"]; } }
        public string CodecSettings { get { return MediaStream["Codec_Settings"]; } }
        public string CodecSettingsAutomatic { get { return MediaStream["Codec_Settings_Automatic"]; } }
    }
}