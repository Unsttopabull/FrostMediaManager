namespace Frost.SharpOpenSubtitles.Models.Checking {

    public class MovieInfo {
        /// <summary>Video file hash, you can use this value to match the movie info to your input parameters.</summary>
        public string MovieHash;

        /// <summary>Movie IMDb ID.</summary>
        public string MovieImdbID;

        /// <summary>Movie title.</summary>
        public string MovieName;

        /// <summary>Movie release year.</summary>
        public string MovieYear;
    }
}
