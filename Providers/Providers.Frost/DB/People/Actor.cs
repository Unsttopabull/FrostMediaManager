using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Frost.DB {

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
                try {
                    return Person.Id;
                }
                catch {
                    return 0;
                }
            }
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        string IPerson.Name {
            get {
                try {
                    return Person.Name;
                }
                catch {
                    return null;
                }
            }
            set {
                try {
                    Person.Name = value;
                }
                catch {
                }
            }
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        string IPerson.Thumb {
            get {
                try {
                    return Person.Thumb;
                }
                catch {
                    return null;
                }
            }
            set {
                try {
                    Person.Thumb = value;
                }
                catch {
                }
            }
        }

        string IPerson.ImdbID {
            get {
                try {
                    return Person.ImdbID;
                }
                catch {
                    return null;
                }
            }
            set {
                try {
                    Person.ImdbID = value;
                }
                catch {
                }
            }
        }

        #endregion

        internal class Configuration : EntityTypeConfiguration<Actor> {
            public Configuration() {
                ToTable("Actors");
                HasKey(a => a.Id);

                //1:M with Person
                HasRequired(a => a.Person)
                    .WithMany(p => p.MoviesAsActor)
                    .HasForeignKey(a => a.PersonId);
            
                //1:M with Movie
                HasRequired(a => a.Movie)
                    .WithMany(m => m.Actors)
                    .HasForeignKey(a => a.MovieId);
            }
        }
    }
}
