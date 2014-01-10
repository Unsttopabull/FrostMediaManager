using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Frost.Common.Models.DB.MovieVo.Arts {

    /// <summary>Represents a promotional movie art.</summary>
    public class Art : IEquatable<Art> {

        public Art() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="Art"/> class with specified path.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        public Art(string path, string preview = null) {
            Type = ArtType.Unknown;
            Preview = preview;
            Path = path;
        }

        /// <summary>Initializes a new instance of the <see cref="Art"/> class with specified path and type.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        /// <param name="type">The type used as a discriminator.</param>
        public Art(string path, string preview, ArtType type) : this(path, preview) {
            Type = type;
        }

        /// <summary>Gets or sets the database Art Id.</summary>
        /// <value>The database art Id</value>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        [Required]
        public string Path { get; set; }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        public string Preview { get; set; }

        /// <summary>Gets or sets the type used as a discriminator.</summary>
        /// <value>The type used as a discriminator.</value>
        [Required]
        protected ArtType Type { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the movie this art is for.</summary>
        /// <value>The movie this art is for</value>
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Art other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Id == other.Id && string.Equals(Path, other.Path) && Type == other.Type && MovieId == other.MovieId && string.Equals(Preview, other.Preview);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != this.GetType()) {
                return false;
            }
            return Equals((Art) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode() {
            unchecked {
                int hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Path != null ? Path.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Type.GetHashCode();
                hashCode = (hashCode * 397) ^ MovieId.GetHashCode();
                hashCode = (hashCode * 397) ^ (Preview != null ? Preview.GetHashCode() : 0);
                return hashCode;
            }
        }

        internal class Configuration : EntityTypeConfiguration<Art> {

            public Configuration() {
                ToTable("Arts");
                Map<Cover>(m => m.Requires("Type").HasValue((int)ArtType.Cover));
                Map<Poster>(m => m.Requires("Type").HasValue((int)ArtType.Poster));
                Map<Fanart>(m => m.Requires("Type").HasValue((int)ArtType.Fanart));

                HasRequired(a => a.Movie)
                    .WithMany(m => m.Arts)
                    .HasForeignKey(a => a.MovieId)
                    .WillCascadeOnDelete();
            }

        }

    }

}
