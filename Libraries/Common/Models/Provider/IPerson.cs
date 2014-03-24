namespace Frost.Common.Models.Provider {

    public interface IPerson : IMovieEntity {
        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        string Name { get; set; }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        string Thumb { get; set; }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        string ImdbID { get; set; }

        ///// <summary>Gets or sets movies where this person was a director.</summary>
        ///// <value>The movies where this person was a director.</value>
        //IEnumerable<IMovie> MoviesAsDirector { get; }

        ///// <summary>Gets or sets movies where this person was a writer.</summary>
        ///// <value>The movies where this person was a writer.</value>
        //IEnumerable<IMovie> MoviesAsWriter { get; }
    }

}