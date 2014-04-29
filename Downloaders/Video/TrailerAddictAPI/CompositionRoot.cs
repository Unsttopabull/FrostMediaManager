using Frost.InfoParsers.Models;
using LightInject;
using SharpTrailerAddictAPI;

[assembly: CompositionRootType(typeof(CompositionRoot))]
namespace SharpTrailerAddictAPI {

    public class CompositionRoot : ICompositionRoot {

        /// <summary>Composes services by adding services to the <paramref name="serviceRegistry"/>.</summary>
        /// <param name="serviceRegistry">The target <see cref="T:LightInject.IServiceRegistry"/>.</param>
        public void Compose(IServiceRegistry serviceRegistry) {
            serviceRegistry.Register<IPromotionalVideoClient, TrailerAddictClient>(TrailerAddictClient.CLIENT_NAME);
        }
    }
}
