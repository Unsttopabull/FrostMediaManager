using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class SearchResult : StatusInfo {

        [XmlRpcMember("results")]
        public MovieResults Results;
    }
}
