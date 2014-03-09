using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models;

namespace Frost.Models.Frost.DB.People {

    /// <summary>Represents a person that worked on a movie.</summary>
    public class Person : IPerson {
        private string _thumb;

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        public Person() {
            MoviesAsDirector = new HashSet<Movie>();
            MoviesAsWriter = new HashSet<Movie>();
        }

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        public Person(string name) : this() {
            Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        /// <param name="thumb">The thumbnail image.</param>
        public Person(string name, string thumb) : this(name) {
            if (!string.IsNullOrEmpty(thumb)) {
                _thumb = thumb;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        /// <param name="id">The Id of the person in the database</param>
        /// <param name="name">The full name of the actor.</param>
        /// <param name="thumb">The thumbnail image.</param>
        internal Person(long id, string name, string thumb) : this(name, thumb) {
            PersonId = id;
        }

        public Person(IPerson person) {
            Name = person.Name;
            Thumb = person.Thumb;
            ImdbID = person.Name;
        }

        /// <summary>Gets or sets the database person Id.</summary>
        /// <value>The database person Id</value>
        public long PersonId { get; set; }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        [Required]
        public string Name { get; set; }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        public string Thumb {
            get {
                return string.IsNullOrEmpty(_thumb)
                    ? null
                    : _thumb;
            }
            set { _thumb = value; }
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        public string ImdbID { get; set; }

        /// <summary>Gets or sets movies where this person was a director.</summary>
        /// <value>The movies where this person was a director.</value>
        [InverseProperty("Directors")]
        public virtual ICollection<Movie> MoviesAsDirector { get; set; }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        [InverseProperty("Writers")]
        public virtual ICollection<Movie> MoviesAsWriter { get; set; }

        #region IPerson

        /// <summary>Gets or sets movies where this person was a director.</summary>
        /// <value>The movies where this person was a director.</value>
        ICollection<IMovie> IPerson.MoviesAsDirector {
            get { return null; }
        }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        ICollection<IMovie> IPerson.MoviesAsWriter {
            get { return null; }
        }

        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name;
        }

        internal class Configuration : EntityTypeConfiguration<Person> {

            public Configuration() {
                ToTable("People");

                HasKey(p => p.PersonId);

                // Movie <--> Director link
                HasMany(p => p.MoviesAsDirector)
                    .WithMany(m => m.Directors)
                    .Map(m => {
                        m.ToTable("MovieDirectors");
                        m.MapLeftKey("DirectorId");
                        m.MapRightKey("MovieId");
                    });

                // Movie <--> Writer link
                HasMany(p => p.MoviesAsWriter)
                    .WithMany(m => m.Writers)
                    .Map(m => {
                        m.ToTable("MovieWriters");
                        m.MapLeftKey("WriterId");
                        m.MapRightKey("MovieId");
                    });
            }

        }

    }

}
