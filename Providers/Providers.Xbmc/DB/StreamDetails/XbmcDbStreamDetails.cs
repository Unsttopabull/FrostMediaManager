using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Providers.Xbmc.NFO;

namespace Frost.Providers.Xbmc.DB.StreamDetails {

    public enum StreamType : long {
        Video,
        Audio,
        Subtitle
    }

    /// <summary>Represents information about a stream in a file.</summary>
    [Table("streamdetails")]
    public class XbmcDbStreamDetails {

        /// <summary>Initializes a new instance of the <see cref="XbmcDbStreamDetails"/> class.</summary>
        public XbmcDbStreamDetails() {
        }

        public XbmcDbStreamDetails(StreamType type) {
            Type = type;
        }

        /// <summary>Gets or sets this stream's database Id.</summary>
        /// <value>This stream's database Id.</value>
        [Column("idStream")]
        public long Id { get; set; }

        [Column("iStreamType")]
        public StreamType Type { get; set; }

        #region Audio

        /// <summary>Gets or sets the codec this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        [Column("strAudioCodec")]
        public string AudioCodec { get; set; }

        /// <summary>Gets or sets the number of audio channels.</summary>
        /// <value>The number of audio channels.</value>
        [Column("iAudioChannels")]
        public long? AudioChannels { get; set; }

        /// <summary>Gets or sets the language of this audio.</summary>
        /// <value>The language of this audio.</value>
        [Column("strAudioLanguage")]
        public string AudioLanguage { get; set; }

        #endregion

        #region Video
        /// <summary>Gets or sets the video codec.</summary>
        /// <value>The video codec.</value>
        /// <example>\eg{ <c>XVID, DIVX, MPEG4</c>}</example>
        [Column("strVideoCodec")]
        public string VideoCodec { get; set; }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <example>\eg{ <c>1.333</c>}</example>
        [Column("fVideoAspect")]
        public double? Aspect { get; set; }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        [Column("iVideoWidth")]
        public long? VideoWidth { get; set; }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        [Column("iVideoHeight")]
        public long? VideoHeight { get; set; }

        /// <summary>Gets or sets the duration of the video in seconds.</summary>
        /// <value>The duration of the video in seconds.</value>
        [Column("iVideoDuration")]
        public long? VideoDuration { get; set; }

        #endregion

        /// <summary>Gets or sets the language of this subtitle.</summary>
        /// <value>The language of this subtitle.</value>
        [Column("strSubtitleLanguage")]
        public string SubtitleLanguage { get; set; }


        [Column("idFile")]
        public long? FileId { get; set; }

        /// <summary>Gets or sets the file this stream is contained in.</summary>
        /// <value>The file this stream is contained in.</value>
        [ForeignKey("FileId")]
        public virtual XbmcFile File { get; set; }

        internal class Configuration : EntityTypeConfiguration<XbmcDbStreamDetails> {

            /// <summary>Initializes a new instance of the <see cref="Configuration"/> class.</summary>
            public Configuration() {
                HasKey(sd => sd.Id);

                HasRequired(sd => sd.File)
                    .WithMany(f => f.StreamDetails)
                    .HasForeignKey(sd => sd.FileId);
            }

        }

    }

}
