using System.Collections.Generic;
using System.Net.Mime;

namespace Frost.Common.Models {

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

        /// <summary>Gets or sets movies where this person was a director.</summary>
        /// <value>The movies where this person was a director.</value>
        ICollection<IMovie> MoviesAsDirector { get; }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        ICollection<IMovie> MoviesAsWriter { get; }

        /// <summary>Gets movies this person acted in.</summary>
        /// <value>The movies this person acted in.</value>
        IEnumerable<IMovie> MoviesAsActor { get; }
    }

    public interface IPerson<TMovie, TActor> : IPerson where TMovie : IMovie where TActor : IActor {
        /// <summary>Gets or sets movies where this person was a director.</summary>
        /// <value>The movies where this person was a director.</value>
        new ICollection<TMovie> MoviesAsDirector { get; }

        /// <summary>Gets or sets a link to movies where this person was an actor.</summary>
        /// <value>A link to movies where this person was an actor.</value>
        new ICollection<TActor> MoviesLink { get; }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        new ICollection<TMovie> MoviesAsWriter { get; }

        /// <summary>Gets movies this person acted in.</summary>
        /// <value>The movies this person acted in.</value>
        new IEnumerable<TMovie> MoviesAsActor { get; }        
    }

}