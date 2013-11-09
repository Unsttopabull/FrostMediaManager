namespace Frost.MediaInfo.Output.Properties.Formats {
    public class Format{
        protected readonly Media MediaStream;

        internal Format(Media media) {
            MediaStream = media;
        }

        public string Summary { get { return MediaStream[""]; } }
        public string Info { get { return MediaStream[""]; } }
        public string Url { get { return MediaStream[""]; } }
        public string Commercial { get { return MediaStream[""]; } }
        public string CommercialIfAny { get { return MediaStream[""]; } }
        public string Version { get { return MediaStream[""]; } }
        public string Profile { get { return MediaStream[""]; } }
        public string Compression { get { return MediaStream[""]; } }

        public string Settings { get { return MediaStream[""]; } }
    }
}