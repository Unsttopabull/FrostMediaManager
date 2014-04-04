using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Frost.Common.Proxies.ChangeTrackers {

    public class ChangeTrackingCollection<TItem> : ICollection<TItem> where TItem : IEquatable<TItem> {
        protected readonly ICollection<TItem> Collection;
        private readonly Action<TItem> _add;
        private readonly Action<TItem> _remove;
        private readonly HashSet<TItem> _addedItems;
        private readonly HashSet<TItem> _removedItems;
        private readonly IEqualityComparer<TItem> _comparer;

        public ChangeTrackingCollection(ICollection<TItem> collection) {
            Collection = collection;
            _addedItems = new HashSet<TItem>();
            _removedItems = new HashSet<TItem>();
        }

        public ChangeTrackingCollection(ICollection<TItem> collection, IEqualityComparer<TItem> comparer) : this(collection) {
            _comparer = comparer;
        }

        public ChangeTrackingCollection(ICollection<TItem> collection, Action<TItem> add, Action<TItem> remove) : this(collection){
            _add = add;
            _remove = remove;
        }

        public ChangeTrackingCollection(ICollection<TItem> collection, Action<TItem> add, Action<TItem> remove, IEqualityComparer<TItem> comparer) : this(collection, add, remove) {
            _comparer = comparer;
        }

        /// <summary>Returns true if any items have been added or removed since tracking began.</summary>
        public bool IsDirty { get { return _addedItems.Count > 0 || _removedItems.Count > 0; } }

        /// <summary>Returns the added items since tracking began.</summary>
        public IEnumerable<TItem> AddedItems {
            get { return _addedItems; }
        }

        /// <summary>Returns the removed items since tracking began.</summary>
        public IEnumerable<TItem> RemovedItems {
            get { return _removedItems; }
        }

        #region IEnumerable

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<TItem> GetEnumerator() {
            return Collection.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable) Collection).GetEnumerator();
        }

        #endregion

        #region Add/Remove/Clear

        /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(TItem item) {
            if (_comparer == null ? Contains(item) : this.Contains(item, _comparer)) {
                return;
            }

            Collection.Add(item);
            TrackAdd(item);

            if (_add != null) {
                _add(item);
            }
        }

        /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <returns>true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(TItem item) {
            if (!Collection.Remove(item)) {
                return false;
            }

            TrackRemove(item);
            if (_remove != null) {
                _remove(item);
            }
            return true;
        }

        public void RemoveWhere(Predicate<TItem> predicate) {
            foreach (TItem item in this.Where(item => predicate(item))) {
                Remove(item);
            }
        }

        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear() {
            Collection.Clear();
        }

        #endregion

        #region Tracking

        private void TrackRemove(TItem item) {
            bool contains = _comparer == null
                ? _addedItems.Contains(item)
                : _addedItems.Contains(item, _comparer);

            if (contains) {
                _addedItems.Remove(item);
            }
            else {
                _removedItems.Add(item);
            }
        }

        private void TrackAdd(TItem item) {
            bool contains = _comparer == null
                ? _removedItems.Contains(item)
                : _removedItems.Contains(item, _comparer);

            if (contains) {
                _removedItems.Remove(item);
            }
            else {
                _addedItems.Add(item);
            }
        }

        #endregion

        #region ICollection<T>

        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.</summary>
        /// <returns>true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.</returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(TItem item) {
            return Collection.Contains(item);
        }

        /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(TItem[] array, int arrayIndex) {
            Collection.CopyTo(array, arrayIndex);
        }

        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        public int Count {
            get { return Collection.Count; }
        }

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
        public bool IsReadOnly {
            get { return Collection.IsReadOnly; }
        }

        #endregion
    }

}