namespace Frost.SharpMediaInfo.Output.Properties {
    public class Interleave {
        private readonly Media _media;

        internal Interleave(Media media) {
            _media = media;
        }

        /// <summary>Between how many video frames the stream is inserted</summary>
        public string VideoFrames { get { return _media["Interleave_VideoFrames"]; } }

        /// <summary>Between how much time (ms) the stream is inserted</summary>
        public string Duration { get { return _media["Interleave_Duration"]; } }

        /// <summary>Between how much time and video frames the stream is inserted (with measurement)</summary>
        public string DurationString { get { return _media["Interleave_Duration/String"]; } }

        /// <summary>How much time is buffered before the first video frame</summary>
        public string Preload { get { return _media["Interleave_Preload"]; } }

        /// <summary>How much time is buffered before the first video frame (with measurement)</summary>
        public string PreloadString { get { return _media["Interleave_Preload/String"]; } }
    }
}