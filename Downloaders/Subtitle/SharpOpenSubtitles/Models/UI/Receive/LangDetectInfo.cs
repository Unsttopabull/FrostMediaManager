using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.UI {

    public class LangDetectInfo : SessionInfo {

        /// <summary>Contains a structure holding in key the MD5 of the unpacked input string and in value its detected 3-letter language code.</summary>
        [XmlRpcMember("data")]
        public XmlRpcStruct Data;
    }

}