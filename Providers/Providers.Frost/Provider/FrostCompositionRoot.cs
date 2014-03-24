using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.Providers.Frost.DB;
using Frost.Providers.Frost.DB.Files;
using Frost.Providers.Frost.DB.People;
using Frost.Providers.Frost.Provider;
using Frost.Providers.Frost.Proxies;
using LightInject;

[assembly: CompositionRootType(typeof(FrostCompositionRoot))]

namespace Frost.Providers.Frost.Provider {

    public class FrostCompositionRoot : ICompositionRoot {
        private const string SYSTEM_NAME = "Frost";

        /// <summary>Composes services by adding services to the <paramref name="serviceRegistry"/>.</summary>
        /// <param name="serviceRegistry">The target <see cref="T:LightInject.IServiceRegistry"/>.</param>
        public void Compose(IServiceRegistry serviceRegistry) {
            serviceRegistry.Register<IMoviesDataService, FrostMoviesDataDataService>(SYSTEM_NAME, new PerContainerLifetime());

            serviceRegistry.Register<IActor, Actor>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IArt, Art>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IAudio, FrostAudio>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IAward, Award>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<ICertification, FrostCertification>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<ICountry, Country>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IFile, File>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IGenre, Genre>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<ILanguage, Language>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IMovie, FrostMovie>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IMovieSet, Set>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IPerson, Person>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IPlot, Plot>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IPromotionalVideo, PromotionalVideo>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IRating, Rating>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<ISpecial, Special>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<ISubtitle, FrostSubtitle>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IVideo, FrostVideo>(SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IStudio, Studio>(SYSTEM_NAME, new PerRequestLifeTime());
        }
    }

}