using Frost.Common;
using Frost.Common.Models;
using Frost.Models.Frost.DB;
using Frost.Models.Frost.DB.Arts;
using Frost.Models.Frost.DB.Files;
using Frost.Models.Frost.DB.People;

namespace Frost.Models.Frost {
    public class FrostModelRegistrator : IModelRegistrator {
        public void Register(SystemModels models) {
            models.SystemName = "Frost";
            models.Register<IActor, Actor>();
            models.Register<IArt, Art>();
            models.Register<IAudio, Audio>();
            models.Register<IAward, Award>();
            models.Register<ICertification, Certification>();
            models.Register<ICountry, Country>();
            models.Register<IFile, File>();
            models.Register<IGenre, Genre>();
            models.Register<ILanguage, Language>();
            models.Register<IMovie, Movie>();
            models.Register<IMovieSet, Set>();
            models.Register<IPerson, Person>();
            models.Register<IPlot, Plot>();
            models.Register<IPromotionalVideo, PromotionalVideo>();
            models.Register<IRating, Rating>();
            models.Register<ISpecial, Special>();
            models.Register<IStudio, Studio>();
            models.Register<ISubtitle, Subtitle>();
            models.Register<IVideo, Video>();
        }
    }
}
