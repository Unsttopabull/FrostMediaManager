using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class LogInInfo : StatusInfo {

        [XmlRpcMember("session")]
        public string Session;

        [XmlRpcMember("nonce")]
        public string Nonce;
    }
}
