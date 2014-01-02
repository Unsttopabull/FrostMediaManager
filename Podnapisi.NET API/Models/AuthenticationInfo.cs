using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class AuthenticationInfo : StatusInfo {

        [XmlRpcMember("search_restrict")]
        public string SearchRestrict;

        [XmlRpcMember("search_langs")]
        public string SearchLangs;
    }
}
