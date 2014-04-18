using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Frost.DB {

    /// <summary>Represents a promotional movie art.</summary>
    public class Art : IArt {

        public Art() {
        }

        /// <summary>Initializes a new instance of the <see cref="Art"/> class with specified path.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        public Art(string path, string preview = null) {
            if (!string.IsNullOrEmpty(preview)) {
                Preview = preview;
            }
            Path = path;
        }

        /// <summary>Initializes a new instance of the <see cref="Art"/> class with specified path.</summary>
        /// <param name="type"></param>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        public Art(ArtType type, string path, string preview = null) : this(path, preview){
            Type = type;
        }

        internal Art(IArt art) {
            
        }

        /// <summary>Gets or sets the database Art Id.</summary>
        /// <value>The database art Id</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public ArtType Type { get; set; }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        [Required]
        public string Path { get; set; }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        public string Preview { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the movie this art is for.</summary>
        /// <value>The movie this art is for</value>
        //[ForeignKey("MovieId")]
        [InverseProperty("Art")]
        public virtual Movie Movie { get; set; }

        [NotMapped]
        public string PreviewOrPath {
            get {
                if (!string.IsNullOrEmpty(Preview)) {
                    return Preview;
                }
                return Path;
            }
        }

        bool IMovieEntity.this[string propertyName] {
            get { return true; }
        }

        internal class Configuration : EntityTypeConfiguration<Art> {

            public Configuration() {
                ToTable("Art");

                HasRequired(p => p.Movie)
                    .WithMany(m => m.Art)
                    .HasForeignKey(p => p.MovieId)
                    .WillCascadeOnDelete();
            }

        }

    }

}
