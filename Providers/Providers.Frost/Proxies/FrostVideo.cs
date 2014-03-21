using System;
using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Frost.DB.Files;
using Frost.Providers.Frost.Provider;

namespace Frost.Providers.Frost.Proxies {
    public class FrostVideo : ProxyBase<Video>, IVideo {

        public FrostVideo(Video video, FrostMoviesDataDataService service) : base(video, service) {
        }

        public long Id {
            get { return Entity.Id; }
        }

        public bool this[string propertyName] {
            get { throw new NotImplementedException(); }
        }

        public string MovieHash {
            get { return Entity.MovieHash; }
            set { Entity.MovieHash = value; }
        }

        /// <summary>With or from what this video was made from</summary>
        /// <example>\eg{ <c>TS, TC, TELESYNC, CAM, HDRIP, DVDRIP, BDRIP, DTV, HD2DVD, HDDVDRIP, HDTVRIP, VHS, SCREENER, RECODE</c>}</example>
        public string Source {
            get { return Entity.Source; }
            set { Entity.Source = value; }
        }

        /// <summary>The type of the video</summary>
        /// <example>\eg{ <c>XVID, DVD5, DVD9, DVDR, BLUERAY, BD, HD2DVD, X264</c>}</example>
        public string Type {
            get { return Entity.Type; }
            set { Entity.Type = value; }
        }

        /// <summary>Resolution and format of the video</summary>
        /// <example>\eg{ <c>720p, 1080p, 720i, 1080i, PAL, HDTV, NTSC</c>}</example>
        public int? Resolution {
            get { return Entity.Resolution; }
            set { Entity.Resolution = value; }
        }

        /// <summary>Gets or sets the name of the resolution.</summary>
        /// <value>The name of the resolution.</value>
        public string ResolutionName {
            get { return Entity.ResolutionName; }
            set { Entity.ResolutionName = value; }
        }

        public string Standard {
            get { return Entity.Standard; }
            set { Entity.Standard = value; }
        }

        /// <summary>Gets or sets the frames per second in this video.</summary>
        /// <value>The Frames per second.</value>
        public float? FPS {
            get { return Entity.FPS; }
            set { Entity.FPS = value; }
        }

        /// <summary>Gets or sets the video bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public float? BitRate {
            get { return Entity.BitRate; }
            set { Entity.BitRate = value; }
        }

        /// <summary>Gets or sets the video bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        public FrameOrBitRateMode BitRateMode {
            get { return Entity.BitRateMode; }
            set { Entity.BitRateMode = value; }
        }

        /// <summary>Gets or sets the video bit depth.</summary>
        /// <value>The video depth in bits.</value>
        public long? BitDepth {
            get { return Entity.BitDepth; }
            set { Entity.BitDepth = value; }
        }

        /// <summary>Gets or sets the compression mode of this video.</summary>
        /// <value>The compression mode of this video.</value>
        public CompressionMode CompressionMode {
            get { return Entity.CompressionMode; }
            set { Entity.CompressionMode = value; }
        }

        /// <summary>Gets or sets the video duration in miliseconds.</summary>
        /// <value>The video duration in miliseconds.</value>
        public long? Duration {
            get { return Entity.Duration; }
            set { Entity.Duration = value; }
        }

        /// <summary>Gets or sets the video scan type.</summary>
        /// <value>The type video scan type.</value>
        public ScanType ScanType {
            get { return Entity.ScanType; }
            set { Entity.ScanType = value; }
        }

        /// <summary>Gets or sets the color space.</summary>
        /// <value>The video color space.</value>
        /// <example>\eg{ <c>YUV, YDbDr, YPbPr, YCbCr, RGB, CYMK</c>}</example>
        public string ColorSpace {
            get { return Entity.ColorSpace; }
            set { Entity.ColorSpace = value; }
        }

        /// <summary>Gets or sets the type of chroma subsampling.</summary>
        /// <value>The chroma subsampling.</value>
        public string ChromaSubsampling {
            get { return Entity.ChromaSubsampling; }
            set { Entity.ChromaSubsampling = value; }
        }

        public string Format {
            get { return Entity.Format; }
            set { Entity.Format = value; }
        }

        /// <summary>Gets or sets the codec this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>WMV3 DIVX XVID H264 VP6 AVC</c>}</example>
        public string Codec {
            get { return Entity.Codec; }
            set { Entity.Codec = value; }
        }

        /// <summary>Gets or sets the codec tag this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>x265, div3, dx50, mpeg2v</c>}</example>
        public string CodecId {
            get { return Entity.CodecId; }
            set { Entity.CodecId = value; }
        }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <value>Aspect ratio of the video</value>
        /// <example>\eg{ <c>1.333</c>}</example>
        public double? Aspect {
            get { return Entity.Aspect; }
            set { Entity.Aspect = value; }
        }

        /// <summary>Gets or sets the the commercial name of the aspect ratio.</summary>
        /// <value>The the commercial name of the aspect ratio.</value>
        public string AspectCommercialName {
            get { return Entity.AspectCommercialName; }
            set { Entity.AspectCommercialName = value; }
        }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        public int? Width {
            get { return Entity.Width; }
            set { Entity.Width = value; }
        }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        public int? Height {
            get { return Entity.Height; }
            set { Entity.Height = value; }
        }

        /// <summary>Gets or sets the file this video is contained in.</summary>
        /// <value>The file this video is contained in.</value>
        public IFile File {
            get { return Entity.File; }
        }

        public ILanguage Language {
            get { return Entity.Language; }
            set { Entity.Language = Service.FindOrCreate(value, true); }
        }
    }
}
