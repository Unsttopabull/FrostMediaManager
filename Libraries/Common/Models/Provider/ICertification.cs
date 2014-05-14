namespace Frost.Common.Models.Provider {

    /// <summary>Represents the information about a certification that is accessible by the UI.</summary>
    public interface ICertification : IMovieEntity {

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        string Rating { get; set; }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        ICountry Country { get; set; }
    }

}