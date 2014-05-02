using Frost.InfoParsers.Models;
using LightInject;
using SharpOmdbAPI.Client;


[assembly: CompositionRootType(typeof(CompositionRoot))]
namespace SharpOmdbAPI.Client {
    public class CompositionRoot : ICompositionRoot {

        /// <summary>Composes services by adding services to the <paramref name="serviceRegistry"/>.</summary>
        /// <param name="serviceRegistry">The target <see cref="T:LightInject.IServiceRegistry"/>.</param>
        public void Compose(IServiceRegistry serviceRegistry) {
            serviceRegistry.Register<IParsingClient, OmdbClient>(OmdbClient.CLIENT_NAME);
        }
    }
}
