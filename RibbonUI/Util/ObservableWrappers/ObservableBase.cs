using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Models;
using RibbonUI.Annotations;

namespace RibbonUI.Util.ObservableWrappers {

    public class ObservableBase<T> : INotifyPropertyChanged where T : class, IMovieEntity {
        public event PropertyChangedEventHandler PropertyChanged;
        protected readonly T _observedEntity;

        public ObservableBase(T observed) {
            _observedEntity = observed;
        }

        public bool this[string propertyName] {
            get { return _observedEntity[propertyName]; }
        }

        public bool IsImplemented([CallerMemberName] string propertyName = null) {
            return _observedEntity[propertyName];
        }

        public T ObservedEntity {
            get { return _observedEntity; }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}