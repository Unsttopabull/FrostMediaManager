using System;
using System.Collections.Generic;
using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;
using Frost.Providers.Xtreamer.Proxies.ChangeTrackers;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtVideo : ChangeTrackingProxy<XjbPhpMovie>, IVideo {
        private readonly string _xtPath;
        private IFile _file;

        public XtVideo(XjbPhpMovie movie, string xtPath) : base(movie) {
            _xtPath = xtPath;
            OriginalValues = new Dictionary<string, object> {
                {"Source", Entity.VideoSource},
                {"Type", Entity.VideoSource},
                {"Resolution", Entity.VideoSource},
                {"FPS", Entity.FPS},
                {"Aspect", Entity.Aspect},
                {"Width", Entity.Width},
                {"Height", Entity.Height}
            };
        }

        /// <summary>With or from what this video was made from</summary>
        /// <example>\eg{ <c>TS, TC, TELESYNC, CAM, HDRIP, DVDRIP, BDRIP, DTV, HD2DVD, HDDVDRIP, HDTVRIP, VHS, SCREENER, RECODE</c>}</example>
        public string Source {
            get { return Entity.VideoSource; }
            set {
                if (value == Entity.VideoSource) {
                    return;
                }

                Entity.VideoSource = value;
                TrackChanges(value);
            }
        }

        /// <summary>The type of the video</summary>
        /// <example>\eg{ <c>XVID, DVD5, DVD9, DVDR, BLUERAY, BD, HD2DVD, X264</c>}</example>
        public string Type {
            get { return Entity.VideoType; }
            set {
                Entity.VideoType = value;
                TrackChanges(value);
            }
        }

        /// <summary>Resolution and format of the video</summary>
        /// <example>\eg{ <c>720p, 1080p, 720i, 1080i, PAL, HDTV, NTSC</c>}</example>
        public int? Resolution {
            get {
                if (string.IsNullOrEmpty(Entity.VideoResolution)) {
                    return null;
                }

                int intRes;
                string res = Entity.VideoResolution.TrimEnd('p', 'i');
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
            get { return Entity.VideoResolution; }
            set {
                Entity.VideoResolution = value;
                TrackChanges(value);
            } 
        }

        /// <summary>Gets or sets the frames per second in this video.</summary>
        /// <value>The Frames per second.</value>
        public float? FPS {
            get { return Entity.FPS; }
            set {
                Entity.FPS = value.HasValue
                    ? (int?) Math.Round(value.Value)
                    : null;

                TrackChanges(Entity.FPS);
            }
        }

        /// <summary>Gets or sets the codec this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>WMV3 DIVX XVID H264 VP6 AVC</c>}</example>
        public string Codec {
            get { return Entity.VideoCodec; }
            set {
                //Entity.VideoCodec = value;
                //TrackChanges(value);
            }
        }

        /// <summary>Gets or sets the codec tag this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>x265, div3, dx50, mpeg2v</c>}</example>
        public string CodecId {
            get { return Entity.VideoCodec; }
            set {
                Entity.VideoCodec = value;
                TrackChanges(value);
            }
        }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <value>Aspect ratio of the video</value>
        /// <example>\eg{ <c>1.333</c>}</example>
        public double? Aspect {
            get { return Entity.Aspect; }
            set {
                Entity.Aspect = value;
                TrackChanges(value);
            }
        }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        public int? Width {
            get { return Entity.Width; }
            set {
                Entity.Width = value;
                TrackChanges(value);
            }
        }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        public int? Height {
            get { return Entity.Height; }
            set {
                Entity.Height = value;
                TrackChanges(value);
            }
        }

        #region Not Implemented

        public long Id {
            get { return 0; }
        }

        public string MovieHash {
            get { return default(string); }
            set { }
        }

        public string Standard {
            get { return null; }
            set { }
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
            set { }
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

        /// <summary>Gets or sets the the commercial name of the aspect ratio.</summary>
        /// <value>The the commercial name of the aspect ratio.</value>
        public string AspectCommercialName {
            get { return null; }
            set { }
        }

        #endregion

        #region Relations

        /// <summary>Gets or sets the file this video is contained in.</summary>
        /// <value>The file this video is contained in.</value>
        public IFile File {
            get { return _file ?? (_file = new XtFile(Entity, _xtPath)); }
        }

        public ILanguage Language {
            get { return null; }
            set { }
        }

        #endregion

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