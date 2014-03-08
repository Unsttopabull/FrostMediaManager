using System.Collections.Generic;

namespace Frost.Common.Models {

    public interface IGenre : IHasName, IMovieEntity {

        /// <summary>Gets or sets the movies of this genre.</summary>
        /// <value>The movies of this genre.</value>
        ICollection<IMovie> Movies { get; }
    }


    public interface IGenre<TMovie> : IGenre {

        /// <summary>Gets or sets the movies of this genre.</summary>
        /// <value>The movies of this genre.</value>
        new ICollection<TMovie> Movies { get; }
    }

}