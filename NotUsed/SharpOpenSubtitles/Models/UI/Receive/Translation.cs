using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace Frost.SharpOpenSubtitles.Models.UI.Receive {

    public class Translation : SessionInfo {

        /// <summary>Base64-encoded language file contents.</summary>
        [XmlRpcMember("data")]
        public string Data;
    }

}