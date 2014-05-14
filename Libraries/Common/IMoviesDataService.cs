using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;

namespace Frost.Common {

    /// <summary>Inteface for a service that mediates between the Frost Media Manager UI and the provider</summary>
    public interface IMoviesDataService : IDisposable {

        /// <summary>Gets all movies in the provider database.</summary>
        /// <value>The movies in the provider database.</value>
        ObservableCollection<IMovie> Movies { get; }

        /// <summary>Gets all countries in the provider database.</summary>
        /// <value>The countries in the provider database.</value>
        IEnumerable<ICountry> Countries { get; }

        /// <summary>Gets all studios in the provider database.</summary>
        /// <value>The studios in the provider database.</value>
        IEnumerable<IStudio> Studios { get; }

        /// <summary>Gets all genres in the provider database.</summary>
        /// <value>The genres in the provider database.</value>
        IEnumerable<IGenre> Genres { get; }

        /// <summary>Gets all awards in the provider database.</summary>
        /// <value>The awards in the provider database.</value>
        IEnumerable<IAward> Awards { get; }

        /// <summary>Gets all sets in the provider database.</summary>
        /// <value>The sets in the provider database.</value>
        IEnumerable<IMovieSet> Sets { get; }

        /// <summary>Gets all languages in the provider database.</summary>
        /// <value>The languages in the provider database.</value>
        IEnumerable<ILanguage> Languages { get; }

        /// <summary>Gets all specials in the provider database.</summary>
        /// <value>The specials in the provider database.</value>
        IEnumerable<ISpecial> Specials { get; }

        /// <summary>Gets all the people in the provider database.</summary>
        /// <value>All people in the provider database.</value>
        IEnumerable<IPerson> People { get; }

        /// <summary>Determines whether there are unsaved changes left.</summary>
        /// <returns>Returns <c>true</c> if unsaved changed are present, othrewise <c>false</c>.</returns>
        bool HasUnsavedChanges();

        /// <summary>Saves the changes made to the provider database.</summary>
        void SaveChanges();

        /// <summary>Saves the new detected movie in the provider database.</summary>
        /// <param name="movieInfo">The movie information to be saved.</param>
        void SaveDetected(MovieInfo movieInfo);

        /// <summary>Removes the movie from the provider database.</summary>
        /// <param name="movie">The movie to remove.</param>
        /// <returns>Returns <c>true</c> if successfull otherwise <c>false</c>.</returns>
        bool RemoveMovie(IMovie movie);

    }
}
