namespace SharpMediaInfo.Output.Properties {
    public enum StreamSizeType {
        StreamSize,
        SourceStreamSize,
        EncodedStreamSize,
        EncodedSourceStreamSize
    }

    public class StreamSizeInfo {
        private readonly Media _media;
        private readonly string[] _propNames;

        internal StreamSizeInfo(Media mediaAudio, StreamSizeType type = StreamSizeType.StreamSize) {
            _media = mediaAudio;

            switch (type) {
                case StreamSizeType.StreamSize:
                case StreamSizeType.SourceStreamSize:
                case StreamSizeType.EncodedStreamSize:
                case StreamSizeType.EncodedSourceStreamSize:
                    _propNames = new[] { "", "", "", "", "", "", "" };
                    break;
            }
        }

        /// <summary>Streamsize in with percentage value</summary>
        public string String { get { return _media[_propNames[0]]; } }
        public string String1 { get { return _media[_propNames[1]]; } }
        public string String2 { get { return _media[_propNames[2]]; } }
        public string String3 { get { return _media[_propNames[3]]; } }
        public string String4 { get { return _media[_propNames[4]]; } }

        /// <summary>Streamsize in with percentage value</summary>
        public string String5 { get { return _media[_propNames[5]]; } }

        /// <summary>Stream size divided by file size</summary>
        public string Proportion { get { return _media[_propNames[6]]; } }
    }
}