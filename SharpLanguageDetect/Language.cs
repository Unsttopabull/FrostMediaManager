
namespace Frost.SharpLanguageDetect {

    /**
     * {@link Language} is to store the detected language.
     * {@link Detector#getProbabilities()} returns an {@link ArrayList} of {@link Language}s.
     *  
     * @see Detector#getProbabilities()
     * @author Nakatani Shuyo
     *
     */
    public class Language {

        public string LangCode { get; private set; }
        public double Probability { get; private set; }

        public Language(string langCode, double probability) {
            LangCode = langCode;
            Probability = probability;
        }

        public override string ToString() {
            return LangCode + ":" + Probability;
        }

    }

}