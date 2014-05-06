using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.Movies {
    public class ImdbMovieDetailsInfo : SessionInfo {

        /// <summary>A Structure containing movie information</summary>
        [XmlRpcMember("data")]
        public ImdbMovieDetails Data;
    }
}