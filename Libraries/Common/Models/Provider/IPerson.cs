namespace Frost.Common.Models.Provider {

    /// <summary>Represents a base infor about a person that is accessible by the UI.</summary>
    public interface IPerson : IMovieEntity, IHasName {

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        string Thumb { get; set; }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        string ImdbID { get; set; }
    }

}