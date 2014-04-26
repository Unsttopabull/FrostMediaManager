using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class SubtitleResult {

        [XmlRpcMember("id")]
        public int ID;

        [XmlRpcMember("lang")]
        public string LanguageCode;

        [XmlRpcMember("uploader")]
        public string Uploader;

        [XmlRpcMember("uploaderId")]
        public int UploaderId;

        /// <summary>
        /// Represents adequacy for this subtitle:
        /// <list type="bullet">
        ///     <item><description>&lt; 0 - probably inadequate.</description></item>
        ///     <item><description>= 0 - unknown.</description></item>
        ///     <item><description>&gt; 0 - probably adequate.</description></item>
        /// </list>
        /// </summary>
        [XmlRpcMember("weight")]
        public int MatchRanking;

        /// <summary>Are releases delimited by a space (data may be false).</summary>
        [XmlRpcMember("release")]
        public string Release;

        /// <summary>Are flags that define additional properties of a subtitle (see <a href="https://www.podnapisi.net/wiki/wiki/Docs/SSP#flags">flags</a>).</summary>
        [XmlRpcMember("flags")]
        public string Flags;

        [XmlRpcMember("rating")]
        public int Rating;

        /// <summary>Value which specifies if a subtitle is matched directly to the hash.</summary>
        [XmlRpcMember("inexact")]
        public bool Inexact;

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return LanguageCode + " => " + Release;
        }
    }
}
