using System;
using System.Collections.Generic;
using Frost.Common.Models;

namespace Frost.Common {
    public interface IMoviesDataService : IDisposable {

        IEnumerable<IMovie> Movies { get; }
        IEnumerable<IFile> Files { get; }
        IEnumerable<IVideo> Videos { get; }
        IEnumerable<IAudio> Audios { get; }
        IEnumerable<ISubtitle> Subtitles { get; }
        IEnumerable<IArt> Art { get; }
        IEnumerable<ICountry> Countries { get; }

        IEnumerable<IStudio> Studios { get; }

        IEnumerable<IRating> Ratings { get; }
        IEnumerable<IPlot> Plots { get; }

        IEnumerable<IGenre> Genres { get; }

        IEnumerable<IAward> Awards { get; }
        IEnumerable<IPromotionalVideo> PromotionalVideos { get; }
        IEnumerable<ICertification> Certifications { get; }

        IEnumerable<IMovieSet> Sets { get; }

        IEnumerable<ILanguage> Languages { get; }

        IEnumerable<ISpecial> Specials { get; }

        IEnumerable<IPerson> People { get; }

        IEnumerable<IActor> Actors { get; }

        bool HasUnsavedChanges();
        void SaveChanges();

    }
}
