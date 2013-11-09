namespace Frost.MediaInfo.Output.Properties {
    public class Interleave {
        private readonly Media _media;

        public Interleave(Media media) {
            _media = media;
        }

        /// <summary>Between how many video frames the stream is inserted</summary>
        public string VideoFrames { get { return _media[""]; } }

        /// <summary>Between how much time (ms) the stream is inserted</summary>
        public string Duration { get { return _media[""]; } }

        /// <summary>Between how much time and video frames the stream is inserted (with measurement)</summary>
        public string DurationString { get { return _media[""]; } }

        /// <summary>How much time is buffered before the first video frame</summary>
        public string Preload { get { return _media[""]; } }

        /// <summary>How much time is buffered before the first video frame (with measurement)</summary>
        public string PreloadString { get { return _media[""]; } }
    }
}