using System;
using Frost.Common;
using Frost.Common.Models.Provider;

namespace RibbonUI.Design.Models {
    class DesignVideo : IVideo {
        public ILanguage Language { get; set; }
        public long Id { get; private set; }

        public bool this[string propertyName] {
            get { throw new NotImplementedException(); }
        }

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

        /// <summary>Gets or sets the file this video is contained in.</summary>
        /// <value>The file this video is contained in.</value>
        public IFile File { get; private set; }
    }
}
