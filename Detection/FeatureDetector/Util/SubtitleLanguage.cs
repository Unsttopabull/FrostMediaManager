using System.Text;
using Frost.Common.Util.ISO;

namespace Frost.DetectFeatures.Util {

    internal class SubtitleLanguage {
        internal SubtitleLanguage(Encoding enc, string isoLangCode, string md5 = null) {
            Encoding = enc;
            MD5 = md5;

            if (isoLangCode != null) {
                Language = ISOLanguageCodes.Instance.GetByISOCode(isoLangCode);
            }
        }

        public Encoding Encoding { get; private set; }

        public ISOLanguageCode Language { get; private set; }

        public string MD5 { get; private set; }
    }

}