using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace Frost.SharpOpenSubtitles.Models.Checking.Receive {

    public class SubtitleHashInfo : SessionInfo {

        /// <summary>Contains key/value pairs where key is the subtitle file hash and value is subtitle file ID (if found).</summary>
        [XmlRpcMember("data")]
        public SubtitleHashes Data; //SubtitleHashes
    }

}