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

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} => {1}", Mapping, ISO639Alpha3);
        }
    }
}
