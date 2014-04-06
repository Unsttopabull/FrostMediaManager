using System;
using System.Collections;
using System.Collections.Specialized;
using Frost.DetectFeatures.FileName;

namespace Frost.DetectFeatures.Util {

    public class SegmentCollection : ObservableKeyedCollection<string, SegmentMapping> {

        public SegmentCollection() : base(StringComparer.InvariantCultureIgnoreCase) {
            
        }

        public SegmentCollection(StringDictionary stringDictionary) : this() {
            foreach (DictionaryEntry entry in stringDictionary) {
                SegmentType type;
                if(Enum.TryParse((string) entry.Value, out type)){
                    Add((string) entry.Key, type);
                }
            }
        }

        /// <summary>When implemented in a derived class, extracts the key from the specified element.</summary>
        /// <returns>The key for the specified element.</returns>
        /// <param name="item">The element from which to extract the key.</param>
        protected override string GetKeyForItem(SegmentMapping item) {
            return item.Segment;
        }

        public void Add(string segment, SegmentType segmentType) {
            Add(new SegmentMapping(segment, segmentType));
        }

        public bool ContainsKey(string key) {
            return key != null && Contains(key);
        }

        public new SegmentType this[string segment] {
            get { return base[segment].SegmentType; }
            set {
                if (Dictionary == null) {
                    return;
                }

                SegmentMapping sm;
                if(Dictionary.TryGetValue(value.ToString(), out sm)) {
                    sm.SegmentType = value;
                }
            }
        }

        public bool TryGetValue(string segment, out SegmentType type) {
            if (Dictionary != null) {
                SegmentMapping sm;
                if(Dictionary.TryGetValue(segment, out sm)) {
                    type = sm.SegmentType;
                    return true;
                }
            }
            type = default(SegmentType);
            return false;
        }
    }

}
