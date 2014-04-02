using System;
using System.Collections.ObjectModel;

namespace Frost.Common.Util {

    /// <summary>Generic KeyedCollection, so we don't need to subclass every time.</summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class KeyedCollectionGeneric<TKey, TItem> : KeyedCollection<TKey, TItem> {
        private readonly Func<TItem, TKey> _keySelector;

        /// <summary>Initializes a new instance of the <see cref="KeyedCollectionGeneric{TKey, TItem}"/> class.</summary>
        /// <param name="keySelector">The key selector.</param>
        public KeyedCollectionGeneric(Func<TItem, TKey> keySelector) {
            _keySelector = keySelector;
        }

        /// <summary>When implemented in a derived class, extracts the key from the specified element.</summary>
        /// <returns>The key for the specified element.</returns>
        /// <param name="item">The element from which to extract the key.</param>
        protected override TKey GetKeyForItem(TItem item) {
            return _keySelector(item);
        }

        /// <summary>Determines whether an item with the specified key is contained in this collection.</summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns <c>true</c> if an item with specifed key exists, otherwise <c>false</c>.</returns>
        public bool ContainsKey(TKey key) {
            return Contains(key);
        }
    }

}