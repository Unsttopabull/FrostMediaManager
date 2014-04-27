using Frost.InfoParsers.Models;

namespace Frost.InfoParsers {
    public class ParsedPerson : IParsedPerson {

        /// <summary>Initializes a new instance of the <see cref="ParsedPerson"/> class.</summary>
        public ParsedPerson(string name) : this(name, null){
            
        }

        /// <summary>Initializes a new instance of the <see cref="ParsedPerson"/> class.</summary>
        public ParsedPerson(string name, string imdbId) : this(name, null, imdbId){
            
        }

        /// <summary>Initializes a new instance of the <see cref="ParsedPerson"/> class.</summary>
        public ParsedPerson(string name, string imdbID, string thumb) {
            Name = name;
            Thumb = thumb;
            ImdbID = imdbID;
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
    }
}
