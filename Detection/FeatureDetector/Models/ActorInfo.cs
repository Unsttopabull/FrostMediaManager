
namespace Frost.DetectFeatures.Models {

    public class ActorInfo : PersonInfo {

        public ActorInfo() {
            
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

        public ActorInfo(string name, string character, string thumb) : base(name, thumb) {
            Character = character;
        }

        public string Character { get; set; }
    }

}