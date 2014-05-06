using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.Upload {
    public class UploadStatusInfo : SessionInfo {

        /// <summary>Link to subtitle on OSDb server (to be used in a web browser).</summary>
        [XmlRpcMember("data")]
        public string Data;

        /// <summary>TO-DO</summary>
        [XmlRpcMember("subtitles")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public bool Subtitles;
    }
}