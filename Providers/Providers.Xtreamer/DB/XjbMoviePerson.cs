using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Providers.Xtreamer.DB {

    /// <summary>
    /// Represents a link table between Movie and a Person containing person's job in the movie.
    /// If they are an actor it also contains their character.
    /// </summary>
    [Table("movies_persons")]
    public class XjbMoviePerson {

        public XjbMoviePerson() {
        }

        #region Properties/Columns

        /// <summary>Gets or sets the Id of this person to movie link in the database.</summary>
        /// <value>The Id of this person to movie link in the database.</value>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>Gets or sets the character the person is portraying in this movie.</summary>
        /// <value>The character the person is portraying in this movie.</value>
        [Column("character")]
        public string Character { get; set; }

        /// <summary>Gets or sets the person's job in this movie.</summary>
        /// <value>The person's job in this movie</value>
        [Column("job")]
        public string Job { get; set; }

        #endregion

        #region Foreign keys

        /// <summary>Gets or sets the foreign key to the movie.</summary>
        /// <value>The foreign key to the movie</value>
        [Column("movie_id")]
        public long? MovieId { get; set; }

        /// <summary>Gets or sets the foreign key to the person.</summary>
        /// <value>The foreign key to the person.</value>
        [Column("person_id")]
        public long? PersonId { get; set; }

        #endregion

        /// <summary>Gets or sets the person that this link refers to.</summary>
        /// <value>The person that this link refers to.</value>
        [ForeignKey("PersonId")]
        public virtual XjbPerson Person { get; set; }

        /// <summary>Gets or sets the movie that this link refers to.</summary>
        /// <value>The movie that this link refers to.</value>
        [ForeignKey("MovieId")]
        public virtual XjbMovie Movie { get; set; }
    }

}
