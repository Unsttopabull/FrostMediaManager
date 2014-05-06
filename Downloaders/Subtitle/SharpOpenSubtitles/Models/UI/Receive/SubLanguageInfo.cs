using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.UI {

    public class SubLanguageInfo {

        /// <summary>Array of enabled subtitle languages.</summary>
        [XmlRpcMember("data")]
        public SubLanguage[] Data;

        /// <summary>Time taken to execute this command on server.</summary>
        [XmlRpcMember("seconds")]
        public double Seconds;
    }

}