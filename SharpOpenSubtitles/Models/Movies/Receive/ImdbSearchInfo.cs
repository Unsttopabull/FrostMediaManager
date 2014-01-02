using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace Frost.SharpOpenSubtitles.Models.Movies.Receive {

    public class ImdbSearchInfo : SessionInfo {
        /// <summary>Array containing information about movies matching given title.</summary>
        [XmlRpcMember("data")]
        public ImdbMovie[] Data;
    }

}