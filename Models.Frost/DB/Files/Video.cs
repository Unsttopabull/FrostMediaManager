using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common;
using Frost.Common.Models;
using Frost.Model.Xbmc.NFO;

namespace Frost.Models.Frost.DB.Files {

    /// <summary>Represents information about a video stream in a file.</summary>
    public class Video : IVideo {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Video"/> class.</summary>
        public Video() {
        }

        /// <summary>Initializes a new instance of the <see cref="Video"/> class.</summary>
        /// <param name="file">The file.</param>
        public Video(File file) {
            File = file;
        }

        public Video(IVideo video) {
            MovieHash = video.MovieHash;
            Source = video.Source;
            Type = video.Type;
            Resolution = video.Resolution;
            ResolutionName = video.ResolutionName;
            Standard = video.Standard;
            FPS = video.FPS;
            BitRate = video.BitRate;
            BitRateMode = video.BitRateMode;
            BitDepth = video.BitDepth;
            CompressionMode = video.CompressionMode;
            Duration = video.Duration;
            ScanType = video.ScanType;
            ColorSpace = video.ColorSpace;
            ChromaSubsampling = video.ChromaSubsampling;
            Format = video.Format;
            Codec = video.Codec;
            CodecId = video.CodecId;
            Aspect = video.Aspect;
            AspectCommercialName = video.AspectCommercialName;
            Width = video.Width;
            Height = video.Height;

            if (video.Language != null) {
                Language = new Language(video.Language);
            }

            if (video.File != null) {
                File = new File(video.File);
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Video"/> class.</summary>
        /// <param name="codec">The codec this video is encoded in.</param>
        /// <param name="width">The width of the video.</param>
        /// <param name="height">The height of the video.</param>
        public Video(string codec, int? width, int? height) : this() {
            Codec = codec;

            if (height.HasValue && height.Value > 0) {
                Height = height;
            }

            if (width.HasValue && width.Value> 0) {
                Width = width;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Video"/> class.</summary>
        /// <param name="codec">The codec this video is encoded in.</param>
        /// <param name="width">The width of the video.</param>
        /// <param name="height">The height of the video.</param>
        /// <param name="aspect">The aspect ratio of the video (width / height)</param>
        /// <param name="fps">The Frames per second of this video</param>
        /// <param name="resolution">Resolution and format of the video</param>
        /// <param name="type">The type of the video (DVD, BR, XVID, ...)</param>
        /// <param name="source">With or from what this video was made from (CAM, TS, DVDRip ...)</param>
        public Video(string codec, int width, int height, double aspect, float? fps, string resolution, string type, string source) : this(codec, width, height) {
            Resolution = GetVideoResolution(resolution);
            Type = type;
            Source = source;
            FPS = fps;

            if (aspect > 0) {
                Aspect = aspect;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Video"/> class.</summary>
        /// <param name="codec">The codec this video is encoded in.</param>
        /// <param name="width">The width of the video.</param>
        /// <param name="height">The height of the video.</param>
        /// <param name="aspect">The aspect ratio of the video (width / height)</param>
        public Video(string codec, int width, int height, double aspect) : this(codec, width, height) {
            if (aspect > 0) {
                Aspect = aspect;
            }
        }

        #endregion

        #region Properties/Columns

        /// <summary>Gets or sets the database video Id.</summary>
        /// <value>The database video Id</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string MovieHash { get; set; }

        /// <summary>With or from what this video was made from</summary>
        /// <example>\eg{ <c>TS, TC, TELESYNC, CAM, HDRIP, DVDRIP, BDRIP, DTV, HD2DVD, HDDVDRIP, HDTVRIP, VHS, SCREENER, RECODE</c>}</example>
        public string Source { get; set; }

        /// <summary>The type of the video</summary>
        /// <example>\eg{ <c>XVID, DVD5, DVD9, DVDR, BLUERAY, BD, HD2DVD, X264</c>}</example>
        public string Type { get; set; }

        /// <summary>Resolution and format of the video</summary>
        /// <example>\eg{ <c>720p, 1080p, 720i, 1080i, PAL, HDTV, NTSC</c>}</example>
        public int? Resolution { get; set; }

        /// <summary>Gets or sets the name of the resolution.</summary>
        /// <value>The name of the resolution.</value>
        public string ResolutionName { get; set; }

        public string Standard { get; set; }

        /// <summary>Gets or sets the frames per second in this video.</summary>
        /// <value>The Frames per second.</value>
        public float? FPS { get; set; }

        /// <summary>Gets or sets the video bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public float? BitRate { get; set; }

        /// <summary>Gets or sets the video bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        public FrameOrBitRateMode BitRateMode { get; set; }

        /// <summary>Gets or sets the video bit depth.</summary>
        /// <value>The video depth in bits.</value>
        public long? BitDepth { get; set; }

        /// <summary>Gets or sets the compression mode of this video.</summary>
        /// <value>The compression mode of this video.</value>
        public CompressionMode CompressionMode { get; set; }

        /// <summary>Gets or sets the video duration in miliseconds.</summary>
        /// <value>The video duration in miliseconds.</value>
        public long? Duration { get; set; }

        /// <summary>Gets or sets the video scan type.</summary>
        /// <value>The type video scan type.</value>
        public ScanType ScanType { get; set; }

        /// <summary>Gets or sets the color space.</summary>
        /// <value>The video color space.</value>
        /// <example>\eg{ <c>YUV, YDbDr, YPbPr, YCbCr, RGB, CYMK</c>}</example>
        public string ColorSpace { get; set; }

        /// <summary>Gets or sets the type of chroma subsampling.</summary>
        /// <value>The chroma subsampling.</value>
        public string ChromaSubsampling { get; set; }

        public string Format { get; set; }

        /// <summary>Gets or sets the codec this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>WMV3 DIVX XVID H264 VP6 AVC</c>}</example>
        public string Codec { get; set; }

        /// <summary>Gets or sets the codec tag this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>x265, div3, dx50, mpeg2v</c>}</example>
        public string CodecId { get; set; }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <value>Aspect ratio of the video</value>
        /// <example>\eg{ <c>1.333</c>}</example>
        public double? Aspect { get; set; }

        /// <summary>Gets or sets the the commercial name of the aspect ratio.</summary>
        /// <value>The the commercial name of the aspect ratio.</value>
        public string AspectCommercialName { get; set; }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        public int? Width { get; set; }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        public int? Height { get; set; }

        #endregion

        #region Foreign Keys

        /// <summary>Gets or sets the language foreign key.</summary>
        /// <value>The language foreign key.</value>
        public long? LanguageId { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the file foreign key.</summary>
        /// <value>The file foreign key.</value>
        public long FileId { get; set; }

        #endregion

        #region Relation tables

        /// <summary>Gets or sets the language of this video.</summary>
        /// <value>The language of this video.</value>
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }

        /// <summary>Gets or sets the language of this video.</summary>
        /// <value>The language of this video.</value>
        ILanguage IHasLanguage.Language {
            get { return Language; }
            set {
                if (value == null) {
                    Language = null;
                    return;
                }
                Language = new Language(value);
            }
        }

        /// <summary>Gets or sets the file this video is contained in.</summary>
        /// <value>The file this video is contained in.</value>
        public virtual File File { get; set; }

        /// <summary>Gets or sets the file this video is contained in.</summary>
        /// <value>The file this video is contained in.</value>
        IFile IVideo.File {
            get { return File; }
            set {
                if (value == null) {
                    File = null;
                    return;
                }
                File = new File(value);
            }
        }

        /// <summary>Gets or sets the movie this video is from.</summary>
        /// <value>The movie this video is from.</value>
        public virtual Movie Movie { get; set; }

        #endregion

        private int? GetVideoResolution(string resolution) {
            if (!string.IsNullOrEmpty(resolution)) {
                int res;
                string vres = resolution.TrimEnd('p', 'i');

                if (!int.TryParse(vres, out res)) {
                    if (resolution.Equals("NTSC", StringComparison.OrdinalIgnoreCase)) {
                        res = 480;
                        ScanType = ScanType.Interlaced;
                    }

                    if (resolution.Equals("PAL", StringComparison.OrdinalIgnoreCase)) {
                        res = 576;
                        ScanType = ScanType.Interlaced;
                    }
                }
                return res;
            }
            return null;
        }

        /// <summary>Converts an instance of <see cref="XbmcXmlVideoInfo"/> to an instance of <see cref="Video">Video</see>.</summary>
        /// <param name="video">The video instance to convert.</param>
        /// <returns>An instance of <see cref="Video">Video</see> converted from <see cref="XbmcXmlVideoInfo"/></returns>
        public static explicit operator Video(XbmcXmlVideoInfo video) {
            return new Video(video.Codec, video.Width, video.Height, video.Aspect);
        }

        internal class Configuration : EntityTypeConfiguration<Video> {
            public Configuration() {
                ToTable("Videos");

                HasRequired(v => v.Movie)
                    .WithMany(m => m.Videos)
                    .HasForeignKey(v => v.MovieId)
                    .WillCascadeOnDelete();

                HasRequired(v => v.File)
                    .WithMany(f => f.VideoDetails)
                    .HasForeignKey(v => v.FileId)
                    .WillCascadeOnDelete();

                HasOptional(v => v.Language);
            }
        }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() {
            return MemberwiseClone();
        }
    }

}