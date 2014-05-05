using Frost.InfoParsers.Models.Art;
using LightInject;
using SharpFanartTv;

[assembly: CompositionRootType(typeof(CompositionRoot))]
namespace SharpFanartTv {
    public class CompositionRoot : ICompositionRoot {

        /// <summary>Composes services by adding services to the <paramref name="serviceRegistry"/>.</summary>
        /// <param name="serviceRegistry">The target <see cref="T:LightInject.IServiceRegistry"/>.</param>
        public void Compose(IServiceRegistry serviceRegistry) {
            serviceRegistry.Register<IFanartClient, FanartTvArtClient>(FanartTvArtClient.CLIENT_NAME);
        }
    }
}
