using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace Frost.SharpOpenSubtitles.Models.Movies.Receive {
    public class ImdbMovieDetailsInfo : SessionInfo {

        /// <summary>A Structure containing movie information</summary>
        [XmlRpcMember("data")]
        public ImdbMovieDetails Data;
    }
}