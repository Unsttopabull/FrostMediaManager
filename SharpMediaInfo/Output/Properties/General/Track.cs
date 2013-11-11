namespace Frost.SharpMediaInfo.Output.Properties.General {
    public class Track {
        private readonly Media _media;

        public Track(Media media) {
            _media = media;
        }

        public string Name { get { return _media["Track"]; } }
        public string More { get { return _media["Track/More"]; } }
        public string Url { get { return _media["Track/Url"]; } }
        public string Sort { get { return _media["Track/Sort"]; } }
        public string Position { get { return _media["Track/Position"]; } }
        public string PositionTotal { get { return _media["Track/Position_Total"]; } }
    }
}