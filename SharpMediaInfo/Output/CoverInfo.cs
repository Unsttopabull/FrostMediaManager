namespace Frost.SharpMediaInfo.Output {
    public class CoverInfo {
        private MediaGeneral _media;

        public CoverInfo(MediaGeneral mediaGeneral) {
            _media = mediaGeneral;
        }

        public string CoverDescription { get { return _media["Cover_Description"]; } }
        public string CoverType { get { return _media["Cover_Type"]; } }
        public string CoverMime { get { return _media["Cover_Mime"]; } }
        public string CoverData { get { return _media["Cover_Data"]; } }
    }
}