using System;
using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {

    public enum MediaType {
        Movie,
        TvSeries,
        TvMiniseries
    }

    public class MovieMatch {

        /// <summary>Initializes a new instance of the <see cref="MovieMatch"/> class.</summary>
        public MovieMatch(string movieHash, int movieId, string movieTitle, int movieYear, MediaType movieType, int tvSeason, int tvEpisode, SubtitleResult[] subtitles) {
            MovieHash = movieHash;
            MovieId = movieId;
            MovieTitle = movieTitle;
            MovieYear = movieYear;
            MovieType = movieType;
            TvSeason = tvSeason;
            TvEpisode = tvEpisode;
            Subtitles = subtitles;
        }

        /// <summary>Initializes a new instance of the <see cref="MovieMatch"/> class.</summary>
        public MovieMatch(string movieHash) {
            MovieHash = movieHash;
        }        

        [NonSerialized]
        public string MovieHash;

        /// <summary>An OMDb unique identifier.</summary>
        [XmlRpcMember("movieId")]
        public int MovieId;

        [XmlRpcMember("movieTitle")]
        public string MovieTitle;

        [XmlRpcMember("movieYear")]
        public int MovieYear;

        [XmlRpcMember("movieType")]
        [XmlRpcEnumMapping(EnumMapping.Number)]
        public MediaType MovieType;

        /// <remarks>0 if <see cref="MovieType"/> is not TV Series.</remarks>
        [XmlRpcMember("tvSeason")]
        public int TvSeason;

        [XmlRpcMember("tvEpisode")]
        public int TvEpisode;

        /// <summary>Is a list of found subtitles up to 100 entries, this list may be empty.</summary>
        [XmlRpcMember("subtitles")]
        public SubtitleResult[] Subtitles;
    }
}
