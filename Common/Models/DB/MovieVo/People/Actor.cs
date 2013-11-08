using Common.Models.XML.Jukebox;

namespace Common.Models.DB.MovieVo.People {

    /// <summary>Represents an actor in a movie.</summary>
    public class Actor : Person {
        private string _character;

        /// <summary>Initializes a new instance of the <see cref="Actor"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        public Actor(string name) : base(name) {
        }

        /// <summary>Initializes a new instance of the <see cref="Actor"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        /// <param name="thumb">The thumbnail image.</param>
        public Actor(string name, string thumb) : base(name, thumb) {
        }

        public Actor(string name, string thumb, string character) : base(name, thumb) {
            _character = character;
        }

        /// <summary>Initializes a new instance of the <see cref="Actor"/> class.</summary>
        /// <param name="person">The person portraying the character.</param>
        /// <param name="character">The character the actor is portraying in this movie.</param>
        public Actor(Person person, string character) : base(person.Id, person.Name, person.Thumb) {
            _character = character;
        }

        /// <summary>Gets or sets the character the actor is portraying in this movie.</summary>
        /// <value>The character the actor is portraying in this movie.</value>
        public string Character {
            get {
                return string.IsNullOrEmpty(_character)
                               ? null
                               : _character;
            }
            set { _character = value; }
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