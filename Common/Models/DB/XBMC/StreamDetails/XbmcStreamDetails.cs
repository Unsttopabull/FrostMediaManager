using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Common.Models.DB.XBMC.StreamDetails {

    /// <summary>Represents information about a stream in a file.</summary>
    [Table("streamdetails")]
    public abstract class XbmcStreamDetails {

        /// <summary>Initializes a new instance of the <see cref="XbmcStreamDetails"/> class.</summary>
        public XbmcStreamDetails() {
            File = new XbmcFile();
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcStreamDetails"/> class.</summary>
        /// <param name="file">The file that contains this stream.</param>
        public XbmcStreamDetails(XbmcFile file) {
            File = file;
        }

        /// <summary>Gets or sets this stream's database Id.</summary>
        /// <value>This stream's database Id.</value>
        [Column("idStream")]
        public long Id { get; set; }

        /// <summary>Gets or sets the file this stream is contained in.</summary>
        /// <value>The file this stream is contained in.</value>
        public virtual XbmcFile File { get; set; }

        internal class Configuration : EntityTypeConfiguration<XbmcStreamDetails> {

            /// <summary>Initializes a new instance of the <see cref="Configuration"/> class.</summary>
            public Configuration() {
                Map<XbmcVideoDetails>(s => s.Requires("iStreamType").HasValue(0))
                    .Map<XbmcAudioDetails>(s => s.Requires("iStreamType").HasValue(1))
                    .Map<XbmcSubtitleDetails>(s => s.Requires("iStreamType").HasValue(2))
                    .HasRequired(sd => sd.File)
                    .WithMany(f => f.StreamDetails)
                    .Map(m => m.MapKey("idFile"));
            }

        }

    }

}
