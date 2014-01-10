using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Frost.Common.Models.DB.MovieVo.People {

    /// <summary>Represents a link table between a movie and a person containing the name of the person's charater.</summary>
    public class MovieActor {

        /// <summary>Initializes a new instance of the <see cref="MovieActor"/> class.</summary>
        /// <param name="movie">The movie the actor is preforming in</param>
        /// <param name="person">The person that is performing in the movie</param>
        /// <param name="character">The character the actor is portraing.</param>
        public MovieActor(Movie movie, Person person, string character){
            Movie = movie;
            Person = person;
            Character = character;
        }

        /// <summary>Initializes a new instance of the <see cref="MovieActor"/> class.</summary>
        /// <param name="movie">The movie the actor is preforming in</param>
        /// <param name="personName">The full name of the actor</param>
        /// <param name="character">The character the actor is portraing.</param>
        public MovieActor(Movie movie, string personName, string character) : this(movie, new Person(personName), character) {
        }

        /// <summary>Initializes a new instance of the <see cref="MovieActor"/> class.</summary>
        /// <param name="actor">The actor in the following movie</param>
        /// <param name="movie">The movie the actor is preforming in</param>
        public MovieActor(Movie movie, Actor actor) : this(movie, actor, actor.Character) {
        }

        /// <summary>Gets or sets the database Id of this link entry.</summary>
        /// <value>The database Id of this link entry</value>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the character the person is portraying in this movie.</summary>
        /// <value>The character the person is portraying in this movie.</value>
        public string Character { get; set; }

        /// <summary>Gets or sets the foreign key to the movie.</summary>
        /// <value>The foreign key to the movie</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the foreign key to the person.</summary>
        /// <value>The foreign key to the person</value>
        public long PersonId { get; set; }

        /// <summary>Gets or sets the movie where the linked person is portraying that character.</summary>
        /// <value>The movie where the linked person is portraying that character.</value>
        public virtual Movie Movie { get; set; }

        /// <summary>Gets or sets the person that is portraying that character in the linked movie</summary>
        /// <value>The person that is portraying that character in the linked movie</value>
        public virtual Person Person { get; set; }

        /// <summary>Converts an instance of <see cref="MovieActor"/> into <see cref="Actor"/></summary>
        /// <param name="ma">The MovieActor to convert.</param>
        /// <returns>
        /// A mew instance of <see cref="Actor"/> converted from <see cref="MovieActor"/> with its
        /// Person information and the character they are portraying in the linked movie
        /// </returns>
        public static explicit operator Actor(MovieActor ma) {
            return new Actor(ma);
        }

        internal class Configuration : EntityTypeConfiguration<MovieActor> {
            public Configuration() {
                ToTable("MovieActors");

                HasRequired(ma => ma.Movie)
                    .WithMany(m => m.ActorsLink)
                    .HasForeignKey(fk => fk.MovieId)
                    .WillCascadeOnDelete();

                HasRequired(ma => ma.Person)
                    .WithMany(p => p.MoviesLink)
                    .HasForeignKey(fk => fk.PersonId)
                    .WillCascadeOnDelete();
            }
        }
    }

}
