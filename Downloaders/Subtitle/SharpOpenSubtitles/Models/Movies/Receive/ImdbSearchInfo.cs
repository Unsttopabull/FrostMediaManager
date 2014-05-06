using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.Movies {

    public class ImdbSearchInfo : SessionInfo {
        /// <summary>Array containing information about movies matching given title.</summary>
        [XmlRpcMember("data")]
        public ImdbMovie[] Data;
    }

}