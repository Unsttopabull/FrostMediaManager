namespace SharpMediaInfo.Output.Properties.Codecs {
    public class TextCodec : Codec {
        public TextCodec(Media mediaText) : base(mediaText){
        }

        public string CC { get { return MediaStream[""]; } }
    }
}