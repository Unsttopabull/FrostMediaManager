
namespace Frost.Common.Models.FeatureDetector {

    /// <summary>Represents an info about an actor that has been detected by Feature Detector.</summary>
    public class ActorInfo : PersonInfo {

        /// <summary>Initializes a new instance of the <see cref="ActorInfo"/> class.</summary>
        public ActorInfo() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="ActorInfo"/> class.</summary>
        public ActorInfo(PersonInfo person, string character) {
            Character = character;
            Name = person.Name;
            ImdbID = person.ImdbID;
            Thumb = person.Thumb;
        }

        /// <summary>Initializes a new instance of the <see cref="ActorInfo"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        public ActorInfo(string name) : base(name) {
        }

        /// <summary>Initializes a new instance of the <see cref="ActorInfo"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        /// <param name="thumb">The thumbnail image.</param>
        public ActorInfo(string name, string thumb) : base(name, thumb) {
        }

        /// <summary>Initializes a new instance of the <see cref="ActorInfo"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        /// <param name="character">The character or role of the actor.</param>
        /// <param name="thumb">The thumbnail image.</param>
        public ActorInfo(string name, string character, string thumb) : base(name, thumb) {
            Character = character;
        }

        /// <summary>Gets the character or role of the actor.</summary>
        /// <value>The character or role of the actor.</value>
        public string Character { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} as {1}", Name, Character ?? "N/A");
        }
    }

}