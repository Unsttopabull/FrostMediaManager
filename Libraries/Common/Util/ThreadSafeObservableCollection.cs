using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;

namespace Frost.Common.Util {

    /// <summary />Provides a threadsafe ObservableCollection of T<summary />
    public class ThreadSafeObservableCollection<T> : FastObservableCollection<T> {
        #region Data

        private readonly Dispatcher _dispatcher;
        private readonly ReaderWriterLockSlim _lock;
        private static readonly TimeSpan timeout = new TimeSpan(0,0, 30);

        #endregion

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/> class that contains elements copied from the specified list.</summary>
        public ThreadSafeObservableCollection() {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _lock = new ReaderWriterLockSlim();
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/> class that contains elements copied from the specified list.</summary>
        /// <param name="list">The list from which the elements are copied.</param><exception cref="T:System.ArgumentNullException">The <paramref name="list"/> parameter cannot be null.</exception>
        public ThreadSafeObservableCollection(List<T> list) : base(list) {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _lock = new ReaderWriterLockSlim();
        }

        #region Overrides

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/> class that contains elements copied from the specified collection.</summary>
        /// <param name="collection">The collection from which the elements are copied.</param><exception cref="T:System.ArgumentNullException">The <paramref name="collection"/> parameter cannot be null.</exception>
        public ThreadSafeObservableCollection(IEnumerable<T> collection) : base(collection) {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _lock = new ReaderWriterLockSlim();
        }

        /// <summary />
        /// A simple WPF threading extension method, to invoke a delegate
        /// on the correct thread if it is not currently on the correct thread
        /// Which can be used with DispatcherObject types
        /// <summary />
        /// <param name="disp" />The Dispatcher object on which to do the Invoke<param />
        /// <param name="dotIt" />The delegate to run<param />
        /// <param name="priority" />The DispatcherPriority<param />
        private void InvokeIfRequired(Dispatcher disp, Action dotIt, DispatcherPriority priority) {
            if (disp.CheckAccess()) {
                dotIt();
            }
            else {
                disp.Invoke(priority, timeout, dotIt);
            }
        }

        protected override void ClearItems() {
            InvokeIfRequired(_dispatcher, () => {
                _lock.EnterWriteLock();
                try {
                    base.ClearItems();
                }
                finally {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }

        protected override void InsertItem(int index, T item) {
            ThreadState threadState = _dispatcher.Thread.ThreadState;

            InvokeIfRequired(_dispatcher, () => {
                if (index > Count) {
                    return;
                }

                _lock.EnterWriteLock();
                try {
                    base.InsertItem(index, item);
                }
                finally {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }

        protected override void MoveItem(int oldIndex, int newIndex) {
            InvokeIfRequired(_dispatcher, () => {
                _lock.EnterReadLock();
                int itemCount = Count;
                _lock.ExitReadLock();

                if (oldIndex >= itemCount |
                    newIndex >= itemCount |
                    oldIndex == newIndex) {
                    return;
                }

                _lock.EnterWriteLock();
                try {
                    base.MoveItem(oldIndex, newIndex);
                }
                finally {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }

        protected override void RemoveItem(int index) {
            InvokeIfRequired(_dispatcher, () => {
                if (index >= Count) {
                    return;
                }

                _lock.EnterWriteLock();
                try {
                    base.RemoveItem(index);
                }
                finally {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }

        /// <summary />Sets an item<summary />
        protected override void SetItem(int index, T item) {
            InvokeIfRequired(_dispatcher, () => {
                _lock.EnterWriteLock();
                try {
                    base.SetItem(index, item);
                }
                finally {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }

        #endregion
    }

}