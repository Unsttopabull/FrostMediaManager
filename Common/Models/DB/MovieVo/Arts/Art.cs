using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.MovieVo.Arts {

    /// <summary>Represents a promotional movie art.</summary>
    public class Art : IEquatable<Art> {

        /// <summary>Initializes a new instance of the <see cref="Art"/> class with specified path.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        public Art(string path) {
            Path = path;

            Movie = new Movie();
        }

        /// <summary>Initializes a new instance of the <see cref="Art"/> class with specified path and type.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="type">The type used as a discriminator.</param>
        public Art(string path, ArtType type) : this(path) {
            Type = type;
        }

        /// <summary>Gets or sets the database Art Id.</summary>
        /// <value>The database art Id</value>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        public string Path { get; set; }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        public string Preview { get; set; }

        /// <summary>Gets or sets the type used as a discriminator.</summary>
        /// <value>The type used as a discriminator.</value>
        protected ArtType Type { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the movie this art is for.</summary>
        /// <value>The movie this art is for</value>
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Art other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Path == other.Path &&
                   Preview == other.Preview &&
                   Type == other.Type &&
                   MovieId == other.MovieId;
        }

        internal class Configuration : EntityTypeConfiguration<Art> {

            public Configuration() {
                Map<Cover>(m => m.Requires("Type").HasValue(1));
                Map<Poster>(m => m.Requires("Type").HasValue(2));
                Map<Fanart>(m => m.Requires("Type").HasValue(3));
            }

        }

    }

}
