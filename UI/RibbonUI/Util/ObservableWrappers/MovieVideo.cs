using System;
using Frost.Common;
using Frost.Common.Models;
using Frost.DetectFeatures;

namespace RibbonUI.Util.ObservableWrappers {

    public class MovieVideo : MovieHasLanguageBase<IVideo> {

        public MovieVideo(IVideo video) : base(video) {
        }

        #region Observed properties

        public override ILanguage Language {
            get { return _observedEntity.Language; }
            set {
                _observedEntity.Language = value;
                OnPropertyChanged();
                OnPropertyChanged("LanguageImage");
            }
        }

        public string MovieHash {
            get { return _observedEntity.MovieHash; }
            set {
                _observedEntity.MovieHash = value;
                OnPropertyChanged();
            }
        }

        /// <summary>With or from what this video was made from</summary>
        /// <example>\eg{ <c>TS, TC, TELESYNC, CAM, HDRIP, DVDRIP, BDRIP, DTV, HD2DVD, HDDVDRIP, HDTVRIP, VHS, SCREENER, RECODE</c>}</example>
        public string Source {
            get { return _observedEntity.Source; }
            set {
                _observedEntity.Source = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>The type of the video</summary>
        /// <example>\eg{ <c>XVID, DVD5, DVD9, DVDR, BLUERAY, BD, HD2DVD, X264</c>}</example>
        public string Type {
            get { return _observedEntity.Type; }
            set {
                _observedEntity.Type = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Resolution and format of the video</summary>
        /// <example>\eg{ <c>720p, 1080p, 720i, 1080i, PAL, HDTV, NTSC</c>}</example>
        public int? Resolution {
            get { return _observedEntity.Resolution; }
            set {
                _observedEntity.Resolution = value;

                OnPropertyChanged("ResolutionImage");
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the name of the resolution.</summary>
        /// <value>The name of the resolution.</value>
        public string ResolutionName {
            get { return _observedEntity.ResolutionName; }
            set {
                _observedEntity.ResolutionName = value;

                OnPropertyChanged("ResolutionImage");
                OnPropertyChanged();
            }
        }

        public string Standard {
            get { return _observedEntity.Standard; }
            set {
                _observedEntity.Standard = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the frames per second in this video.</summary>
        /// <value>The Frames per second.</value>
        public float? FPS {
            get { return _observedEntity.FPS; }
            set {
                _observedEntity.FPS = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public float? BitRate {
            get { return _observedEntity.BitRate; }
            set {
                _observedEntity.BitRate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        public FrameOrBitRateMode BitRateMode {
            get { return _observedEntity.BitRateMode; }
            set {
                _observedEntity.BitRateMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video bit depth.</summary>
        /// <value>The video depth in bits.</value>
        public long? BitDepth {
            get { return _observedEntity.BitDepth; }
            set {
                _observedEntity.BitDepth = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the compression mode of this video.</summary>
        /// <value>The compression mode of this video.</value>
        public CompressionMode CompressionMode {
            get { return _observedEntity.CompressionMode; }
            set {
                _observedEntity.CompressionMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video duration in miliseconds.</summary>
        /// <value>The video duration in miliseconds.</value>
        public long? Duration {
            get { return _observedEntity.Duration; }
            set {
                _observedEntity.Duration = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan DurationTimeSpan {
            get {
                return Duration.HasValue
                    ? TimeSpan.FromMilliseconds((double) Duration)
                    : new TimeSpan();
            }
            set {
                Duration = Convert.ToInt64(value.TotalMilliseconds);
            }
        }

        /// <summary>Gets or sets the video scan type.</summary>
        /// <value>The type video scan type.</value>
        public ScanType ScanType {
            get { return _observedEntity.ScanType; }
            set {
                _observedEntity.ScanType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the color space.</summary>
        /// <value>The video color space.</value>
        /// <example>\eg{ <c>YUV, YDbDr, YPbPr, YCbCr, RGB, CYMK</c>}</example>
        public string ColorSpace {
            get { return _observedEntity.ColorSpace; }
            set {
                _observedEntity.ColorSpace = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the type of chroma subsampling.</summary>
        /// <value>The chroma subsampling.</value>
        public string ChromaSubsampling {
            get { return _observedEntity.ChromaSubsampling; }
            set {
                _observedEntity.ChromaSubsampling = value;
                OnPropertyChanged();
            }
        }

        public string Format {
            get { return _observedEntity.Format; }
            set {
                _observedEntity.Format = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the codec this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>WMV3 DIVX XVID H264 VP6 AVC</c>}</example>
        public string Codec {
            get { return _observedEntity.Codec; }
            set {
                _observedEntity.Codec = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the codec tag this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>x265, div3, dx50, mpeg2v</c>}</example>
        public string CodecId {
            get { return _observedEntity.CodecId; }
            set {
                _observedEntity.CodecId = value;
                OnPropertyChanged("CodecImage");
                OnPropertyChanged();
            }
        }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <value>Aspect ratio of the video</value>
        /// <example>\eg{ <c>1.333</c>}</example>
        public double? Aspect {
            get { return _observedEntity.Aspect; }
            set {
                _observedEntity.Aspect = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the the commercial name of the aspect ratio.</summary>
        /// <value>The the commercial name of the aspect ratio.</value>
        public string AspectCommercialName {
            get { return _observedEntity.AspectCommercialName; }
            set {
                _observedEntity.AspectCommercialName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        public int? Width {
            get { return _observedEntity.Width; }
            set {
                _observedEntity.Width = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        public int? Height {
            get { return _observedEntity.Height; }
            set {
                _observedEntity.Height = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the file this video is contained in.</summary>
        /// <value>The file this video is contained in.</value>
        public IFile File {
            get { return _observedEntity.File; }
            //set { _video.File = value; }
        }

        #endregion

        #region Images

        public string CodecImage {
            get {
                string mapping;
                FileFeatures.VideoCodecIdMappings.TryGetValue(CodecId, out mapping);
                return GetImageSourceFromPath("Images/FlagsE/vcodec_" + (mapping ?? CodecId) + ".png");
            }
        }

        public string ResolutionImage {
            get {
                string filePath;
                if (!string.IsNullOrEmpty(ResolutionName)) {
                    filePath = "Images/FlagsE/vres_" + ResolutionName + ".png";
                    return GetImageSourceFromPath(filePath);
                }

                if (!Resolution.HasValue) {
                    return null;
                }

                int res = Resolution.Value;
                switch (ScanType) {
                    case ScanType.Interlaced:
                        filePath = "Images/FlagsE/vres_" + res + "i.png";
                        break;
                    case ScanType.Progressive:
                        filePath = "Images/FlagsE/vres_" + res + "p.png";
                        break;
                    default:
                        filePath = "Images/FlagsE/vres_" + res + ".png";
                        break;
                }
                return GetImageSourceFromPath(filePath);
            }
        }


        #endregion
    }

}