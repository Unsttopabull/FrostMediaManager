using System.Collections.Generic;

namespace Frost.Common.Models {

    public interface IStudio : IHasName, IMovieEntity {

        /// <summary>Gets or sets the movies this studio has produced.</summary>
        /// <value>The movies this studio has produced.</value>
        ICollection<IMovie> Movies { get; }
    }

     public interface IStudio<TMovie> : IStudio where TMovie : IMovie {
        
        /// <summary>Gets or sets the movies this studio has produced.</summary>
        /// <value>The movies this studio has produced.</value>
        new ICollection<TMovie> Movies { get; }
    }

}