using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Xtreamer.DB {

    /// <summary>Represents a preson that participated in a movie.</summary>
    [Table("persons")]
    public class XjbPerson : IPerson {

        public XjbPerson() {
            Movies = new HashSet<XjbMoviePerson>();
        }

        /// <summary>Initializes a new instance of the <see cref="XjbPerson"/> class.</summary>
        /// <param name="name">The full name of the person.</param>
        public XjbPerson(string name) {
            Name = name;
        }

        /// <summary>Gets or sets the Id of this person in the database.</summary>
        /// <value>The Id of this person in the database</value>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>Gets or sets the full name of the actor.</summary>
        /// <value>The full name of the person.</value>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the link to the movies that this person worked on.</summary>
        /// <value>The link to the movies that this person worked on</value>
        public virtual HashSet<XjbMoviePerson> Movies { get; set; }

        bool IMovieEntity.this[string propertyName] {
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

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        string IPerson.ImdbID {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        string IPerson.Thumb {
            get { return null; }
            set { }
        }
    }

}
