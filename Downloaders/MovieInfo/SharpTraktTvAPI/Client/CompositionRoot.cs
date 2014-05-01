using Frost.InfoParsers.Models;
using LightInject;
using SharpTraktTvAPI.Client;

[assembly: CompositionRootType(typeof(CompositionRoot))]
namespace SharpTraktTvAPI.Client {
    public class CompositionRoot : ICompositionRoot {

        /// <summary>Composes services by adding services to the <paramref name="serviceRegistry"/>.</summary>
        /// <param name="serviceRegistry">The target <see cref="T:LightInject.IServiceRegistry"/>.</param>
        public void Compose(IServiceRegistry serviceRegistry) {
            serviceRegistry.Register<IParsingClient, TraktTvClient>(TraktTvClient.CLIENT_NAME);
        }
    }
}
