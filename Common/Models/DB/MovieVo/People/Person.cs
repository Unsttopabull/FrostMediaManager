using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Frost.Common.Util;

namespace Frost.Common.Models.DB.MovieVo.People {

    /// <summary>Represents a person that worked on a movie.</summary>
    public class Person {

        private string _thumb;

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        public Person() {
            MoviesAsDirector = new ObservableHashSet<Movie>();
            MoviesAsWriter = new ObservableHashSet<Movie>();
            MoviesLink = new ObservableHashSet<MovieActor>();
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
            Id = id;
        }

        /// <summary>Gets or sets the database person Id.</summary>
        /// <value>The database person Id</value>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

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
        public virtual ObservableHashSet<Movie> MoviesAsDirector { get; set; }

        /// <summary>Gets or sets a link to movies where this person was an actor.</summary>
        /// <value>A link to movies where this person was an actor.</value>
        public virtual ObservableHashSet<MovieActor> MoviesLink { get; set; }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        [InverseProperty("Writers")]
        public virtual ObservableHashSet<Movie> MoviesAsWriter { get; set; }

        /// <summary>Gets or sets movies where this person was an actor.</summary>
        /// <value>The movies where this person was an actor.</value>
        [NotMapped]
        public IEnumerable<Movie> MoviesAsActor {
            get { return MoviesLink.Select(ma => ma.Movie); }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name;
        }

        internal class Configuration : EntityTypeConfiguration<Person> {

            public Configuration() {
                ToTable("People");

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
