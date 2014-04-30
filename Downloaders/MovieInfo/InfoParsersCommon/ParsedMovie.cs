﻿namespace Frost.InfoParsers {

    public class ParsedMovie {

        public ParsedMovie(string originalName, string sloveneName, string url) {
            OriginalName = originalName;
            SloveneName = sloveneName;
            Url = url;
        }

        public ParsedMovie(string title, int releaseYear, string imdbId) {
            OriginalName = title;
            ReleaseYear = releaseYear;
        }

        public string OriginalName { get; private set; }
        public string SloveneName { get; private set; }

        public int ReleaseYear { get; set; }

        public string ImdbID { get; set; }

        public string Url { get; private set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} ({1})", OriginalName, SloveneName);
        }
    }

}