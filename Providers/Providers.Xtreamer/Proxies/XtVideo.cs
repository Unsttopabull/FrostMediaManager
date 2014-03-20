using System;
using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtVideo : IVideo {
        private readonly XjbPhpMovie _movie;

        public XtVideo(XjbPhpMovie movie) {
            _movie = movie;
        }

        public long Id {
            get { return 0; }
        }

        public string MovieHash {
            get { return default(string); }
            set { }
        }

        /// <summary>With or from what this video was made from</summary>
        /// <example>\eg{ <c>TS, TC, TELESYNC, CAM, HDRIP, DVDRIP, BDRIP, DTV, HD2DVD, HDDVDRIP, HDTVRIP, VHS, SCREENER, RECODE</c>}</example>
        public string Source {
            get { return _movie.VideoSource; }
            set { _movie.VideoSource = value; }
        }

        /// <summary>The type of the video</summary>
        /// <example>\eg{ <c>XVID, DVD5, DVD9, DVDR, BLUERAY, BD, HD2DVD, X264</c>}</example>
        public string Type {
            get { return _movie.VideoType; }
            set { _movie.VideoType = value; }
        }

        /// <summary>Resolution and format of the video</summary>
        /// <example>\eg{ <c>720p, 1080p, 720i, 1080i, PAL, HDTV, NTSC</c>}</example>
        public int? Resolution {
            get {
                if (string.IsNullOrEmpty(_movie.VideoResolution)) {
                    return null;
                }

                int intRes;
                string res = _movie.VideoResolution.TrimEnd('p', 'i');
                if (int.TryParse(res, out intRes)) {
                    return intRes;
                }
                return null;
            }
            set {
            }
        }

        /// <summary>Gets or sets the name of the resolution.</summary>
        /// <value>The name of the resolution.</value>
        public string ResolutionName {
            get { return _movie.VideoResolution; }
            set { _movie.VideoResolution = value; } 
        }

        public string Standard {
            get { return null; }
            set{ }
        }

        /// <summary>Gets or sets the frames per second in this video.</summary>
        /// <value>The Frames per second.</value>
        public float? FPS {
            get { return _movie.FPS; }
            set {
                _movie.FPS = value.HasValue
                    ? (int?) Math.Round(value.Value)
                    : null;
            }
        }

        /// <summary>Gets or sets the video bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public float? BitRate {
            get { return null; }
            set { } 
        }

        /// <summary>Gets or sets the video bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        public FrameOrBitRateMode BitRateMode {
            get { return default(FrameOrBitRateMode); }
            set { }
        }

        /// <summary>Gets or sets the video bit depth.</summary>
        /// <value>The video depth in bits.</value>
        public long? BitDepth {
            get { return default(long?); }
            set { }
        }

        /// <summary>Gets or sets the compression mode of this video.</summary>
        /// <value>The compression mode of this video.</value>
        public CompressionMode CompressionMode {
            get { return default(CompressionMode); }
            set { }
        }

        /// <summary>Gets or sets the video duration in miliseconds.</summary>
        /// <value>The video duration in miliseconds.</value>
        public long? Duration {
            get { return null; }
            set{ }
        }

        /// <summary>Gets or sets the video scan type.</summary>
        /// <value>The type video scan type.</value>
        public ScanType ScanType {
            get { return default(ScanType); }
            set { }
        }

        /// <summary>Gets or sets the color space.</summary>
        /// <value>The video color space.</value>
        /// <example>\eg{ <c>YUV, YDbDr, YPbPr, YCbCr, RGB, CYMK</c>}</example>
        public string ColorSpace {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the type of chroma subsampling.</summary>
        /// <value>The chroma subsampling.</value>
        public string ChromaSubsampling {
            get { return null; }
            set { }
        }
        public string Format {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the codec this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>WMV3 DIVX XVID H264 VP6 AVC</c>}</example>
        public string Codec {
            get { return _movie.VideoCodec; }
            set { _movie.VideoCodec = value; }
        }

        /// <summary>Gets or sets the codec tag this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>x265, div3, dx50, mpeg2v</c>}</example>
        public string CodecId {
            get { return _movie.VideoCodec; }
            set { _movie.VideoCodec = value; }
        }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <value>Aspect ratio of the video</value>
        /// <example>\eg{ <c>1.333</c>}</example>
        public double? Aspect {
            get { return _movie.Aspect; }
            set { _movie.Aspect = value; }
        }

        /// <summary>Gets or sets the the commercial name of the aspect ratio.</summary>
        /// <value>The the commercial name of the aspect ratio.</value>
        public string AspectCommercialName {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        public int? Width {
            get { return _movie.Width; }
            set { _movie.Width = value; }
        }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        public int? Height {
            get { return _movie.Height; }
            set { _movie.Height = value; }
        }

        /// <summary>Gets or sets the file this video is contained in.</summary>
        /// <value>The file this video is contained in.</value>
        public IFile File {
            get { return null; }
        }

        public ILanguage Language {
            get { return null; }
            set { }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Height":
                    case "Width":
                    case "Aspect":
                    case "CodecId":
                    case "Codec":
                    case "FPS":
                    case "ResolutionName":
                    case "Resolution":
                    case "Type":
                    case "Source":
                        return true;
                    default:
                        return false;
                }
            }
        }
    }

}