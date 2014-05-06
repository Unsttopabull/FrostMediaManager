using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Search {
    public class SearchSubtitleInfo {

        /// <summary>Array of found subtitle file matches, when no matches are found data is empty.</summary>
        [XmlRpcMember("data")]
        public SubtitleInfo[] Data;

        /// <summary>Time taken to execute this command on server.</summary>
        [XmlRpcMember("seconds")]
        public double Seconds;
    }
}