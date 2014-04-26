using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class DownloadInfo : StatusInfo {

        [XmlRpcMember("names")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public SubtitleDownload[] Names;
    }
}
