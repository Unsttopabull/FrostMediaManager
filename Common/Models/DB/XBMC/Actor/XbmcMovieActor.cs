using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.Actor {

    [Table("actorlinkmovie")]
    public class XbmcMovieActor {

        public XbmcMovieActor() {
            Movie = new XbmcMovie();
            Person = new XbmcPerson();
        }

        public XbmcMovieActor(XbmcActor actor, long movieId) {
            Movie = new XbmcMovie();
            MovieId = movieId;

            Person = (XbmcPerson) actor;
            Role = actor.Role;
            Order = actor.Order;
        }

        [Column("idActor", Order = 0)]
        public long PersonId { get; set; }

        [Column("idMovie", Order = 1)]
        public long MovieId { get; set; }

        [Column("strRole")]
        public string Role { get; set; }

        [Column("iOrder")]
        public long Order { get; set; }

        //[ForeignKey("MovieId")]
        public XbmcMovie Movie { get; set; }

        //[ForeignKey("PersonId")]
        public XbmcPerson Person { get; set; }

        public XbmcActor ToActor() {
            return (XbmcActor) this;
        }

        public static explicit operator XbmcActor(XbmcMovieActor actor) {
            return new XbmcActor(actor.Person, actor.Role, actor.Order, actor.MovieId);
        }
    }
}