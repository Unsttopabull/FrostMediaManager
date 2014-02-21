using System;
using System.Collections.ObjectModel;

namespace Frost.Common.Util {

    public class KeyedCollectionGeneric<TKey, TItem> : KeyedCollection<TKey, TItem> {
        private readonly Func<TItem, TKey> _keySelector;

        public KeyedCollectionGeneric(Func<TItem, TKey> keySelector) {
            _keySelector = keySelector;
        }

        /// <summary>When implemented in a derived class, extracts the key from the specified element.</summary>
        /// <returns>The key for the specified element.</returns>
        /// <param name="item">The element from which to extract the key.</param>
        protected override TKey GetKeyForItem(TItem item) {
            return _keySelector(item);
        }

        public bool ContainsKey(TKey key) {
            return Contains(key);
        }
    }

}