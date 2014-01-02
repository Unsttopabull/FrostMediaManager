using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {

    public enum MediaType {
        Movie,
        TvSeries,
        TvMiniseries
    }

    public class MovieMatch {

        [XmlRpcMember("movieId")]
        public string MovieId;

        [XmlRpcMember("movieTitle")]
        public string MovieTitle;

        [XmlRpcMember("movieYear")]
        public string MovieYear;

        [XmlRpcMember("movieType")]
        [XmlRpcEnumMapping(EnumMapping.Number)]
        public MediaType MovieType;

        [XmlRpcMember("tvSeason")]
        public int TvSeason;

        [XmlRpcMember("tvEpisode")]
        public int TvEpisode;

        [XmlRpcMember("subtitles")]
        public SubtitleResult[] Subtitles;
    }
}
