using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Frost.Providers.Xbmc.DB.StreamDetails {

    /// <summary>Represents information about a stream in a file.</summary>
    [Table("streamdetails")]
    public abstract class XbmcDbStreamDetails {

        /// <summary>Initializes a new instance of the <see cref="XbmcDbStreamDetails"/> class.</summary>
        public XbmcDbStreamDetails() {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcDbStreamDetails"/> class.</summary>
        /// <param name="file">The file that contains this stream.</param>
        public XbmcDbStreamDetails(XbmcFile file) {
            File = file;
        }

        /// <summary>Gets or sets this stream's database Id.</summary>
        /// <value>This stream's database Id.</value>
        [Column("idStream")]
        public long Id { get; set; }

        [Column("idFile")]
        public long? FileId { get; set; }

        /// <summary>Gets or sets the file this stream is contained in.</summary>
        /// <value>The file this stream is contained in.</value>
        //[ForeignKey("FileId")]
        public virtual XbmcFile File { get; set; }

        internal class Configuration : EntityTypeConfiguration<XbmcDbStreamDetails> {

            /// <summary>Initializes a new instance of the <see cref="Configuration"/> class.</summary>
            public Configuration() {
                Map<XbmcVideoDetails>(s => s.Requires("iStreamType").HasValue(0));
                Map<XbmcAudioDetails>(s => s.Requires("iStreamType").HasValue(1));
                Map<XbmcSubtitleDetails>(s => s.Requires("iStreamType").HasValue(2));

                HasRequired(sd => sd.File).
                    WithMany(f => f.StreamDetails).
                    HasForeignKey(sd => sd.FileId);

                //.Map(m => m.MapKey("idFile"));
            }

        }

    }

}
