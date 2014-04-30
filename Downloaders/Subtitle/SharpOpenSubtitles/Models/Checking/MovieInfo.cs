using System.Globalization;
using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Checking {

    public class MovieInfo {
        public MovieInfo(string movieHash, XmlRpcStruct info) {
            MovieHash = movieHash;

            if (info == null) {
                return;
            }

            if (info.ContainsKey("MovieImdbID")) {
                ImdbId = (string) info["MovieImdbID"];
            }

            if (info.ContainsKey("MovieName")) {
                Title = (string) info["MovieName"];
            }

            if (info.ContainsKey("MovieYear")) {
                int realeaseYear;
                if (int.TryParse((string) info["MovieYear"], NumberStyles.Integer, CultureInfo.InvariantCulture, out realeaseYear)) {
                    ReleaseYear = realeaseYear;
                }
            }

            if (info.ContainsKey("MovieKind")) {
                Kind = (string) info["MovieKind"];
            }

            if (info.ContainsKey("SeriesSeason")) {
                Season = (string) info["SeriesSeason"];
            }

            if (info.ContainsKey("SeriesEpisode")) {
                Episode = (string) info["SeriesEpisode"];
            }
        }

        /// <summary>Movie IMDb ID.</summary>
        public string ImdbId { get; set; }

        /// <summary>Movie title.</summary>
        public string Title { get; set; }

        /// <summary>Movie release year.</summary>
        public int ReleaseYear { get; set; }

        public string Kind { get; set; }

        public string Season { get; set; }

        public string Episode { get; set; }

        /// <summary>Video file hash, you can use this value to match the movie info to your input parameters.</summary>
        public string MovieHash { get; set; }
    }

}