using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Report {

    public class DownloadLimits {

        /// <summary></summary>
        [XmlRpcMember("global_24h_download_limit")]
        public int Global24HDownloadLimit;

        /// <summary></summary>
        [XmlRpcMember("client_ip")]
        public string ClientIP;

        /// <summary></summary>
        [XmlRpcMember("limit_check_by")]
        public string LimitCheckBy;

        /// <summary></summary>
        [XmlRpcMember("client_24h_download_limit")]
        public int Client24HDownloadLimit;
    }

}