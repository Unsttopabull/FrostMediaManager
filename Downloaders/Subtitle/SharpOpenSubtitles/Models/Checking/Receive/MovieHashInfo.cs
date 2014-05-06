using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.Checking {

    public class MovieHashInfo : SessionInfo {

        /// <summary>List of movie info structures.</summary>
        [XmlRpcMember("data")]
        public MovieResults Data; //MovieResult
    }

}
