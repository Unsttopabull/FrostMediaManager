using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Frost.Common.Models.DB.XBMC.Actor {

    /// <summary>Represents a link table in XBMC database between a movie and a person containing the name of the persons charater.</summary>
    [Table("actorlinkmovie")]
    public class XbmcMovieActor {

        /// <summary>Initializes a new instance of the <see cref="XbmcMovieActor"/> class.</summary>
        public XbmcMovieActor() {
            Movie = new XbmcMovie();
            Person = new XbmcPerson();
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcMovieActor"/> class.</summary>
        /// <param name="actor">The actor.</param>
        /// <param name="movieId">The movie identifier.</param>
        public XbmcMovieActor(XbmcActor actor, long movieId) {
            Movie = new XbmcMovie();
            MovieId = movieId;

            Person = (XbmcPerson) actor;
            Role = actor.Role;
            Order = actor.Order;
        }

        /// <summary>Gets or sets the foreign key to the person.</summary>
        /// <value>The foreign key to the person</value>
        [Column("idActor", Order = 0)]
        public long PersonId { get; set; }

        /// <summary>Gets or sets the foreign key to the movie.</summary>
        /// <value>The foreign key to the movie</value>
        [Column("idMovie", Order = 1)]
        public long MovieId { get; set; }

        /// <summary>Gets or sets the role or character the actor is portraying in this movie.</summary>
        /// <value>The role or character the actor is portraying in this movie.</value>
        [Column("strRole")]
        public string Role { get; set; }

        /// <summary>Gets or sets the position in a list this actor should appear at.</summary>
        /// <value>The position in a list this actor should appear at.</value>
        [Column("iOrder")]
        public long Order { get; set; }

        /// <summary>Gets or sets the movie where the linked person is portraying that character.</summary>
        /// <value>The movie where the linked person is portraying that character.</value>
        public XbmcMovie Movie { get; set; }

        /// <summary>Gets or sets the person that is portraying that character in the linked movie</summary>
        /// <value>The person that is portraying that character in the linked movie</value>
        public XbmcPerson Person { get; set; }

        /// <summary>Converts this instance to an instance of <see cref="Common.Models.DB.XBMC.Actor.XbmcActor">XbmcActor</see>.</summary>
        /// <returns>An instance of <see cref="Common.Models.DB.XBMC.Actor.XbmcActor">XbmcActor</see> converted from <see cref="XbmcMovieActor"/>.</returns>
        public XbmcActor ToActor() {
            return (XbmcActor) this;
        }

        /// <summary>Converts an instance of <see cref="XbmcMovieActor"/> to an instance of <see cref="XbmcActor"/>.</summary>
        /// <param name="actor">The movie to actor link to convert</param>
        /// <returns>An instance of <see cref="XbmcActor"/> converted from <see cref="XbmcMovieActor"/>.</returns>
        public static explicit operator XbmcActor(XbmcMovieActor actor) {
            return new XbmcActor(actor.Person, actor.Role, actor.Order, actor.MovieId);
        }

        internal class Configuration : EntityTypeConfiguration<XbmcMovieActor> {

            public Configuration() {
                //1:M with Person
                HasRequired(mp => mp.Person)
                    .WithMany(mp => mp.MoviesAsActor)
                    .HasForeignKey(ma => ma.PersonId);

                //1:M with Movie
                HasRequired(mp => mp.Movie)
                    .WithMany(m => m.Actors)
                    .HasForeignKey(m => m.MovieId);

                //composite primary key
                HasKey(ma => new {ma.PersonId, ma.MovieId});
            }

        }

    }

}
