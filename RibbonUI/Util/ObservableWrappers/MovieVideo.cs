using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common;
using Frost.Common.Annotations;
using Frost.Common.Models;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieVideo : IVideo, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IVideo _video;

        public MovieVideo(IVideo video) {
            _video = video;
        }

        public ILanguage Language {
            get { return _video.Language; }
            set {
                _video.Language = value;
                OnPropertyChanged();
            }
        }

        public string MovieHash {
            get { return _video.MovieHash; }
            set {
                _video.MovieHash = value;
                OnPropertyChanged();
            }
        }

        /// <summary>With or from what this video was made from</summary>
        /// <example>\eg{ <c>TS, TC, TELESYNC, CAM, HDRIP, DVDRIP, BDRIP, DTV, HD2DVD, HDDVDRIP, HDTVRIP, VHS, SCREENER, RECODE</c>}</example>
        public string Source {
            get { return _video.Source; }
            set {
                _video.Source = value;
                OnPropertyChanged();
            }
        }

        /// <summary>The type of the video</summary>
        /// <example>\eg{ <c>XVID, DVD5, DVD9, DVDR, BLUERAY, BD, HD2DVD, X264</c>}</example>
        public string Type {
            get { return _video.Type; }
            set {
                _video.Type = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Resolution and format of the video</summary>
        /// <example>\eg{ <c>720p, 1080p, 720i, 1080i, PAL, HDTV, NTSC</c>}</example>
        public int? Resolution {
            get { return _video.Resolution; }
            set {
                _video.Resolution = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the name of the resolution.</summary>
        /// <value>The name of the resolution.</value>
        public string ResolutionName {
            get { return _video.ResolutionName; }
            set {
                _video.ResolutionName = value;
                OnPropertyChanged();
            }
        }

        public string Standard {
            get { return _video.Standard; }
            set {
                _video.Standard = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the frames per second in this video.</summary>
        /// <value>The Frames per second.</value>
        public float? FPS {
            get { return _video.FPS; }
            set {
                _video.FPS = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public float? BitRate {
            get { return _video.BitRate; }
            set {
                _video.BitRate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        public FrameOrBitRateMode BitRateMode {
            get { return _video.BitRateMode; }
            set {
                _video.BitRateMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video bit depth.</summary>
        /// <value>The video depth in bits.</value>
        public long? BitDepth {
            get { return _video.BitDepth; }
            set {
                _video.BitDepth = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the compression mode of this video.</summary>
        /// <value>The compression mode of this video.</value>
        public CompressionMode CompressionMode {
            get { return _video.CompressionMode; }
            set {
                _video.CompressionMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video duration in miliseconds.</summary>
        /// <value>The video duration in miliseconds.</value>
        public long? Duration {
            get { return _video.Duration; }
            set {
                _video.Duration = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the video scan type.</summary>
        /// <value>The type video scan type.</value>
        public ScanType ScanType {
            get { return _video.ScanType; }
            set {
                _video.ScanType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the color space.</summary>
        /// <value>The video color space.</value>
        /// <example>\eg{ <c>YUV, YDbDr, YPbPr, YCbCr, RGB, CYMK</c>}</example>
        public string ColorSpace {
            get { return _video.ColorSpace; }
            set {
                _video.ColorSpace = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the type of chroma subsampling.</summary>
        /// <value>The chroma subsampling.</value>
        public string ChromaSubsampling {
            get { return _video.ChromaSubsampling; }
            set {
                _video.ChromaSubsampling = value;
                OnPropertyChanged();
            }
        }

        public string Format {
            get { return _video.Format; }
            set {
                _video.Format = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the codec this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>WMV3 DIVX XVID H264 VP6 AVC</c>}</example>
        public string Codec {
            get { return _video.Codec; }
            set {
                _video.Codec = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the codec tag this video is encoded in.</summary>
        /// <value>The codec this video is encoded in.</value>
        /// <example>\eg{ <c>x265, div3, dx50, mpeg2v</c>}</example>
        public string CodecId {
            get { return _video.CodecId; }
            set {
                _video.CodecId = value;
                OnPropertyChanged();
            }
        }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <value>Aspect ratio of the video</value>
        /// <example>\eg{ <c>1.333</c>}</example>
        public double? Aspect {
            get { return _video.Aspect; }
            set {
                _video.Aspect = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the the commercial name of the aspect ratio.</summary>
        /// <value>The the commercial name of the aspect ratio.</value>
        public string AspectCommercialName {
            get { return _video.AspectCommercialName; }
            set {
                _video.AspectCommercialName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        public int? Width {
            get { return _video.Width; }
            set {
                _video.Width = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        public int? Height {
            get { return _video.Height; }
            set {
                _video.Height = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the file this video is contained in.</summary>
        /// <value>The file this video is contained in.</value>
        public IFile File {
            get { return _video.File; }
            set { _video.File = value; }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
