using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace RibbonUI.Util {

    /// <summary>Represents a set of values that is observable.</summary>
    /// <typeparam name="T">The type of elements in the hash set.</typeparam>
    public class ObservableHashSet2<T> : INotifyPropertyChanged, INotifyCollectionChanged, IList, IList<T> {
        private readonly HashSet<T> _hashSet;

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Occurs when the collection changes.</summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1"/> class that is empty and uses the default equality comparer for the set type.</summary>
        public ObservableHashSet2() {
            _hashSet = new HashSet<T>();
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1"/> class that is empty and uses the default equality comparer for the set type.</summary>
        public ObservableHashSet2(HashSet<T> hash) {
            _hashSet = hash;
        }

        public ObservableHashSet2(IEnumerable<T> hash) {
            _hashSet = new HashSet<T>(hash);
        }

        #region HasSet<T> Wrappers

        /// <summary>Adds the specified element to a set.</summary>
        /// <returns>true if the element is added to the <see cref="T:System.Collections.Generic.HashSet`1"/> object; false if the element is already present.</returns>
        /// <param name="item">The element to add to the set.</param>
        public bool Add(T item) {
            if (_hashSet.Add(item)) {
                NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                NotifyPropertyChanged("Count");
                return true;
            }
            return false;
        }

        /// <summary>Modifies the current <see cref="T:System.Collections.Generic.HashSet`1"/> object to contain only elements that are present in that object and in the specified collection.</summary>
        /// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1"/> object.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public void IntersectWith(IEnumerable<T> other) {
            _hashSet.IntersectWith(other);

            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            NotifyPropertyChanged("Count");
        }

        /// <summary>Removes all elements in the specified collection from the current <see cref="T:System.Collections.Generic.HashSet`1"/> object.</summary>
        /// <param name="other">The collection of items to remove from the <see cref="T:System.Collections.Generic.HashSet`1"/> object.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public void ExceptWith(IEnumerable<T> other) {
            _hashSet.ExceptWith(other);

            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, other));
            NotifyPropertyChanged("Count");
        }

        /// <summary>Modifies the current <see cref="T:System.Collections.Generic.HashSet`1"/> object to contain only elements that are present either in that object or in the specified collection, but not both.</summary>
        /// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1"/> object.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public void SymmetricExceptWith(IEnumerable<T> other) {
            _hashSet.SymmetricExceptWith(other);

            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            NotifyPropertyChanged("Count");
        }

        /// <summary>Removes all elements that match the conditions defined by the specified predicate from a <see cref="T:System.Collections.Generic.HashSet`1"/> collection.</summary>
        /// <returns>The number of elements that were removed from the <see cref="T:System.Collections.Generic.HashSet`1"/> collection.</returns>
        /// <param name="match">The <see cref="T:System.Predicate`1"/> delegate that defines the conditions of the elements to remove.</param><exception cref="T:System.ArgumentNullException"><paramref name="match"/> is null.</exception>
        public int RemoveWhere(Predicate<T> match) {
            IList<T> removed = _hashSet.Where(item => match(item)).ToList();

            int numRemoved = _hashSet.RemoveWhere(match);
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
            NotifyPropertyChanged("Count");

            return numRemoved;
        }

        /// <summary>Modifies the current <see cref="T:System.Collections.Generic.HashSet`1"/> object to contain all elements that are present in itself, the specified collection, or both.</summary>
        /// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1"/> object.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public void UnionWith(IEnumerable<T> other) {
            _hashSet.UnionWith(other);

            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, other));
            NotifyPropertyChanged("Count");
        }

        #endregion

        #region ICollection<T>

        /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        void ICollection<T>.Add(T item) {
            Add(item);
        }

        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.</summary>
        /// <returns>true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.</returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(T item) {
            return _hashSet.Contains(item);
        }

        /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(T[] array, int arrayIndex) {
            _hashSet.CopyTo(array, arrayIndex);
        }

        /// <summary>Removes the specified element from a <see cref="T:System.Collections.Generic.HashSet`1"/> object.</summary>
        /// <returns>true if the element is successfully found and removed; otherwise, false.  This method returns false if <paramref name="item"/> is not found in the <see cref="T:System.Collections.Generic.HashSet`1"/> object.</returns>
        /// <param name="item">The element to remove.</param>
        public bool Remove(T item) {
            if (_hashSet.Remove(item)) {
                NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                NotifyPropertyChanged("Count");
                return true;
            }
            return false;
        }

        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</returns>
        public int Count {
            get { return _hashSet.Count; }
        }

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
        public bool IsReadOnly {
            get { return false; } 
        }

        /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing. </param><param name="index">The zero-based index in <paramref name="array"/> at which copying begins. </param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. </exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.-or-The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        void ICollection.CopyTo(Array array, int index) {
            _hashSet.CopyTo((T[]) array, index);
        }

        /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
        public object SyncRoot { get; private set; }

        /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).</summary>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
        public bool IsSynchronized { get; private set; }

        #endregion

        #region IList<T>

        /// <summary>Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.</summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void RemoveAt(int index) {
            T item = this[index];
            Remove(item);
        }

        /// <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void Insert(int index, T item) {
            Add(item);
        }

        /// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.</summary>
        /// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf(T item) {
            if (!_hashSet.Contains(item)) {
                return -1;
            }

            int idx = -1;
            foreach (T tItem in _hashSet) {
                idx++;
                if (tItem.Equals(item)) {
                    return idx;
                }
            }
            return idx;            
        }

        #endregion

        #region IList

        /// <summary>Adds an item to the <see cref="T:System.Collections.IList"/>.</summary>
        /// <returns>The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection</returns>
        /// <param name="value">The object to add to the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public int Add(object value) {
            if (Add((T) value)) {
                return 1;
            }
            return -1;
        }

        /// <summary>Removes all elements from a <see cref="T:System.Collections.Generic.HashSet`1"/> object.</summary>
        public void Clear() {
            _hashSet.Clear();
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            NotifyPropertyChanged("Count");
        }

        /// <summary>Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.</summary>
        /// <returns>true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.</returns>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param>
        bool IList.Contains(object value) {
            return _hashSet.Contains((T) value);
        }

        /// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.</summary>
        /// <returns>The index of <paramref name="value"/> if found in the list; otherwise, -1.</returns>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param>
        int IList.IndexOf(object value) {
            if (!(value is T)) {
                return -1;
            }

            return IndexOf((T) value);
        }

        /// <summary>Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted. </param><param name="value">The object to insert into the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception><exception cref="T:System.NullReferenceException"><paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
        void IList.Insert(int index, object value) {
            Insert(index, (T) value);
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
                if (index > _hashSet.Count) {
                    throw new IndexOutOfRangeException();
                }

                int idx = -1;
                foreach (T item in _hashSet) {
                    idx++;
                    if (idx == index) {
                        return item;
                    }
                }
                return default(T);
            }
            set {
                if (index > _hashSet.Count) {
                    throw new IndexOutOfRangeException();
                }

                _hashSet.Remove(this[index]);
                _hashSet.Add(value);
            }
        }

        #endregion

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return _hashSet.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        public IEnumerator GetEnumerator() {
            return _hashSet.GetEnumerator();
        }

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
    }

}