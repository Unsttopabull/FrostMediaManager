using Frost.InfoParsers.Models.Info;

namespace Frost.InfoParsers {
    public class ParsedActor : ParsedPerson, IParsedActor {

        public ParsedActor(string name, string character = null) : base(name){
            Character = character;
        }

        public ParsedActor(string name, string character, string imdbId, string thumb) : base(name, imdbId, thumb) {
            Character = character;
        }

        public string Character { get; private set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} [{2}] as {3}, {1}", Name, Thumb, ImdbID ?? "??", Character ?? "??");
        }
    }
}
