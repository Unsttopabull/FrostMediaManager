using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Frost.Common.Util {

    /// <summary>Represents a set of values that is observable.</summary>
    /// <typeparam name="T">The type of elements in the hash set.</typeparam>
    public class ObservableHashSet<T> : HashSet<T>, INotifyPropertyChanged, INotifyCollectionChanged, IList {
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Occurs when the collection changes.</summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args) {
            if (CollectionChanged != null) {
                CollectionChanged(this, args);
            }
        }

        private void NotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1"/> class that is empty and uses the default equality comparer for the set type.</summary>
        public ObservableHashSet() {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1"/> class that is empty and uses the specified equality comparer for the set type.</summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing values in the set, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"/> implementation for the set type.</param>
        public ObservableHashSet(IEqualityComparer<T> comparer) : base(comparer) {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1"/> class that uses the default equality comparer for the set type, contains elements copied from the specified collection, and has sufficient capacity to accommodate the number of elements copied.</summary>
        /// <param name="collection">The collection whose elements are copied to the new set.</param><exception cref="T:System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public ObservableHashSet(IEnumerable<T> collection) : base(collection) {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1"/> class that uses the specified equality comparer for the set type, contains elements copied from the specified collection, and has sufficient capacity to accommodate the number of elements copied.</summary>
        /// <param name="collection">The collection whose elements are copied to the new set.</param><param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing values in the set, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"/> implementation for the set type.</param><exception cref="T:System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public ObservableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) : base(collection, comparer) {
        }

        /// <summary>Adds the specified element to a set.</summary>
        /// <returns>true if the element is added to the <see cref="T:System.Collections.Generic.HashSet`1"/> object; false if the element is already present.</returns>
        /// <param name="item">The element to add to the set.</param>
        public new bool Add(T item) {
            if (base.Add(item)) {
                NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                NotifyPropertyChanged("Count");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <returns>
        /// The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection,
        /// </returns>
        /// <param name="value">The object to add to the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public int Add(object value) {
            if (Add((T) value)) {
                return 1;
            }
            return -1;
        }

        /// <summary>Removes all elements from a <see cref="T:System.Collections.Generic.HashSet`1"/> object.</summary>
        public new void Clear() {
            base.Clear();
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            NotifyPropertyChanged("Count");
        }

        /// <summary>Modifies the current <see cref="T:System.Collections.Generic.HashSet`1"/> object to contain only elements that are present in that object and in the specified collection.</summary>
        /// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1"/> object.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public new void IntersectWith(IEnumerable<T> other) {
            base.IntersectWith(other);

            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            NotifyPropertyChanged("Count");
        }

        /// <summary>Removes all elements in the specified collection from the current <see cref="T:System.Collections.Generic.HashSet`1"/> object.</summary>
        /// <param name="other">The collection of items to remove from the <see cref="T:System.Collections.Generic.HashSet`1"/> object.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public new void ExceptWith(IEnumerable<T> other) {
            base.ExceptWith(other);

            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, other));
            NotifyPropertyChanged("Count");
        }

        /// <summary>Modifies the current <see cref="T:System.Collections.Generic.HashSet`1"/> object to contain only elements that are present either in that object or in the specified collection, but not both.</summary>
        /// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1"/> object.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public new void SymmetricExceptWith(IEnumerable<T> other) {
            base.SymmetricExceptWith(other);

            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            NotifyPropertyChanged("Count");
        }

        /// <summary>Removes all elements that match the conditions defined by the specified predicate from a <see cref="T:System.Collections.Generic.HashSet`1"/> collection.</summary>
        /// <returns>The number of elements that were removed from the <see cref="T:System.Collections.Generic.HashSet`1"/> collection.</returns>
        /// <param name="match">The <see cref="T:System.Predicate`1"/> delegate that defines the conditions of the elements to remove.</param><exception cref="T:System.ArgumentNullException"><paramref name="match"/> is null.</exception>
        public new int RemoveWhere(Predicate<T> match) {
            IList<T> removed = this.Where(item => match(item)).ToList();

            int numRemoved = base.RemoveWhere(match);
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
            NotifyPropertyChanged("Count");

            return numRemoved;
        }

        /// <summary>Modifies the current <see cref="T:System.Collections.Generic.HashSet`1"/> object to contain all elements that are present in itself, the specified collection, or both.</summary>
        /// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1"/> object.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public new void UnionWith(IEnumerable<T> other) {
            base.UnionWith(other);

            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, other));
            NotifyPropertyChanged("Count");
        }

        /// <summary>Removes the specified element from a <see cref="T:System.Collections.Generic.HashSet`1"/> object.</summary>
        /// <returns>true if the element is successfully found and removed; otherwise, false.  This method returns false if <paramref name="item"/> is not found in the <see cref="T:System.Collections.Generic.HashSet`1"/> object.</returns>
        /// <param name="item">The element to remove.</param>
        public new bool Remove(T item) {
            if (base.Remove(item)) {
                NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                NotifyPropertyChanged("Count");
                return true;
            }
            return false;
        }

        /// <summary>Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.</summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void RemoveAt(int index) {
            T item = this[index];
            Remove(item);
        }

        #region IList

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
        /// </returns>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param>
        bool IList.Contains(object value) {
            return Contains((T) value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param>
        int IList.IndexOf(object value) {
            if (!(value is T)) {
                return -1;
            }

            if (!Contains((T) value)) {
                return -1;
            }

            int idx = -1;
            foreach (T tItem in this) {
                idx++;
                if (tItem.Equals(value)) {
                    return idx;
                }
            }
            return idx;
        }

        /// <summary>Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted. </param><param name="value">The object to insert into the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception><exception cref="T:System.NullReferenceException"><paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
        void IList.Insert(int index, object value) {
            Add(value);
        }

        /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.</summary>
        /// <param name="value">The object to remove from the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        void IList.Remove(object value) {
            Remove((T)value);
        }

        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <returns>The element at the specified index.</returns>
        /// <param name="index">The zero-based index of the element to get or set. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList"/> is read-only. </exception>
        object IList.this[int index] {
            get { return this[index]; }
            set { this[index] = (T) value; }
        }

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
        bool IList.IsReadOnly { get { return false; }}

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.</summary>
        /// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.</returns>
        bool IList.IsFixedSize { get { return false; } }

        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <returns>The element at the specified index.</returns>
        /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public T this[int index] {
            get {
                if (index > Count) {
                    throw new IndexOutOfRangeException();
                }

                int idx = -1;
                foreach (T item in this) {
                    idx++;
                    if (idx == index) {
                        return item;
                    }
                }
                return default(T);
            }
            set {
                if (index > Count) {
                    throw new IndexOutOfRangeException();
                }

                base.Remove(this[index]);
                base.Add(value);
            }
        }

        /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing. </param><param name="index">The zero-based index in <paramref name="array"/> at which copying begins. </param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. </exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.-or-The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        void ICollection.CopyTo(Array array, int index) {
            T[] arr = new T[Count - index];

            int i = -1;
            foreach (T item in this) {
                i++;
                if (i >= index) {
                    arr[i] = item;
                }
            }
        }

        /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
        public object SyncRoot { get; private set; }

        /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).</summary>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
        public bool IsSynchronized { get; private set; }
        #endregion
    }

}