using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Frost.Common.Proxies.ChangeTrackers {

    /// <summary>Tracks changes made to the collection and reporting them.</summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class ObservableChangeTrackingCollection<TItem> : ChangeTrackingCollection<TItem>, INotifyCollectionChanged, IDisposable where TItem : IEquatable<TItem> {

        /// <summary>Reports changes made to the collection</summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>Initializes a new instance of the <see cref="ObservableChangeTrackingCollection{TItem}"/> class.</summary>
        /// <param name="collection">The collection.</param>
        public ObservableChangeTrackingCollection(ICollection<TItem> collection) : base(collection) {
            if (!(collection is INotifyCollectionChanged) && !(collection is IList<TItem>)) {
                throw new ArgumentException("Collection must implement INotifyCollectionChanged or IList<T>");
            }

            INotifyCollectionChanged collectionChanged = Collection as INotifyCollectionChanged;
            if (collectionChanged != null) {
                collectionChanged.CollectionChanged += OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args) {
            if (CollectionChanged != null) {
                CollectionChanged(sender, args);
            }
        }

        #region IDisposable

        /// <summary>Gets a value indicating whether this tracker still forwards the INotifyCollectionChanged event.</summary>
        /// <value><c>true</c> if it does; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            Dispose(false);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        private void Dispose(bool finalizer) {
            if (IsDisposed) {
                return;
            }

            CollectionChanged -= OnCollectionChanged;

            if (!finalizer) {
                GC.SuppressFinalize(this);
            }
            IsDisposed = true;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        ~ObservableChangeTrackingCollection() {
            Dispose(true);
        }

        #endregion
    }
}
