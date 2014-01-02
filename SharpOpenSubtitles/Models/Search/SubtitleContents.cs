using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Search {
    public class SubtitleContents {

        /// <summary>Subtitle ID</summary>
        [XmlRpcMember("idsubtitlefile")]
        public string SubtitleFileID;

        /// <summary>Base64 and GZIPed file contents</summary>
        [XmlRpcMember("data")]
        public string Data; //Base64 GZIP
    }
}