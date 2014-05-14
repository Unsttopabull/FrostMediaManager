using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Models.Provider;
using Frost.RibbonUI.Properties;

namespace Frost.RibbonUI.Util.ObservableWrappers {

    public class ObservableBase<T> : INotifyPropertyChanged where T : class, IMovieEntity {
        protected readonly T _observedEntity;

        public ObservableBase(T observed) {
            _observedEntity = observed;
        }

        public bool this[string propertyName] {
            get { return _observedEntity[propertyName]; }
            set {
            }
        }

        public T ObservedEntity {
            get { return _observedEntity; }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return _observedEntity != null
                ? _observedEntity.ToString()
                : base.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}