using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.UI {

    public class TranslationLanguage {
        /// <summary>Initializes a new instance of the <see cref="TranslationLanguage"/> class.</summary>
        public TranslationLanguage(string lastCreated, string stringsNo) {
            LastCreated = lastCreated;
            StringsNo = stringsNo;
        }

        public TranslationLanguage(string isoLanguageCode, XmlRpcStruct lang) {
            ISOLanguageCode = isoLanguageCode;

            if (lang.ContainsKey("LastCreated")) {
                LastCreated = (string) lang["LastCreated"];
            }
            if (lang.ContainsKey("StringsNo")) {
                StringsNo = (string) lang["StringsNo"];
            }
        }

        public string ISOLanguageCode { get; set; }

        /// <summary>Last date/time a translation was created/modified in this language.</summary>
        public string LastCreated { get; set; }

        /// <summary>Number of translated strings currently available in this translation.</summary>
        public string StringsNo { get; set; }
    }

}