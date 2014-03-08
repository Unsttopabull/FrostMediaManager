using System.Collections.Generic;

namespace Frost.Common.Models {

    public interface IMovieSet : IHasName, IMovieEntity {

        /// <summary>Gets or sets the movies in this set.</summary>
        /// <value>The movies in this set.</value>
        ICollection<IMovie> Movies { get; }
    }

    public interface IMovieSet<TMovie> : IMovieSet where TMovie : IMovie {
        /// <summary>Gets or sets the movies in this set.</summary>
        /// <value>The movies in this set.</value>
        new ICollection<TMovie> Movies { get; }
    }

}