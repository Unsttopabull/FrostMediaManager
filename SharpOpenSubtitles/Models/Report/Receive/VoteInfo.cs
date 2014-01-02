using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace Frost.SharpOpenSubtitles.Models.Report.Receive {

    public class VoteInfo : SessionInfo {
        /// <summary></summary>
        [XmlRpcMember("data")]
        public SubtitleRating Data;
    }

}