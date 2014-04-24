using CookComputing.XmlRpc;

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

        public MovieInfo(string movieHash, XmlRpcStruct info) {
            MovieHash = movieHash;

            if (info == null) {
                return;
            }

            if (info.ContainsKey("MovieImdbID")) {
                MovieImdbID = (string) info["MovieImdbID"];
            }

            if (info.ContainsKey("MovieName")) {
                MovieImdbID = (string) info["MovieName"];
            }

            if (info.ContainsKey("MovieYear")) {
                MovieImdbID = (string) info["MovieYear"];
            }

            if (info.ContainsKey("MovieKind")) {
                MovieImdbID = (string) info["MovieKind"];
            }

            if (info.ContainsKey("SeriesSeason")) {
                MovieImdbID = (string) info["SeriesSeason"];
            }

            if (info.ContainsKey("SeriesEpisode")) {
                MovieImdbID = (string) info["SeriesEpisode"];
            }
        }
    }

}