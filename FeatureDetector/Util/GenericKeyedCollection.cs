using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Frost.DetectFeatures.Util {
    public class GenericKeyedCollection<T, T2> : KeyedCollection<T, T2> {
        private readonly Func<T2, T> _getKey;

        public GenericKeyedCollection(Func<T2, T> getKey) {
            _getKey = getKey;
        }

        /// <summary>When implemented in a derived class, extracts the key from the specified element.</summary>
        /// <returns>The key for the specified element.</returns>
        /// <param name="item">The element from which to extract the key.</param>
        protected override T GetKeyForItem(T2 item) {
            return _getKey(item);
        }

        public void AddRange(IEnumerable<T2> range) {
            foreach (T2 item in range) {
                Add(item);
            }
        }
    }
}
