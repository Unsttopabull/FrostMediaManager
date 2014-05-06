using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.UI {

    public class Translation : SessionInfo {

        /// <summary>Base64-encoded language file contents.</summary>
        [XmlRpcMember("data")]
        public string Data;
    }

}