using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;

namespace Frost.Common {
    public interface IMoviesDataService : IDisposable {

        ObservableCollection<IMovie> Movies { get; }
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

        bool HasUnsavedChanges();
        void SaveChanges();

        void SaveDetected(MovieInfo movieInfo);

    }
}
