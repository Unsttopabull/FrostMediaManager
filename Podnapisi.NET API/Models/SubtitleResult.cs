using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class SubtitleResult {

        [XmlRpcMember("id")]
        public string ID;

        [XmlRpcMember("lang")]
        public string Lang;

        [XmlRpcMember("uploader")]
        public string Uploader;

        [XmlRpcMember("uploaderId")]
        public string UploaderId;

        /// <summary>
        /// Represents adequacy for this subtitle:
        /// <list type="bullet">
        ///     <item><description>&lt; 0 - probably inadequate.</description></item>
        ///     <item><description>= 0 - unknown.</description></item>
        ///     <item><description>&gt; 0 - probably adequate.</description></item>
        /// </list>
        /// </summary>
        [XmlRpcMember("weight")]
        public double Weight;

        [XmlRpcMember("release")]
        public string Release;

        [XmlRpcMember("flags")]
        public string Flags;

        [XmlRpcMember("rating")]
        public string Rating;

        [XmlRpcMember("inexact")]
        public bool Inexact;
    }
}
