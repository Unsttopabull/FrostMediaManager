using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Frost.DetectFeatures.Util {

    public class CodecIdMappingCollection : KeyedCollection<string, CodecIdBinding>, IEnumerable<KeyValuePair<string, string>> {

        public CodecIdMappingCollection() : base(StringComparer.InvariantCultureIgnoreCase){
        }

        public void Add(string codecId, string mapping) {
            Add(new CodecIdBinding(codecId, mapping));
        }

        /// <summary>When implemented in a derived class, extracts the key from the specified element.</summary>
        /// <returns>The key for the specified element.</returns>
        /// <param name="item">The element from which to extract the key.</param>
        protected override string GetKeyForItem(CodecIdBinding item) {
            return item.CodecId;
        }

        public bool ContainsKey(string key) {
            return key != null && Contains(key);
        }

        public new string this[string codecId] {
            get { return base[codecId].Mapping; }
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public new IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            IEnumerator<CodecIdBinding> baseEnumerator = base.GetEnumerator();

            while (baseEnumerator.MoveNext()) {
                yield return new KeyValuePair<string, string>(baseEnumerator.Current.CodecId, baseEnumerator.Current.Mapping);
            }
        }
    }

}