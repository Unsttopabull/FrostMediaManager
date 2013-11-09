namespace Frost.MediaInfo.Output.Properties {
    public class SizeInfo {
        private readonly Media _media;

        public SizeInfo(Media media) {
            _media = media;
        }

        /// <summary>Size (aperture size if present) with measurement (pixel)</summary>
        public string String { get { return _media[""]; } }

        /// <summary>Offset between original height and displayed size (aperture size) in pixel</summary>
        public string Offset { get { return _media[""]; } }
        /// <summary>Offset between original height and displayed size (aperture size) in pixel</summary>
        public string OffsetString { get { return _media[""]; } }

        /// <summary>Original (in the raw stream) size in pixel</summary>
        public string Original { get { return _media[""]; } }
        /// <summary>Original (in the raw stream) size with measurement (pixel)</summary>
        public string OriginalString { get { return _media[""]; } }
    }
}