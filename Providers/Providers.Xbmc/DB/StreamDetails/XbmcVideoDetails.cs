using System;
using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Xbmc.DB.StreamDetails {

    /// <summary>Represents information about a video stream in a file.</summary>
    public class XbmcVideoDetails : XbmcDbStreamDetails, IVideo {
        public XbmcVideoDetails() {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcVideoDetails"/> class.</summary>
        /// <param name="codec">The video codec.</param>
        /// <param name="aspect">Aspect ratio. The ratio between width and height (width / height)</param>
        /// <param name="width">The width of the video.</param>
        /// <param name="height">The height of the video.</param>
        /// <param name="duration">The duration of the video in seconds.</param>
        public XbmcVideoDetails(string codec, double? aspect, long? width, long? height, long? duration) : this(new XbmcFile(), codec, aspect, width, height, duration) {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcVideoDetails"/> class.</summary>
        /// <param name="file">The file that contains this video stream.</param>
        /// <param name="codec">The video codec.</param>
        /// <param name="aspect">Aspect ratio. The ratio between width and height (width / height)</param>
        /// <param name="width">The width of the video.</param>
        /// <param name="height">The height of the video.</param>
        /// <param name="duration">The duration of the video in seconds.</param>
        public XbmcVideoDetails(XbmcFile file, string codec, double? aspect, long? width, long? height, long? duration) : base(file) {
            Codec = codec;
            Aspect = aspect;
            Width = width;
            Height = height;
            Duration = duration;
        }

        internal XbmcVideoDetails(IVideo video) {
            Id = video.Id;
            Codec = video.CodecId;
            Aspect = video.Aspect;
            Width = video.Width;
            Height = video.Height;

            //convert ms to seconds
            Duration = video.Duration / 1000;
        }

        /// <summary>Gets or sets the video codec.</summary>
        /// <value>The video codec.</value>
        /// <example>\eg{ <c>XVID, DIVX, MPEG4</c>}</example>
        [Column("strVideoCodec")]
        public string Codec { get; set; }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <example>\eg{ <c>1.333</c>}</example>
        [Column("fVideoAspect")]
        public double? Aspect { get; set; }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        [Column("iVideoWidth")]
        public long? Width { get; set; }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        [Column("iVideoHeight")]
        public long? Height { get; set; }

        /// <summary>Gets or sets the duration of the video in seconds.</summary>
        /// <value>The duration of the video in seconds.</value>
        [Column("iVideoDuration")]
        public long? Duration { get; set; }

        #region IVideo

        bool IMovieEntity.this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Codec":
                    case "Aspect":
                    case "Width":
                    case "Height":
                    case "Duration":
                        return true;
                    default:
                        return false;
                }
            }
        }

        long? IVideo.Duration {
            get { return Duration * 1000; }
            set {
                if (value.HasValue) {
                    Duration = value / 1000;
                }
                Duration = null;
            }
        }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        int? IVideo.Width {
            get { return (int?) Width; }
            set { Width = value; }
        }

        /// <summary>Gets or sets the color space.</summary>
        /// <value>The video color space.</value>
        /// <example>\eg{ <c>YUV, YDbDr, YPbPr, YCbCr, RGB, CYMK</c>}</example>
        string IVideo.ColorSpace {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        int? IVideo.Height {
            get { return (int?) Height; }
            set { Height = value; }
        }

        /// <summary>Gets or sets the codec tag this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>x265, div3, dx50, mpeg2v</c>}</example>
        string IVideo.CodecId {
            get { return Codec; }
            set { Codec = value; }
        }

        /// <summary>Gets or sets the file this video is contained in.</summary>
        /// <value>The file this video is contained in.</value>
        IFile IVideo.File {
            get { return File; }
            //set { File = new XbmcFile(value); }
        }

        /// <summary>Resolution and format of the video</summary>
        /// <example>\eg{ <c>720p, 1080p, 720i, 1080i, PAL, HDTV, NTSC</c>}</example>
        int? IVideo.Resolution {
            get {
                long h = Height ?? 0;
                long w = Width ?? 0;

                int resolution = 0;
                if (h == 1080 && w == 1920) {
                    resolution = 1080;
                }
                if (h == 720 && w == 1280) {
                    resolution = 720;
                }

                if (h == 480 && w == 720) {
                    resolution = 480;
                }

                if (h == 576 && w == 720) {
                    resolution = 576;
                }

                if (resolution == 0) {
                    return null;
                }
                return resolution;
            }
            set { }
        }

        #region Not Implemented

        /// <summary>Gets or sets the name of the resolution.</summary>
        /// <value>The name of the resolution.</value>
        string IVideo.ResolutionName {
            get { return default(string); }
            set { }
        }

        ILanguage IHasLanguage.Language {
            get { return default(ILanguage); }
            set { }
        }

        /// <summary>Gets or sets the the commercial name of the aspect ratio.</summary>
        /// <value>The the commercial name of the aspect ratio.</value>
        string IVideo.AspectCommercialName {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the video bit depth.</summary>
        /// <value>The video depth in bits.</value>
        long? IVideo.BitDepth {
            get { return default(long?); }
            set { }
        }

        /// <summary>Gets or sets the video bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        float? IVideo.BitRate {
            get { return default(float?); }
            set { }
        }

        /// <summary>Gets or sets the video bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        FrameOrBitRateMode IVideo.BitRateMode {
            get { return default(FrameOrBitRateMode); }
            set { }
        }

        /// <summary>Gets or sets the type of chroma subsampling.</summary>
        /// <value>The chroma subsampling.</value>
        string IVideo.ChromaSubsampling {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the compression mode of this video.</summary>
        /// <value>The compression mode of this video.</value>
        CompressionMode IVideo.CompressionMode {
            get { return default(CompressionMode); }
            set { }
        }

        string IVideo.Format {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the frames per second in this video.</summary>
        /// <value>The Frames per second.</value>
        float? IVideo.FPS {
            get { return default(float?); }
            set { }
        }

        string IVideo.MovieHash {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the video scan type.</summary>
        /// <value>The type video scan type.</value>
        ScanType IVideo.ScanType {
            get { return default(ScanType); }
            set { }
        }

        /// <summary>With or from what this video was made from</summary>
        /// <example>\eg{ <c>TS, TC, TELESYNC, CAM, HDRIP, DVDRIP, BDRIP, DTV, HD2DVD, HDDVDRIP, HDTVRIP, VHS, SCREENER, RECODE</c>}</example>
        string IVideo.Source {
            get { return default(string); }
            set { }
        }

        string IVideo.Standard {
            get { return default(string); }
            set { }
        }

        /// <summary>The type of the video</summary>
        /// <example>\eg{ <c>XVID, DVD5, DVD9, DVDR, BLUERAY, BD, HD2DVD, X264</c>}</example>
        string IVideo.Type {
            get { return default(string); }
            set { }
        }

        #endregion

        #endregion
    }

}