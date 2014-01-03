using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class SearchResult : StatusInfo {

        /// <summary>A dictionary of hashes (as defined in the search call) with a result, if there are no results, this dictionary is empty.</summary>
        [XmlRpcMember("results")]
        public MovieResults Results;
    }
}
