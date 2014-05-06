using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Session {
    public class SessionInfo {

        /// <summary>Function result code, see <a href="http://trac.opensubtitles.org/projects/opensubtitles/wiki/XmlRpcStatusCode">list of status codes</a>.</summary>
        [XmlRpcMember("status")]
        public string Status;

        /// <summary>Time taken to execute this command on server.</summary>
        [XmlRpcMember("seconds")]
        public double Seconds;
    }
}