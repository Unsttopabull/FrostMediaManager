using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.Providers.Xbmc.DB;
using Frost.Providers.Xbmc.DB.People;
using Frost.Providers.Xbmc.DB.Proxy;
using Frost.Providers.Xbmc.Provider;
using Frost.Providers.Xbmc.Proxies;
using LightInject;

[assembly: CompositionRootType(typeof(XbmcCompositionRoot))]
namespace Frost.Providers.Xbmc.Provider {

    public class XbmcCompositionRoot : ICompositionRoot {
        private const string SYSTEM_NAME = "XBMC";

        /// <summary>Composes services by adding services to the <paramref name="serviceRegistry"/>.</summary>
        /// <param name="serviceRegistry">The target <see cref="T:LightInject.IServiceRegistry"/>.</param>
        public void Compose(IServiceRegistry serviceRegistry) {
            serviceRegistry.Register<IMoviesDataService, XbmcMoviesDataService>(SYSTEM_NAME, new PerContainerLifetime());

            serviceRegistry.Register<IActor, XbmcMovieActor>(SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IArt, XbmcMovieArt>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IAudio, XbmcAudioDetails>(SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IAward>(f => new Award(), SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<ICertification, XbmcCertification>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<ICountry, XbmcCountry>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IFile, XbmcFile>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IGenre, XbmcGenre>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<ILanguage, XbmcLanguage>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IMovie, XbmcMovie>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IMovieSet, XbmcSet>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IPerson, XbmcPerson>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IPlot, XbmcPlot>(SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IPromotionalVideo>(f => new PromotionalVideo(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IRating>(f => new Rating(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<ISpecial>(f => new Special(), SYSTEM_NAME, new PerRequestLifeTime());

            serviceRegistry.Register<IStudio, XbmcStudio>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<ISubtitle, XbmcSubtitleDetails>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IVideo, XbmcVideoDetails>(SYSTEM_NAME, new PerRequestLifeTime());
        }
    }

}