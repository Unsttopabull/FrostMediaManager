using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Frost.Common.Proxies.ChangeTrackers {
    public abstract class ChangeTrackingProxy<T> : Proxy<T> where T : class {
        protected IDictionary<string, object> OriginalValues;
        private readonly HashSet<string> _changedProperties;

        protected ChangeTrackingProxy(T entity) : base(entity){
            _changedProperties = new HashSet<string>();
        }

        public virtual bool IsDirty {
            get { return _changedProperties.Count > 0; }
        }

        public HashSet<string> GetChangedProperties() {
            return new HashSet<string>(_changedProperties);
        }

        protected void MarkChanged(string propertyName) {
            _changedProperties.Add(propertyName);
        }

        protected void UnmarkChanged(string propertyName) {
            _changedProperties.Remove(propertyName);
        }

        protected void TrackChanges<TProperty>(TProperty newValue, [CallerMemberName] string propertyName = null) {
            if (OriginalValues == null || propertyName == null) {
                return;
            }

            //Check if original value exists
            object value;
            if (!OriginalValues.TryGetValue(propertyName, out value)) {
                return;
            }

            //check if new value is equal to the new one
            if (Equals((TProperty) value, newValue)) {
                //if equal remove from changed properties
                _changedProperties.Remove(propertyName);
            }
            else {
                //if different add to changed ones
                _changedProperties.Add(propertyName);
            }
        }
    }

}
