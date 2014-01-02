using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Movies.Send {

    public class InsertMovieInfo {

        /// <summary>The movie title.</summary>
        [XmlRpcMember("moviename")]
        public string MovieName;

        /// <summary>The movie release year.</summary>
        [XmlRpcMember("movieyear")]
        public string MovieYear;
    }
}