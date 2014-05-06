using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.Search {
    public class DownloadInfo : SessionInfo {

        /// <summary>Array of subtitle file contents.</summary>
        [XmlRpcMember("data")]
        public SubtitleContents[] Data;
    }
}