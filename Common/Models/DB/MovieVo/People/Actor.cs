using System;
using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common.Models.XML.Jukebox;

namespace Frost.Common.Models.DB.MovieVo.People {

    /// <summary>Represents an actor in a movie.</summary>
    [NotMapped]
    public class Actor : Person, IEquatable<Actor> {
        private readonly MovieActor _ma;

        /// <summary>Initializes a new instance of the <see cref="Actor"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        public Actor(string name) : base(name) {
            _ma = new MovieActor(null, this);
        }

        /// <summary>Initializes a new instance of the <see cref="Actor"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        /// <param name="thumb">The thumbnail image.</param>
        public Actor(string name, string thumb) : base(name, thumb) {
            _ma = new MovieActor(null, this);
        }

        public Actor(string name, string thumb, string character) : base(name, thumb) {
            _ma = new MovieActor(null, this, character);
        }

        /// <summary>Initializes a new instance of the <see cref="Actor"/> class.</summary>
        /// <param name="person">The person portraying the character.</param>
        /// <param name="character">The character the actor is portraying in this movie.</param>
        public Actor(Person person, string character) : base(person.Id, person.Name, person.Thumb) {
           _ma = new MovieActor(null, this, character);
        }

        public Actor(MovieActor movieActor) {
            _ma = movieActor;
        }

        /// <summary>Gets or sets the character the actor is portraying in this movie.</summary>
        /// <value>The character the actor is portraying in this movie.</value>
        public string Character {
            get {
                return string.IsNullOrEmpty(_ma.Character)
                    ? null
                    : _ma.Character;
            }
            set { _ma.Character = value; }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Actor other) {
            return base.Equals(other) && _ma.Character == other.Character;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name + " as " + Character;
        }

        /// <summary>
        /// Converts the specifed <see cref="Actor"/> to an instance of 
        /// <see cref="Common.Models.XML.XBMC.XbmcXmlActor">XbmcXmlActor</see>,
        /// if fields are <c>null</c> they are converted to an empty string
        /// </summary>
        /// <param name="act">The <see cref="Actor"/> to convert</param>
        /// <returns>
        /// An instance of <see cref="Common.Models.XML.XBMC.XbmcXmlActor">XbmcXmlActor</see>
        /// converted from <see cref="Actor"/> with fields as empty strings if <c>null</c>
        /// </returns>
        public static explicit operator XjbXmlActor(Actor act) {
            return new XjbXmlActor(
                act.Character ?? "",
                act.Name ?? "",
                act.Thumb
                );
        }

    }

}
