using Frost.InfoParsers.Models;
using Frost.InfoParsers.Models.Art;
using Frost.InfoParsers.Models.Info;
using Frost.InfoParsers.Models.Subtitles;
using Frost.MovieInfoProviders.Art;
using Frost.MovieInfoProviders.Info;
using Frost.MovieInfoProviders.Info.OmdbAPI;
using Frost.MovieInfoProviders.Info.TraktTV;
using Frost.MovieInfoProviders.Plugin;
using Frost.MovieInfoProviders.Subtitles;
using Frost.MovieInfoProviders.Videos;
using LightInject;

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

            registry.Register<ISubtitleClient, OpenSubtitlesSubtitleClient>(OpenSubtitlesSubtitleClient.CLIENT_NAME);
        }
    }
}
