using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.Provider;
using Frost.Providers.Xtreamer.Proxies;
using LightInject;

[assembly: CompositionRootType(typeof(XtreamerCompositionRoot))]

namespace Frost.Providers.Xtreamer.Provider {

    public class XtreamerCompositionRoot : ICompositionRoot {
        private const string SYSTEM_NAME = "XJB";

        /// <summary>Composes services by adding services to the <paramref name="serviceRegistry"/>.</summary>
        /// <param name="serviceRegistry">The target <see cref="T:LightInject.IServiceRegistry"/>.</param>
        public void Compose(IServiceRegistry serviceRegistry) {
            serviceRegistry.Register<IMoviesDataService, XjbMoviesDataService>(SYSTEM_NAME, new PerContainerLifetime());

            //serviceRegistry.Register<IMovie, XtMovie>(SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IActor>(f => new Actor(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IArt>(f => new Art(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IAudio>(f => new Audio(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IAward>(f => new Award(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<ICertification>(f => new Certification(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<ICountry>(f => new Country(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IFile>(f => new File(), SYSTEM_NAME, new PerRequestLifeTime());
            serviceRegistry.Register<IGenre, XtGenre>(SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<ILanguage>(f => new Language(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IMovie>(f => new Movie(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IMovieSet>(f => new Set(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IPerson>(f => new Person(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IPlot>(f => new Plot(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IPromotionalVideo>(f => new PromotionalVideo(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IRating>(f => new Rating(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<ISpecial>(f => new Special(), SYSTEM_NAME, new PerRequestLifeTime());

           
            ////serviceRegistry.Register<IStudio>(f => new Studio(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<ISubtitle>(f => new Subtitle(), SYSTEM_NAME, new PerRequestLifeTime());

            //serviceRegistry.Register<IVideo>(f => new Video(), SYSTEM_NAME, new PerRequestLifeTime());

            ////serviceRegistry.Register<IStudio>(f => new Studio(), SYSTEM_NAME, new PerRequestLifeTime());
            //serviceRegistry.Register<IStudio, Studio>(SYSTEM_NAME, new PerRequestLifeTime());
        }
    }

}