using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles.Models.UI {

    public class AvailableTranslationsInfo : SessionInfo {

        /// <summary>Structure of subtitle languages.</summary>
        [XmlRpcMember("data")]
        public TranslationInfos Data; //TranslationInfos
    }

}