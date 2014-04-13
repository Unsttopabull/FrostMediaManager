using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Xbmc.DB.People {

    /// <summary>Represents a link table in XBMC database between a movie and a person containing the name of the persons charater.</summary>
    [Table("actorlinkmovie")]
    public class XbmcMovieActor : IActor {

        /// <summary>Initializes a new instance of the <see cref="XbmcMovieActor"/> class.</summary>
        public XbmcMovieActor() {
        }

        public XbmcMovieActor(XbmcPerson actor, string character, long order) {
            Person = actor;
            Role = character;
            Order = order;
        }

        /// <summary>Gets or sets the foreign key to the person.</summary>
        /// <value>The foreign key to the person</value>
        [Column("idActor", Order = 0)]
        public long PersonId { get; set; }

        /// <summary>Gets or sets the foreign key to the movie.</summary>
        /// <value>The foreign key to the movie</value>
        [Column("idMovie", Order = 1)]
        public long MovieId { get; set; }

        /// <summary>Gets or sets the role or character the actor is portraying in this movie.</summary>
        /// <value>The role or character the actor is portraying in this movie.</value>
        [Column("strRole")]
        public string Role { get; set; }

        /// <summary>Gets or sets the position in a list this actor should appear at.</summary>
        /// <value>The position in a list this actor should appear at.</value>
        [Column("iOrder")]
        public long Order { get; set; }

        /// <summary>Gets or sets the movie where the linked person is portraying that character.</summary>
        /// <value>The movie where the linked person is portraying that character.</value>
        public virtual XbmcDbMovie Movie { get; set; }

        /// <summary>Gets or sets the person that is portraying that character in the linked movie</summary>
        /// <value>The person that is portraying that character in the linked movie</value>
        public virtual XbmcPerson Person { get; set; }

        #region IActor

        long IMovieEntity.Id {
            get { return default(long); }
        }

        bool IMovieEntity.this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Name":
                    case "Thumb":
                    case "Character":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        string IPerson.Name {
            get {
                if (Person == null) {
                    Person = new XbmcPerson();
                }

                return Person.Name;
            }
            set {
                if (Person == null) {
                    Person = new XbmcPerson();
                }
                Person.Name = value;
            }
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        string IPerson.Thumb {
            get {
                if (Person == null) {
                    Person = new XbmcPerson();
                }

                string t = Person.ThumbURL;
                return !string.IsNullOrEmpty(t) ? t : null;
            }
            set {
                if (Person == null) {
                    Person = new XbmcPerson();
                }

                Person.ThumbURL = value;
            }
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        string IPerson.ImdbID {
            get { return default(string); }
            set { }
        }

        string IActor.Character {
            get { return Role; }
            set { Role = value; }
        }

        #endregion

        internal class Configuration : EntityTypeConfiguration<XbmcMovieActor> {

            public Configuration() {
                //1:M with Person
                HasRequired(mp => mp.Person)
                    .WithMany(mp => mp.MoviesAsActor)
                    .HasForeignKey(ma => ma.PersonId);

                //1:M with Movie
                HasRequired(mp => mp.Movie)
                    .WithMany(m => m.Actors)
                    .HasForeignKey(m => m.MovieId);

                //composite primary key
                HasKey(ma => new { ma.PersonId, ma.MovieId });
            }

        }
    }

}
