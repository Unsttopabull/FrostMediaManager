using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Frost.Common.Models.DB.MovieVo.People {

    /// <summary>Represents a person that worked on a movie.</summary>
    public class Person : IEquatable<Person> {

        private string _thumb;

        /// <summary>Initializes a new instance of the <see cref="Person"/> class.</summary>
        public Person() {
            MoviesAsDirector = new HashSet<Movie>();
            MoviesAsWriter = new HashSet<Movie>();
            MoviesLink = new HashSet<MovieActor>();
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
        public long Id { get; set; }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
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

        /// <summary>Gets or sets movies where this person was a director.</summary>
        /// <value>The movies where this person was a director.</value>
        [InverseProperty("Directors")]
        public virtual HashSet<Movie> MoviesAsDirector { get; set; }

        /// <summary>Gets or sets a link to movies where this person was an actor.</summary>
        /// <value>A link to movies where this person was an actor.</value>
        public virtual HashSet<MovieActor> MoviesLink { get; set; }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        [InverseProperty("Writers")]
        public virtual HashSet<Movie> MoviesAsWriter { get; set; }

        /// <summary>Gets or sets movies where this person was an actor.</summary>
        /// <value>The movies where this person was an actor.</value>
        [NotMapped]
        public IEnumerable<Movie> MoviesAsActor {
            get { return MoviesLink.Select(ma => ma.Movie); }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Person other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Name == other.Name;
        }

        /// <summary>Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            Person person = obj as Person;
            return person != null && Equals(person);
        }

        /// <summary>Serves as a hash function for a particular type. </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
        public override int GetHashCode() {
            unchecked {
                return (Id.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name;
        }

        internal class Configuration : EntityTypeConfiguration<Person> {

            public Configuration() {
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

                //// Movie <--> Actor link
                //HasMany(p => p.MoviesAsActor)
                //.WithMany()
                //.Map(m => {
                //    m.ToTable("MovieWriters");
                //    m.MapLeftKey("WriterId");
                //    m.MapRightKey("MovieId");
                //});
            }

        }

    }

}