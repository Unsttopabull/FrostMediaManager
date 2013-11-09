namespace Frost.MediaInfo.Output.Properties {
    public class ChannelInfo {
        private readonly Media _media;

        public ChannelInfo(Media media) {
            _media = media;
        }

        /// <summary>Number of channels (with measurement)</summary>
        public string String { get { return _media[""]; } }

        /// <summary>Number of channels</summary>
        public string Original { get { return _media[""]; } }

        /// <summary>Number of channels (with measurement)</summary>
        public string OriginalString { get { return _media[""]; } }

        /// <summary>Position of channels</summary>
        public string Positions { get { return _media[""]; } }

        /// <summary>Position of channels (x/y.z format)</summary>
        public string PositionsString2 { get { return _media[""]; } }

        /// <summary>Layout of channels (in the stream)</summary>
        public string Layout { get { return _media[""]; } }
    }
}