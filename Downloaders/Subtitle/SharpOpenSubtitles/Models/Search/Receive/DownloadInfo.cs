using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace Frost.SharpOpenSubtitles.Models.Search.Receive {
    public class DownloadInfo : SessionInfo {

        /// <summary>Array of subtitle file contents.</summary>
        [XmlRpcMember("data")]
        public SubtitleContents[] Data;
    }
}