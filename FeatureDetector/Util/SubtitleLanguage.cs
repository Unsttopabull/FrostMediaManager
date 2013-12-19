using System.Text;
using Frost.Common.Util.ISO;

namespace Frost.DetectFeatures.Util {

    internal class SubtitleLanguage {
        internal SubtitleLanguage(Encoding enc, string isoLangCode) {
            Encoding = enc;

            if (isoLangCode != null) {
                Language = ISOLanguageCodes.Instance.GetByISOCode(isoLangCode);
            }
        }

        public Encoding Encoding { get; private set; }

        public ISOLanguageCode Language { get; set; }
    }

}