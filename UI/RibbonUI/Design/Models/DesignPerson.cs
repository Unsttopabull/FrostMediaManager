using Frost.Common.Models.Provider;
using Frost.InfoParsers.Models.Info;

namespace RibbonUI.Design.Models {
    public class DesignPerson : IPerson {

        /// <summary>Initializes a new instance of the <see cref="DesignPerson"/> class.</summary>
        public DesignPerson() {
        }

        /// <summary>Initializes a new instance of the <see cref="DesignPerson"/> class.</summary>
        public DesignPerson(string name, string thumb, string imdbID) {
            Name = name;
            Thumb = thumb;
            ImdbID = imdbID;
        }

        /// <summary>Initializes a new instance of the <see cref="DesignPerson"/> class.</summary>
        public DesignPerson(IParsedPerson person) {
            Name = person.Name;
            Thumb = person.Thumb;
            ImdbID = person.ImdbID;
        }

        public long Id { get; private set; }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Name":
                    case "Thumb":
                    case "ImdbID":
                        return true;
                    default:
                        return false;
                }
            }
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
