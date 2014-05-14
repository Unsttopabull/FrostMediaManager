using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Frost.Common.Proxies.ChangeTrackers {

    /// <summary>Represents a proxy that tracks changes to the entity.</summary>
    /// <typeparam name="T">The type of an entity to track.</typeparam>
    public abstract class ChangeTrackingProxy<T> : Proxy<T> where T : class {
        private readonly HashSet<string> _changedProperties;
        /// <summary>The entity values at the start of change tracking.</summary>
        protected IDictionary<string, object> OriginalValues;

        /// <summary>Initializes a new instance of the <see cref="ChangeTrackingProxy{T}"/> class.</summary>
        /// <param name="entity">The entity to track.</param>
        protected ChangeTrackingProxy(T entity) : base(entity){
            _changedProperties = new HashSet<string>();
        }

        /// <summary>Gets a value indicating whether the entity has changed since the tracking began.</summary>
        /// <value>Is <c>true</c> if there are changes; otherwise, <c>false</c>.</value>
        public virtual bool IsDirty {
            get { return _changedProperties.Count > 0; }
        }

        /// <summary>Gets the names of the changed properties.</summary>
        /// <returns>Returns the names of the changed properties</returns>
        public HashSet<string> GetChangedProperties() {
            return new HashSet<string>(_changedProperties);
        }

        /// <summary>Marks the property as changed.</summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void MarkChanged(string propertyName) {
            _changedProperties.Add(propertyName);
        }

        /// <summary>Marks the property as unchanged.</summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void UnmarkChanged(string propertyName) {
            _changedProperties.Remove(propertyName);
        }

        /// <summary>Checks for the changes of the property.</summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="newValue">The new value of a property.</param>
        /// <param name="propertyName">Name of the property.</param>
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
