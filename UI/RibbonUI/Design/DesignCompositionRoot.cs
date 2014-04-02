using Frost.Common;
using Frost.Common.Models.Provider;
using LightInject;
using RibbonUI.Design.Models;

namespace RibbonUI.Design {
    public class DesignCompositionRoot : ICompositionRoot {
        private const string SYSTEM_NAME = "Design";

        /// <summary>Composes services by adding services to the <paramref name="serviceRegistry"/>.</summary>
        /// <param name="serviceRegistry">The target <see cref="T:LightInject.IServiceRegistry"/>.</param>
        public void Compose(IServiceRegistry serviceRegistry) {
            serviceRegistry.Register<IMoviesDataService, DesignMoviesDataService>(SYSTEM_NAME, new PerContainerLifetime());
            
            serviceRegistry.Register<IMovie, DesignMovie>(SYSTEM_NAME);
            serviceRegistry.Register<ICountry, DesignCountry>(SYSTEM_NAME);
            serviceRegistry.Register<ILanguage, DesignLanguage>(SYSTEM_NAME);
            serviceRegistry.Register<IGenre, DesignGenre>(SYSTEM_NAME);
            serviceRegistry.Register<IPerson, DesignPerson>(SYSTEM_NAME);
            serviceRegistry.Register<IMovieSet, DesignSet>(SYSTEM_NAME);
            serviceRegistry.Register<IStudio, DesignStudio>(SYSTEM_NAME);

        }
    }
}
