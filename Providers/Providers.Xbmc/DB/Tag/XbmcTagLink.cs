using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Providers.Xbmc.DB.Tag {

    /// <summary>Represents a table that links a tag to a Movie/Episode/TvShow ...</summary>
    [Table("taglinks")]
    public class XbmcTagLink {

        /// <summary>Gets or sets the Id of the referenced Tag in the database.</summary>
        /// <value>The Id of the referenced Tag in the database.</value>
        [Key]
        [Column("idTag", Order = 0)]
        [ForeignKey("Tag")]
        public long TagId { get; set; }

        /// <summary>Gets or sets the Id of the referenced Media (Movie/Episode/TvShow ...) in the database.</summary>
        /// <value>The Id of the referenced Media (Movie/Episode/TvShow ...) in the database.</value>
        [Key]
        [Column("idMedia", Order = 1)]
        public long MediaId { get; set; }

        /// <summary>Gets or sets the type of the media referenced by <see cref="XbmcTagLink.MediaId"/> used as a dicriminator.</summary>
        /// <value>The type of the media referenced by <see cref="XbmcTagLink.MediaId"/> used as a dicriminator</value>
        /// <example>\eg{<c>"movie"</c>}</example>
        [Column("media_type")]
        public string MediaType { get; set; }

        /// <summary>Gets or sets the referenced tag.</summary>
        /// <value> The tag referenced.</value>
        public virtual XbmcTag Tag { get; set; }

    }

}
