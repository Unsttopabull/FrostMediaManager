namespace Frost.Common.Proxies {

    /// <summary>An abstract base class used for proxing classes</summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Proxy<T> where T : class {
        protected readonly T Entity;

        /// <summary>Initializes a new instance of the <see cref="Proxy{T}"/> class.</summary>
        /// <param name="entity">The entity to proxy.</param>
        protected Proxy(T entity) {
            Entity = entity;
        }

        /// <summary>Gets the proxied entity.</summary>
        /// <value>The proxied entity.</value>
        public T ProxiedEntity { get { return Entity; } }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Entity.ToString();
        }
    }
}
