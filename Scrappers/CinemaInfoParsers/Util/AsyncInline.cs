using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Frost.InfoParsers.Util {

    public static class AsyncInline {
        public static void Run(Func<Task> item) {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ => {
                try {
                    await item();
                }
                finally {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();


            SynchronizationContext.SetSynchronizationContext(oldContext);
        }


        public static T Run<T>(Func<Task<T>> item) {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);

            T ret = default(T);
            synch.Post(async _ => {
                try {
                    ret = await item();
                }
                finally {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }


        private class ExclusiveSynchronizationContext : SynchronizationContext {
            private readonly AutoResetEvent _workItemsWaiting = new AutoResetEvent(false);
            private readonly Queue<Tuple<SendOrPostCallback, object>> _items = new Queue<Tuple<SendOrPostCallback, object>>();
            private bool _done;

            public override void Send(SendOrPostCallback d, object state) {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state) {
                lock (_items) {
                    _items.Enqueue(Tuple.Create(d, state));
                }
                _workItemsWaiting.Set();
            }

            public void EndMessageLoop() {
                Post(_ => _done = true, null);
            }

            public void BeginMessageLoop() {
                while (!_done) {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (_items) {
                        if (_items.Count > 0) {
                            task = _items.Dequeue();
                        }
                    }
                    if (task != null) {
                        task.Item1(task.Item2);
                    }
                    else {
                        _workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy() {
                return this;
            }
        }
    }

}