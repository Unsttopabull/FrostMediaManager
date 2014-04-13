using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.Common.Proxies;
using Frost.Providers.Xbmc.DB.StreamDetails;

namespace Frost.Providers.Xbmc.Proxies {

    /// <summary>Represents information about a video stream in a file.</summary>
    public class XbmcVideoDetails : Proxy<XbmcDbStreamDetails>, IVideo {

        public XbmcVideoDetails(XbmcDbStreamDetails stream) : base(stream){
            
        }

        /// <summary>Unique identifier.</summary>
        public long Id { get { return Entity.Id; } }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <value>Aspect ratio of the video</value>
        /// <example>\eg{ <c>1.333</c>}</example>
        public double? Aspect {
            get { return Entity.Aspect; }
            set { Entity.Aspect = value; }
        }

        /// <summary>Gets or sets the codec this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>WMV3 DIVX XVID H264 VP6 AVC</c>}</example>
        public string Codec {
            get { return Entity.VideoCodec; }
            set{ }
        }

        public bool this[string propertyName] {
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

        public long? Duration {
            get { return Entity.VideoDuration * 1000; }
            set {
                if (value.HasValue) {
                    Entity.VideoDuration = value / 1000;
                }
                Entity.VideoDuration = null;
            }
        }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        public int? Width {
            get { return (int?) Entity.VideoWidth; }
            set { Entity.VideoWidth = value; }
        }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        public int? Height {
            get { return (int?) Entity.VideoHeight; }
            set { Entity.VideoHeight = value; }
        }

        /// <summary>Gets or sets the codec tag this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>x265, div3, dx50, mpeg2v</c>}</example>
        public string CodecId {
            get { return Entity.VideoCodec; }
            set { Entity.VideoCodec = value; }
        }

        /// <summary>Gets or sets the file this video is contained in.</summary>
        /// <value>The file this video is contained in.</value>
        public IFile File {
            get { return Entity.File; }
        }

        /// <summary>Resolution and format of the video</summary>
        /// <example>\eg{ <c>720p, 1080p, 720i, 1080i, PAL, HDTV, NTSC</c>}</example>
        public int? Resolution {
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

        /// <summary>Gets or sets the color space.</summary>
        /// <value>The video color space.</value>
        /// <example>\eg{ <c>YUV, YDbDr, YPbPr, YCbCr, RGB, CYMK</c>}</example>
        public string ColorSpace {
            get { return default(string); }
            set { }
        }

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
    }

}