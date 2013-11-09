using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.Jukebox {

    /// <summary>
    /// Represents a link table between Movie and a Person containing person's job in the movie.
    /// If they are an actor it also contains their character.
    /// </summary>
    public class MoviesPersons {

        /// <summary>Gets or sets the Id of this person to movie link in the database.</summary>
        /// <value>The Id of this person to movie link in the database.</value>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>Gets or sets the foreign key to the movie.</summary>
        /// <value>The foreign key to the movie</value>
        [Column("movie_id")]
        public long? MovieId { get; set; }

        /// <summary>Gets or sets the foreign key to the person.</summary>
        /// <value>The foreign key to the person.</value>
        [Column("person_id")]
        public long? PersonId { get; set; }

        /// <summary>Gets or sets the character the person is portraying in this movie.</summary>
        /// <value>The character the person is portraying in this movie.</value>
        [Column("character")]
        public string Character { get; set; }

        /// <summary>Gets or sets the person's job in this movie.</summary>
        /// <value>The person's job in this movie</value>
        [Column("job")]
        public string Job { get; set; }

    }

}
