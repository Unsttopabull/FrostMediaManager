namespace SharpMediaInfo.Output.Properties.General {
    public class Season {
        private readonly Media _media;

        public Season(Media media) {
            _media = media;
        }

        public string Name { get { return _media["Season"]; } }
        public string Position { get { return _media["Season_Position"]; } }
        public string PositionTotal { get { return _media["Season_Position_Total"]; } }
    }
}