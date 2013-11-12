
namespace Frost.SharpMediaInfo.Output.Properties.Codecs {

    public class VideoCodec : TextCodec{

        public VideoCodec(Media media) : base(media){
        }

        public string Family { get { return MediaStream["Codec/Family"]; } }
        public string Profile { get { return MediaStream["Codec_Profile"]; } }
        public string Description { get { return MediaStream["Codec_Description"]; } }

        public string Settings { get { return MediaStream["Codec_Settings"]; } }
        public string PacketBitStream { get { return MediaStream["Codec_Settings_PacketBitStream"]; } }
        public string BVOP { get { return MediaStream["Codec_Settings_BVOP"]; } }
        public string QPel { get { return MediaStream["Codec_Settings_QPel"]; } }

        public long? GMC { get { return MediaStream.TryParseLong("Codec_Settings_GMC"); } }
        public string GMCString { get { return MediaStream["Codec_Settings_GMC/String"]; } }

        public string Matrix { get { return MediaStream["Codec_Settings_Matrix"]; } }
        public string MatrixData { get { return MediaStream["Codec_Settings_Matrix_Data"]; } }

        public string CABAC { get { return MediaStream["Codec_Settings_CABAC"]; } }
        public string RefFrames { get { return MediaStream["Codec_Settings_RefFrames"]; } }
    }
}