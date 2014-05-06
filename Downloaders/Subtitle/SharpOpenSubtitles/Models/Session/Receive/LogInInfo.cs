using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Session {
    public class LogInInfo : SessionInfo {

        /// <summary>Token string/session id, must be used in subsequent communication.</summary>
        [XmlRpcMember("token")]
        public string Token;
    }
}