using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Report {
    public class LastUpdate {
        public LastUpdate(string isoLanguageCode, XmlRpcStruct update) {
        }

        public string ISOLanguageName { get; set; }
        public string LatestUpdate { get; set; }
    }
}
