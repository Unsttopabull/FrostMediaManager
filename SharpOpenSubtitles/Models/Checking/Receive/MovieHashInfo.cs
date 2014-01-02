using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace Frost.SharpOpenSubtitles.Models.Checking.Receive {

    public class MovieHashInfo : SessionInfo {

        /// <summary>List of movie info structures.</summary>
        [XmlRpcMember("data")]
        public XmlRpcStruct Data; //MovieResult
    }

}
