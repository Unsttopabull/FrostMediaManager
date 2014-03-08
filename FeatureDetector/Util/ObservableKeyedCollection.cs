using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Annotations;

namespace Frost.DetectFeatures.Util {

    public abstract class ObservableKeyedCollection<TKey, TValue> : KeyedCollection<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2"/> class that uses the default equality comparer.</summary>
        public ObservableKeyedCollection() {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2"/> class that uses the specified equality comparer.</summary>
        /// <param name="comparer">The implementation of the <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> generic interface to use when comparing keys, or null to use the default equality comparer for the type of the key, obtained from <see cref="P:System.Collections.Generic.EqualityComparer`1.Default"/>.</param>
        public ObservableKeyedCollection(IEqualityComparer<TKey> comparer) : base(comparer) {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2"/> class that uses the specified equality comparer and creates a lookup dictionary when the specified threshold is exceeded.</summary>
        /// <param name="comparer">The implementation of the <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> generic interface to use when comparing keys, or null to use the default equality comparer for the type of the key, obtained from <see cref="P:System.Collections.Generic.EqualityComparer`1.Default"/>.</param><param name="dictionaryCreationThreshold">The number of elements the collection can hold without creating a lookup dictionary (0 creates the lookup dictionary when the first item is added), or –1 to specify that a lookup dictionary is never created.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="dictionaryCreationThreshold"/> is less than –1.</exception>
        public ObservableKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold) : base(comparer, dictionaryCreationThreshold) {
        }

        #region Keyed Collection

        /// <summary>Removes all elements from the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2"/>.</summary>
        protected override void ClearItems() {
            base.ClearItems();

            OnPropertyChanged("Count");
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>Inserts an element into the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2"/> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.-or-<paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.</exception>
        protected override void InsertItem(int index, TValue item) {
            base.InsertItem(index, item);

            OnPropertyChanged("Count");
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2"/>.</summary>
        /// <param name="index">The index of the element to remove.</param>
        protected override void RemoveItem(int index) {
            TValue item = base[index];

            base.RemoveItem(index);

            OnPropertyChanged("Count");
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        /// <summary>Replaces the item at the specified index with the specified item.</summary>
        /// <param name="index">The zero-based index of the item to be replaced.</param><param name="item">The new item.</param>
        protected override void SetItem(int index, TValue item) {
            TValue prevItem = base[index];
            base.SetItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, prevItem, index));
        }

        /// <summary>When implemented in a derived class, extracts the key from the specified element.</summary>
        /// <returns>The key for the specified element.</returns>
        /// <param name="item">The element from which to extract the key.</param>
        protected override TKey GetKeyForItem(TValue item) {
            throw new NotImplementedException("Must me overriden in the derived class");
        }

        #endregion

        public new TValue this[int index] {
            get { return base[index]; }
            set { SetItem(index, value); }
        }

        public void AddRange(IEnumerable<TValue> range) {
            foreach (TValue item in range) {
                Add(item);
            }
            OnPropertyChanged("Count");
        }

        public bool TryGetValue(TKey key, out TValue value) {
            if (Dictionary != null) {
                return Dictionary.TryGetValue(key, out value);
            }

            value = default(TValue);
            return false;
        }

        public void OnCollectionChanged(NotifyCollectionChangedEventArgs args) {
            if (CollectionChanged != null) {
                CollectionChanged(this, args);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
