namespace Frost.MediaInfo.Output {
    public class PartInfo {
        private readonly Media _media;

        public PartInfo(Media media) {
            _media = media;
        }

        public string Part { get { return _media["Part"]; } }
        public string PartPosition { get { return _media["Part/Position"]; } }
        public string PartPositionTotal { get { return _media["Part/Position_Total"]; } }
    }
}