
namespace Frost.SharpMediaInfo.Output.Properties {

    public class ChannelInfo {

        private readonly Media _media;

        internal ChannelInfo(Media media) {
            _media = media;
        }

        /// <summary>Number of channels (with measurement)</summary>
        public string String { get { return _media["Channel(s)/String"]; } }

        /// <summary>Number of channels</summary>
        public long? Original { get { return _media.TryParseLong("Channel(s)_Original"); } }
        /// <summary>Number of channels (with measurement)</summary>
        public string OriginalString { get { return _media["Channel(s)_Original/String"]; } }

        /// <summary>Position of channels</summary>
        public string Positions { get { return _media["ChannelPositions"]; } }
        /// <summary>Position of channels (x/y.z format)</summary>
        public string PositionsString2 { get { return _media["ChannelPositions/String2"]; } }

        /// <summary>Layout of channels (in the stream)</summary>
        public string Layout { get { return _media["ChannelLayout"]; } }
    }
}