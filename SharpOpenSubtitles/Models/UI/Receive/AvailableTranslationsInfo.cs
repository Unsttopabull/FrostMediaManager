using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace Frost.SharpOpenSubtitles.Models.UI.Receive {

    public class AvailableTranslationsInfo : SessionInfo {

        /// <summary>Structure of subtitle languages.</summary>
        [XmlRpcMember("data")]
        public TranslationInfos Data; //TranslationInfos
    }

}