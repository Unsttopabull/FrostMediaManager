using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common.Models;

namespace Frost.Models.Frost.DB.People {

    public class Actor : Person, IActor {

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        public Actor() {
        }

        public Actor(IActor actor) {
            Name = actor.Name;
            Thumb = actor.Thumb;
            ImdbID = actor.ImdbID;
        }

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        public Actor(string name) : base(name) {
        }

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        /// <param name="thumb">The thumbnail image.</param>
        public Actor(string name, string thumb) : base(name, thumb) {
        }

        public Actor(Person person, string character) {
            Name = person.Name;
            ImdbID = person.ImdbID;
            Thumb = person.Thumb;
            Character = character;
        }

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        /// <param name="thumb">The thumbnail image.</param>
        /// <param name="character"></param>
        internal Actor(string name, string thumb, string character) : this(name, thumb) {
            Character = character;
        }
        #endregion

        public string Character { get; set; }

        public Movie Movie { get; set; }

        [ForeignKey("Movie")]
        public long MovieId { get; set; }

    }
}
