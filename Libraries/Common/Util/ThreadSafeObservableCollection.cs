using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using Frost.Common;

/// <summary />Provides a threadsafe ObservableCollection of T<summary />
public class ThreadSafeObservableCollection<T> : ObservableCollection<T> {
    #region Data

    private readonly Dispatcher _dispatcher;
    private readonly ReaderWriterLockSlim _lock;

    #endregion

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

    protected override void ClearItems() {
        _dispatcher.InvokeIfRequired(() => {
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
        _dispatcher.InvokeIfRequired(() => {
            if (index > this.Count) {
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
        _dispatcher.InvokeIfRequired(() => {
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
        _dispatcher.InvokeIfRequired(() => {
            if (index >= this.Count) {
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
        _dispatcher.InvokeIfRequired(() => {
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