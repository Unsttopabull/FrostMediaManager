using System.Collections.Generic;
using Frost.Common.Models.ISO;

namespace Frost.Common.Models {

    public interface ICountry : IMovieEntity {

        long Id { get; }

        /// <summary>Gets or sets the country name.</summary>
        /// <value>The name of the country.</value>
        string Name { get; set; }

        /// <summary>Gets or sets the ISO 3166-1 Information.</summary>
        /// <value>The ISO 3166-1 Information.</value>
        ISO3166 ISO3166 { get; set; }

        ///// <summary>Gets or sets the movies shot in this country.</summary>
        ///// <value>The country movies</value>
        //IEnumerable<IMovie> Movies { get; }
    }

}