using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Frost.Common.Models.DB.MovieVo.Arts {

    /// <summary>Represents a promotional movie art.</summary>
    public abstract class ArtBase {

        public ArtBase() {
        }

        /// <summary>Initializes a new instance of the <see cref="ArtBase"/> class with specified path.</summary>
        /// <param name="path">The path to this art (can be local or network or an URI).</param>
        /// <param name="preview">The path to a smaller version used as preview image</param>
        public ArtBase(string path, string preview = null) {
            if (!string.IsNullOrEmpty(preview)) {
                Preview = preview;
            }
            Path = path;
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

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the movie this art is for.</summary>
        /// <value>The movie this art is for</value>
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }


        [NotMapped]
        public string Type {
            get {
                Type type = GetType();
                if (type.Namespace == "System.Data.Entity.DynamicProxies") {
                    if (type.BaseType != null) {
                        return type.BaseType.Name;
                    }
                }
                return type.Name;
            }
        }

        [NotMapped]
        public string PreviewOrPath {
            get {
                if (!string.IsNullOrEmpty(Preview)) {
                    return Preview;
                }
                return Path;
            }
        }

        internal class Configuration : EntityTypeConfiguration<ArtBase> {

            public Configuration() {
                ToTable("Arts");

                Map<Art>(m => m.Requires("Type").HasValue((long) ArtType.Unknown));
                Map<Cover>(m => m.Requires("Type").HasValue((long) ArtType.Cover));
                Map<Poster>(m => m.Requires("Type").HasValue((long) ArtType.Poster));
                Map<Fanart>(m => m.Requires("Type").HasValue((long) ArtType.Fanart));

                HasRequired(a => a.Movie)
                    .WithMany(m => m.Arts)
                    .HasForeignKey(a => a.MovieId)
                    .WillCascadeOnDelete();
            }

        }

    }

}
