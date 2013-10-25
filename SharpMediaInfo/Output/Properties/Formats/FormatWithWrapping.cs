namespace SharpMediaInfo.Output.Properties.Formats {
    public class FormatWithWrapping : Format{
        internal FormatWithWrapping(Media media) : base(media) {
        }

        public string WrappingSettings { get { return MediaStream[""]; } }
    }
}