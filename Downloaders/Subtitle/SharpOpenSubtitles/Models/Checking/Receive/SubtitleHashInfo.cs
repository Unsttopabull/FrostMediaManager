using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.Checking {

    public class SubtitleHashInfo : SessionInfo {

        /// <summary>Contains key/value pairs where key is the subtitle file hash and value is subtitle file ID (if found).</summary>
        [XmlRpcMember("data")]
        public SubtitleHashes Data;
    }

}