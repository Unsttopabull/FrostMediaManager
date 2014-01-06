using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class FilterInfo : StatusInfo {

        /// <summary>Indicates if filters are enabled or not.</summary>
        [XmlRpcMember("search_restrict")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public bool SearchRestrict;

        /// <summary>A list of languages used</summary>
        [XmlRpcMember("search_langs")]
        [XmlRpcEnumMapping(EnumMapping.Number)]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public LanguageId[] SearchLangs;
    }
}
