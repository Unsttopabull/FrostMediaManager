namespace Frost.Common.Models {

    public interface ICertification : IMovieEntity {
        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        string Rating { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        long MovieId { get; set; }

        /// <summary>Gets or sets the movie this certification applies to.</summary>
        /// <value>The movie this certification applies to.</value>
        IMovie Movie { get; set; }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        ICountry Country { get; set; }
    }

    public interface ICertification<TMovie, TCountry> : ICertification where TMovie : IMovie where TCountry : ICountry {
        /// <summary>Gets or sets the movie this certification applies to.</summary>
        /// <value>The movie this certification applies to.</value>
        new TMovie Movie { get; set; }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        new TCountry Country { get; set; }
    }

}