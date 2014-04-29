using Frost.Common.Models.Provider.ISO;

namespace Frost.Common.Models.Provider {

    public interface ICountry : IMovieEntity, IHasName {

        /// <summary>Gets or sets the ISO 3166-1 Information.</summary>
        /// <value>The ISO 3166-1 Information.</value>
        ISO3166 ISO3166 { get; set; }
    }

}