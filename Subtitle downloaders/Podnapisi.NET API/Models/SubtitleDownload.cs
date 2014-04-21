using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class SubtitleDownload {

        [XmlRpcMember("id")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int ID;

        [XmlRpcMember("filename")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Filename;
    }
}
