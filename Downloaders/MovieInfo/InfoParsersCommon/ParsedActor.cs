using Frost.InfoParsers.Models.Info;

namespace Frost.InfoParsers {
    public class ParsedActor : IParsedActor {

        public ParsedActor(string name, string character = null) {
            Name = name;
            Character = character;
        }

        public ParsedActor(string name, string character, string imdbId, string thumb) : this(name, character) {
            ImdbID = imdbId;
            Thumb = thumb;
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        public string Thumb { get; set; }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        public string ImdbID { get; set; }
        public string Character { get; private set; }
    }
}
