using System;
using System.Collections.Generic;
using Frost.Common.Models;

namespace Frost.Common {
    public interface IMoviesDataService : IDisposable {

        IEnumerable<IMovie> Movies { get; }
        IEnumerable<IFile> Files { get; }
        IEnumerable<IVideo> Videos { get; }
        IEnumerable<IAudio> Audios { get; }

        #region Subtitles

        IEnumerable<ISubtitle> Subtitles { get; }
        ISubtitle AddSubtitle(IMovie movie, ISubtitle subtitle);
        void RemoveSubtitle(IMovie selectedMovie, ISubtitle observedSubtitle);

        #endregion

        IEnumerable<IArt> Art { get; }

        #region Countries

        IEnumerable<ICountry> Countries { get; }
        ICountry AddCountry(IMovie movie, ICountry country);
        void RemoveCountry(IMovie movie, ICountry country);

        #endregion

        #region Studios

        IEnumerable<IStudio> Studios { get; }
        IStudio AddStudio(IStudio studio);
        void RemoveStudio(IStudio studio);

        IStudio AddStudio(IMovie movie, IStudio studio);
        void RemoveStudio(IMovie movie, IStudio studio);

        #endregion

        IEnumerable<IRating> Ratings { get; }

        #region Plots

        IEnumerable<IPlot> Plots { get; }

        void RemovePlot(IMovie movie, IPlot plot);
        IPlot AddPlot(IMovie movie, IPlot plot);

        #endregion

        #region Genres

        IEnumerable<IGenre> Genres { get; }

        IGenre AddGenre(IGenre genre);
        void RemoveGenre(IGenre genre);

        IGenre AddGenre(IMovie movie, IGenre genre);
        void RemoveGenre(IMovie movie, IGenre genre);

        #endregion

        IEnumerable<IAward> Awards { get; }
        IEnumerable<IPromotionalVideo> PromotionalVideos { get; }
        IEnumerable<ICertification> Certifications { get; }

        #region Sets

        IEnumerable<IMovieSet> Sets { get; }
        IMovieSet AddSet(IMovieSet set);
        void RemoveSet(IMovieSet set);

        #endregion

        IEnumerable<ILanguage> Languages { get; }

        #region Specials

        IEnumerable<ISpecial> Specials { get; }
        ISpecial AddSpecial(ISpecial special);
        void RemoveSpecial(ISpecial special);

        #endregion

        #region People

        IEnumerable<IPerson> People { get; }
        IPerson AddPerson(IPerson person);

        IPerson AddDirector(IMovie movie, IPerson director);
        void RemoveDirector(IMovie movie, IPerson director);

        IEnumerable<IActor> Actors { get; }
        IActor AddActor(IMovie movie, IActor actor);
        void RemoveActor(IMovie movie, IActor actor);

        #endregion

        bool HasUnsavedChanges();
        void SaveChanges();

    }
}
