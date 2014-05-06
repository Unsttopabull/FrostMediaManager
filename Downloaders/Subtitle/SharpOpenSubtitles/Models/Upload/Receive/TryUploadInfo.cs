using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.Upload {
    public class TryUploadInfo : SessionInfo {

        /// <summary>
        /// Indicator if subtitles are already available on the server.<br />
        /// Is <b>1</b> if they are (upload should be interrupted, duplicate subtitle) or <b>0</b> if they're not (upload can continue)
        /// </summary>
        [XmlRpcMember("alreadyindb")]
        public int AlreadyInDb;

        /// <summary>
        /// This array will contain one subtitle file structure (if found).<br />
        /// This structure can be used to automatically gather information about movie, e.g. get IMDb ID so the user doesn't have to input it manually.
        /// </summary>
        [XmlRpcMember("data")]
        public UploadedSubtitle[] Data;
    }
}