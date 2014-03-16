using System;
using System.Collections;
using System.Collections.Specialized;

namespace Frost.DetectFeatures.Util {

    public class LanguageMappingCollection : ObservableKeyedCollection<string, LanguageMapping> {
        public LanguageMappingCollection() : base(StringComparer.InvariantCultureIgnoreCase) {
        }

        public LanguageMappingCollection(StringDictionary stringDictionary) : this() {
            foreach (DictionaryEntry entry in stringDictionary) {
                Add((string) entry.Key, (string) entry.Value);
            }
        }

        /// <summary>When implemented in a derived class, extracts the key from the specified element.</summary>
        /// <returns>The key for the specified element.</returns>
        /// <param name="item">The element from which to extract the key.</param>
        protected override string GetKeyForItem(LanguageMapping item) {
            return item.Mapping;
        }

        public void Add(string mapping, string iso693Alpha3) {
            Add(new LanguageMapping(mapping, iso693Alpha3));
        }

        public bool ContainsKey(string mapping) {
            return mapping != null && Contains(mapping);
        }

        public new string this[string mapping] {
            get { return base[mapping].ISO639Alpha3; }
        }
    }

}