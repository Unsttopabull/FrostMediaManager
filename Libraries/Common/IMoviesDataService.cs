using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;

namespace Frost.Common {
    public interface IMoviesDataService : IDisposable {

        ObservableCollection<IMovie> Movies { get; }
        IEnumerable<ICountry> Countries { get; }

        IEnumerable<IStudio> Studios { get; }
        IEnumerable<IGenre> Genres { get; }

        IEnumerable<IAward> Awards { get; }

        IEnumerable<IMovieSet> Sets { get; }

        IEnumerable<ILanguage> Languages { get; }

        IEnumerable<ISpecial> Specials { get; }

        IEnumerable<IPerson> People { get; }

        bool HasUnsavedChanges();
        void SaveChanges();

        void SaveDetected(MovieInfo movieInfo);

    }
}
