using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.Actor {

    /// <summary>Represents an actor in the XBMC library.</summary>
    [NotMapped]
    public class XbmcActor {

        private readonly long _movieID;
        private XbmcPerson _person;

        /// <summary>Initializes a new instance of the <see cref="XbmcActor"/> class.</summary>
        public XbmcActor() {
        }

        /// <summary> Initializes a new instance of the <see cref="XbmcActor"/> class.</summary>
        /// <param name="person">The person that is portraying the specified character.</param>
        /// <param name="role">The role or character the actor is portraying in this movie.</param>
        /// <param name="order">The position in a list this actor should appear at.</param>
        /// <param name="movieId">The ID of the movie the actor is starring in.</param>
        /// <exception cref="System.ArgumentException">Thrown when the parameter <paramref name="person"/> is <c>null</c></exception>
        internal XbmcActor(XbmcPerson person, string role, long order, long movieId) {
            if (person == null) {
                throw new ArgumentException(@"person must not be null", "person");
            }
            _person = person;

            Role = role;
            Order = order;
            _movieID = movieId;
        }

        /// <summary>Gets or sets the database person Id.</summary>
        /// <value>The database person Id</value>
        public long Id {
            get { return _person.Id; }
            set { _person.Id = value; }
        }

        /// <summary>Gets or sets the full name of the actor.</summary>
        /// <value>The full name of the actor.</value>
        public string Name {
            get { return _person.Name; }
            set { _person.Name = value; }
        }

        /// <summary>Gets or sets the actors xml serialized thumbnail image.</summary>
        /// <value>The xml serialized thumbnail image.</value>
        public string ThumbXml {
            get { return _person.ThumbXml; }
            set { _person.ThumbXml = value; }
        }

        /// <summary>Gets or sets the URI to the persons thumbnail image.</summary>
        /// <value>The URI to the persons thumbnail image.</value>
        public string ThumbURL {
            get { return _person.ThumbURL; }
            set { _person.ThumbURL = value; }
        }

        /// <summary>Gets or sets the role or character the actor is portraying in this movie.</summary>
        /// <value>The role or character the actor is portraying in this movie.</value>
        public string Role { get; set; }

        /// <summary>Gets or sets the position in a list this actor should appear at.</summary>
        /// <value>The position in a list this actor should appear at.</value>
        public long Order { get; set; }

        /// <summary>Converts this instance to an instance of <see cref="Common.Models.DB.XBMC.Actor.XbmcMovieActor">XbmcMovieActor</see></summary>
        /// <returns>An instance of <see cref="Common.Models.DB.XBMC.Actor.XbmcMovieActor">XbmcMovieActor</see> converted from <see cref="XbmcActor"/></returns>
        public XbmcMovieActor ToMovieActor() {
            return (XbmcMovieActor) this;
        }

        /// <summary>Converts the specifed <see cref="XbmcActor"/> to an instance of <see cref="Common.Models.DB.XBMC.Actor.XbmcMovieActor">XbmcMovieActor</see></summary>
        /// <param name="actor">The <see cref="XbmcActor"/> to convert</param>
        /// <returns>An instance of <see cref="Common.Models.DB.XBMC.Actor.XbmcMovieActor">XbmcMovieActor</see> converted from <see cref="XbmcActor"/></returns>
        public static explicit operator XbmcMovieActor(XbmcActor actor) {
            return new XbmcMovieActor(actor, actor._movieID);
        }

        /// <summary>Returns an underlying instance <see cref="Common.Models.DB.XBMC.Actor.XbmcPerson">XbmcPeson</see> in <see cref="XbmcActor"/>.</summary>
        /// <param name="actor">The <see cref="XbmcActor"/> from which to get the person instance from.</param>
        /// <returns>An underlying instance of <see cref="Common.Models.DB.XBMC.Actor.XbmcPerson">XbmcPeson</see> in <see cref="XbmcActor"/>.</returns>
        public static explicit operator XbmcPerson(XbmcActor actor) {
            return actor._person;
        }

    }

}
