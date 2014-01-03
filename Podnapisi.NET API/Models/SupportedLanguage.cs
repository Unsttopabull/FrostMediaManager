namespace Frost.PodnapisiNET.Models {

    public class SupportedLanguage {

        /// <summary>Is a language identifier used by Podnapisi.NET, use them to define filters.</summary>
        public int LanguageId;

        /// <summary>A ISO 639-1 language code, note that there are exceptions (eg. Serbian-cyrillic (cyr) and Brazillian (pb)).</summary>
        public string LanguageCode;

        /// <summary>Initializes a new instance of the <see cref="SupportedLanguage"/> class.</summary>
        public SupportedLanguage(int languageId, string languageCode) {
            LanguageId = languageId;
            LanguageCode = languageCode;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return LanguageCode + " => " + LanguageId;
        }
    }

}