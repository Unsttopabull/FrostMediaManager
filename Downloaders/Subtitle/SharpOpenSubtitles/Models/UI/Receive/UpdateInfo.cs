using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.UI {
    public class UpdateInfo : SessionInfo {

        /// <summary>Latest application version.</summary>
        [XmlRpcMember("version")]
        public string Version;

        /// <summary>Download URL for Windows version.</summary>
        [XmlRpcMember("url_windows")]
        public string UrlWindows;

        /// <summary>Download URL for Linux version.</summary>
        [XmlRpcMember("url_linux")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string UrlLinux;

        /// <summary>Application changelog and other comments.</summary>
        [XmlRpcMember("comments")]
        public string Comments;
    }
}
