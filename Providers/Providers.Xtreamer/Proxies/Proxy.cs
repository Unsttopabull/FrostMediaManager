namespace Frost.Providers.Xtreamer.Proxies {
    public abstract class Proxy<T> where T : class {
        protected readonly T Entity;

        protected Proxy(T entity) {
            Entity = entity;
        }

        public T ObservedEntity { get { return Entity; } }
    }
}
