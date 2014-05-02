namespace Frost.InfoParsers {

    public class ParsedMovie {

        public ParsedMovie() {
            
        }

        public ParsedMovie(string originalName, string translatedName, string url) {
            OriginalName = originalName;
            TranslatedName = translatedName;
            Url = url;
        }

        public ParsedMovie(string title, int releaseYear, string imdbId) {
            OriginalName = title;
            ReleaseYear = releaseYear;
            ImdbID = imdbId;
        }

        public string OriginalName { get; set; }
        public string TranslatedName { get; set; }

        public int ReleaseYear { get; set; }

        public string ImdbID { get; set; }

        public string Url { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} ({1})", OriginalName, TranslatedName);
        }
    }

}