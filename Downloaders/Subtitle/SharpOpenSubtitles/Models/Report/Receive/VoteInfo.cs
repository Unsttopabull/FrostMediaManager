using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.Report {

    public class VoteInfo : SessionInfo {
        /// <summary></summary>
        [XmlRpcMember("data")]
        public SubtitleRating Data;
    }

}