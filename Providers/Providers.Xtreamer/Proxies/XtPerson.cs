using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtPerson : IPerson {
        protected readonly XjbPhpPerson Person;
        private int _id = -1;


        public XtPerson(XjbPhpPerson person) {
            Person = person;
        }

        public long Id {
            get {
                if (_id == -1) {
                    return int.TryParse(Person.Id, out _id)
                        ? _id
                        : 0;
                }
                return _id;
            }
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        public string Name {
            get { return Person.Name; }
            set { Person.Name = value; } 
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        public string Thumb {
            get { return null; }
            set { } 
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        public string ImdbID {
            get { return null; }
            set { } 
        }

        public virtual bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Name":
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}
