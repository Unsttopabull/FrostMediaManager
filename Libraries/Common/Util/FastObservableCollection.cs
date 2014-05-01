using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace Frost.Common.Util {

    //from http://stackoverflow.com/questions/7687000/fast-performing-and-thread-safe-observable-collection

    public class FastObservableCollection<T> : ObservableCollection<T> {

        /// <summary>
        /// This private variable holds the flag to
        /// turn on and off the collection changed notification.
        /// </summary>
        private bool _suspendCollectionChangeNotification;

        /// <summary>This event is overriden CollectionChanged event of the observable collection.</summary>
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/> class.</summary>
        public FastObservableCollection() {
            _suspendCollectionChangeNotification = false;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/> class that contains elements copied from the specified collection.</summary>
        /// <param name="collection">The collection from which the elements are copied.</param><exception cref="T:System.ArgumentNullException">The <paramref name="collection"/> parameter cannot be null.</exception>
        public FastObservableCollection(IEnumerable<T> collection) : base(collection) {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/> class that contains elements copied from the specified list.</summary>
        /// <param name="list">The list from which the elements are copied.</param><exception cref="T:System.ArgumentNullException">The <paramref name="list"/> parameter cannot be null.</exception>
        public FastObservableCollection(List<T> list) : base(list) {
        }

        /// <summary>
        /// This method adds the given generic list of items
        /// as a range into current collection by casting them as type T.
        /// It then notifies once after all items are added.
        /// </summary>
        /// <param name="items">The source collection.</param>
        public void AddItems(IEnumerable<T> items) {
            SuspendCollectionChangeNotification();
            try {
                foreach (var i in items) {
                    InsertItem(Count, i);
                }
            }
            finally {
                NotifyChanges();
            }
        }

        /// <summary>Raises collection change event.</summary>
        public void NotifyChanges() {
            ResumeCollectionChangeNotification();
            var arg = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(arg);
        }

        /// <summary>
        /// This method removes the given generic list of items as a range
        /// into current collection by casting them as type T.
        /// It then notifies once after all items are removed.
        /// </summary>
        /// <param name="items">The source collection.</param>
        public void RemoveItems(IList items) {
            SuspendCollectionChangeNotification();
            try {
                foreach (var i in items) {
                    Remove((T) i);
                }
            }
            catch (Exception ex) {
                throw new InvalidCastException("Please check the type of items getting removed.", ex);
            }
            finally {
                NotifyChanges();
            }
        }

        /// <summary>Resumes collection changed notification.</summary>
        public void ResumeCollectionChangeNotification() {
            _suspendCollectionChangeNotification = false;
        }

        /// <summary>Suspends collection changed notification.</summary>
        public void SuspendCollectionChangeNotification() {
            _suspendCollectionChangeNotification = true;
        }

        /// <summary>This collection changed event performs thread safe event raising.</summary>
        /// <param name="e">The event argument.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
            // Recommended is to avoid reentry 
            // in collection changed event while collection
            // is getting changed on other thread.
            using (BlockReentrancy()) {
                if (!_suspendCollectionChangeNotification) {
                    NotifyCollectionChangedEventHandler eventHandler = CollectionChanged;
                    if (eventHandler == null) {
                        return;
                    }

                    // Walk thru invocation list.
                    Delegate[] delegates = eventHandler.GetInvocationList();

                    foreach (NotifyCollectionChangedEventHandler handler in delegates) {
                        // If the subscriber is a DispatcherObject and different thread.
                        DispatcherObject dispatcherObject
                            = handler.Target as DispatcherObject;

                        if (dispatcherObject != null && !dispatcherObject.CheckAccess()) {
                            // Invoke handler in the target dispatcher's thread... 
                            // asynchronously for better responsiveness.
                            dispatcherObject.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, handler, this, e);
                        }
                        else {
                            // Execute handler as is.
                            handler(this, e);
                        }
                    }
                }
            }
        }
    }

}