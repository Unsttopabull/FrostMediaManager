using Frost.Common.Models.Provider;

namespace RibbonUI.Design.Models {
    public class DesignPerson : IPerson {

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
