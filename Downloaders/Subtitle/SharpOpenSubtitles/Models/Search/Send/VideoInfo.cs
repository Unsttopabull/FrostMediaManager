using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Search {
    public class VideoInfo {

        /// <summary>The movie MovieHash value.</summary>
        [XmlRpcMember("moviehash")]
        public string MovieHash;

        /// <summary>The size of the file in bytes.</summary>
        [XmlRpcMember("moviesize")]
        public double MovieSize;
    }
}