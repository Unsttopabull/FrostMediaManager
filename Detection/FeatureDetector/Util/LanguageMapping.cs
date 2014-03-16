namespace Frost.DetectFeatures.Util {
    public class LanguageMapping {

        public LanguageMapping() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public LanguageMapping(string mapping, string iso639Alpha3) {
            Mapping = mapping;
            ISO639Alpha3 = iso639Alpha3;
        }

        public string Mapping { get; set; }

        public string ISO639Alpha3 { get; set; }
    }
}
