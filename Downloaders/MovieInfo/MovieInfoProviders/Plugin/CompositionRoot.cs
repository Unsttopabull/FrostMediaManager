using Frost.InfoParsers.Models;
using Frost.InfoParsers.Models.Art;
using Frost.InfoParsers.Models.Info;
using Frost.MovieInfoProviders.Plugin;
using LightInject;
using SharpFanartTv;
using SharpOmdbAPI.Client;
using SharpTrailerAddictAPI;
using SharpTraktTvAPI.Client;

[assembly: CompositionRootType(typeof(CompositionRoot))]
namespace Frost.MovieInfoProviders.Plugin {
    public class CompositionRoot : ICompositionRoot {

        /// <summary>Composes services by adding services to the <paramref name="registry"/>.</summary>
        /// <param name="registry">The target <see cref="T:LightInject.IServiceRegistry"/>.</param>
        public void Compose(IServiceRegistry registry) {
            registry.Register<IParsingClient, GremoVKinoClient>(GremoVKinoClient.CLIENT_NAME);
            registry.Register<IParsingClient, KolosejClient>(KolosejClient.CLIENT_NAME);
            registry.Register<IParsingClient, OpenSubtitlesInfoClient>(OpenSubtitlesInfoClient.CLIENT_NAME);
            registry.Register<IParsingClient, OmdbClient>(OmdbClient.CLIENT_NAME);
            registry.Register<IParsingClient, TraktTvClient>(TraktTvClient.CLIENT_NAME);

            registry.Register<IFanartClient, FanartTvClient>(FanartTvClient.CLIENT_NAME);

            registry.Register<IPromotionalVideoClient, TrailerAddictClient>(TrailerAddictClient.CLIENT_NAME);
        }
    }
}
