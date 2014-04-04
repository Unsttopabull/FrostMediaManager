namespace Frost.Common.Proxies {

    /// <summary>An abstract base class used for proxing classes with the help of a service.</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TService">The type of the service.</typeparam>
    public abstract class ProxyWithService<TEntity, TService> : Proxy<TEntity> where TEntity : class where TService : IMoviesDataService {

        protected TService Service;

        /// <summary>Initializes a new instance of the <see cref="ProxyWithService{TEntity, TService}"/> class.</summary>
        /// <param name="entity">The entity to proxy.</param>
        /// <param name="service">The service implementation.</param>
        protected ProxyWithService(TEntity entity, TService service) : base(entity) {
            Service = service;
        }
    }
}
