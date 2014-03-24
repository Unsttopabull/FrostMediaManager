using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Frost.DB.People {

    public class Actor : IActor {
        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        public Actor() {
        }

        public Actor(Person person, string character) {
            Person = person;
            Character = character;
        }

        public long Id { get; set; }

        public string Character { get; set; }

        [Required]
        public virtual Movie Movie { get; set; }

        [ForeignKey("Movie")]
        public long MovieId { get; set; }


        [Required]
        public virtual Person Person { get; set; }

        [ForeignKey("Person")]
        public long PersonId { get; set; }

        #region IActor

        bool IMovieEntity.this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Name":
                    case "Thumb":
                    case "ImdbID":
                    case "Character":
                        return true;
                    default:
                        return false;
                }
            }
        }

        long IMovieEntity.Id {
            get {
                if (Person == null) {
                    Person = new Person();
                }

                return Person.Id;
            }
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        string IPerson.Name {
            get {
                if (Person == null) {
                    Person = new Person();
                }

                return Person.Name;
            }
            set {
                if (Person == null) {
                    Person = new Person();
                }

                Person.Name = value;
            }
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        string IPerson.Thumb {
            get {
                if (Person == null) {
                    Person = new Person();
                }
                return Person.Thumb;
            }
            set {
                if (Person == null) {
                    Person = new Person();
                }
                Person.Thumb = value;
            }
        }

        string IPerson.ImdbID {
            get {
                if (Person == null) {
                    Person = new Person();
                }
                return Person.ImdbID;
            }
            set {
                if (Person == null) {
                    Person = new Person();
                }
                Person.ImdbID = value;
            }
        }

        #endregion

    }
}
