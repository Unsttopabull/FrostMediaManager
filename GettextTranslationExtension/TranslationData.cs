using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GettextTranslationExtension {

    public class TranslationData : IWeakEventListener, INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;
        private static readonly Type LanguageChangedEventManagerType = typeof(LanguageChangedEventManager);
        private readonly string _key;

        public TranslationData(string key) {
            _key = key;
            LanguageChangedEventManager.AddListener(TranslationManager.Instance, this);
        }

        public string Value { get { return TranslationManager.Instance.Translate(_key); } }

        /// <summary>Receives events from the centralized event manager.</summary>
        /// <returns>true if the listener handled the event. It is considered an error by the <see cref="T:System.Windows.WeakEventManager"/> handling in WPF to register a listener for an event that the listener does not handle. Regardless, the method should return false if it receives an event that it does not recognize or handle.</returns>
        /// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager"/> calling this method.</param>
        /// <param name="sender">Object that originated the event.</param><param name="e">Event data.</param>
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
            if (managerType != LanguageChangedEventManagerType) {
                return false;
            }

            OnPropertyChanged("Value");
            return true;
        }

        #region IDisposable

        public bool IsDisposed { get; private set; }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        void IDisposable.Dispose() {
            Dispose();
        }

        /// <summary>Closes all files in the list and disposes all allocated resources.</summary>
        public void Dispose() {
            if (!IsDisposed) {
                LanguageChangedEventManager.RemoveListener(TranslationManager.Instance, this);
                IsDisposed = true;

                GC.SuppressFinalize(this);
            }
        }

        ~TranslationData() {
            Dispose();
        }

        #endregion

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}