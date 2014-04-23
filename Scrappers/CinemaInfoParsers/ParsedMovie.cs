namespace Frost.InfoParsers {

    public class ParsedMovie {

        public ParsedMovie(string originalName, string sloveneName, string url) {
            this.OriginalName = originalName;
            this.SloveneName = sloveneName;
            this.Url = url;
        }

        public string OriginalName { get; private set; }
        public string SloveneName { get; private set; }
        public string Url { get; private set; }
    }

}