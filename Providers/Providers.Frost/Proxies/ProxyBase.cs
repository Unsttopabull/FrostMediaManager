using Frost.Providers.Frost.Provider;

namespace Frost.Providers.Frost.Proxies {
    public abstract class ProxyBase<TEntity> where TEntity : class, new() {
        protected readonly TEntity Entity;
        protected readonly FrostMoviesDataDataService Service;

        protected ProxyBase(TEntity entity, FrostMoviesDataDataService service) {
            Entity = entity;
            Service = service;
        }

        public TEntity ProxiedEntity {
            get { return Entity; }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Entity.ToString();
        }
    }
}
