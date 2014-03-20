using System;
using System.Windows.Forms.VisualStyles;
using Frost.Common;
using Frost.Common.Models;
using Frost.DetectFeatures;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieAudio : MovieHasLanguageBase<IAudio> {

        /// <summary>Initializes a new instance of the <see cref="MovieAudio"/> class.</summary>
        /// <param name="audio">The audio.</param>
        public MovieAudio(IAudio audio) : base(audio) {
        }

        public override ILanguage Language {
            get { return _observedEntity.Language; }
            set {
                _observedEntity.Language = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the source of the audio</summary>
        /// <value>the source of the audio</value>
        /// <example>\eg{<c>LD MD LINE MIC</c>}</example>
        public string Source {
            get { return _observedEntity.Source; }
            set {
                _observedEntity.Source = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the type of the audio</summary>
        /// <summary>The type of the audio</summary>
        /// <example>\eg{<c>AC3 DTS</c>}</example>
        public string Type {
            get { return _observedEntity.Type; }
            set {
                _observedEntity.Type = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the channel setup.</summary>
        /// <value>The audio channels setting.</value>
        /// <example>\eg{ <c>Stereo, 2, 5.1, 6</c>}</example>
        public string ChannelSetup {
            get { return _observedEntity.ChannelSetup; }
            set {
                _observedEntity.ChannelSetup = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the number of chanells in the audio (5.1 has 6 chanels)</summary>
        /// <value>The number of chanells in the audio (5.1 has 6 chanels)</value>
        public int? NumberOfChannels {
            get { return _observedEntity.NumberOfChannels; }
            set {
                _observedEntity.NumberOfChannels = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio channel positions.</summary>
        /// <value>The audio channel positions.</value>
        /// <example>\eg{ <c>Front: L C R, Side: L R, LFE</c>}</example>
        public string ChannelPositions {
            get { return _observedEntity.ChannelPositions; }
            set {
                _observedEntity.ChannelPositions = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the codec this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        public string Codec {
            get { return _observedEntity.Codec; }
            set {
                _observedEntity.Codec = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the codec id this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MPAL3, aac_hd, dtshd</c>}</example>
        public string CodecId {
            get { return _observedEntity.CodecId; }
            set {
                _observedEntity.CodecId = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public float? BitRate {
            get { return _observedEntity.BitRate; }
            set {
                _observedEntity.BitRate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        /// <example>\eg{ ''<c>Constant</c>'' or ''<c>Variable</c>''}</example>
        public FrameOrBitRateMode BitRateMode {
            get { return _observedEntity.BitRateMode; }
            set {
                _observedEntity.BitRateMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio sampling rate.</summary>
        /// <value>The sampling rate in KHz.</value>
        public long? SamplingRate {
            get { return _observedEntity.SamplingRate; }
            set {
                _observedEntity.SamplingRate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio bit depth.</summary>
        /// <value>The audio depth in bits.</value>
        public long? BitDepth {
            get { return _observedEntity.BitDepth; }
            set {
                _observedEntity.BitDepth = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the compression mode of this audio.</summary>
        /// <value>The compression mode of this audio.</value>
        public CompressionMode CompressionMode {
            get { return _observedEntity.CompressionMode; }
            set {
                _observedEntity.CompressionMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio duration.</summary>
        /// <value>The audio duration in miliseconds.</value>
        public long? Duration {
            get { return _observedEntity.Duration; }
            set {
                _observedEntity.Duration = value;

                OnPropertyChanged("DurationTimeSpan");
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
                Duration = value != TimeSpan.Zero
                    ? Convert.ToInt64(value.TotalMilliseconds)
                    : (long?) null;
            }
        }

        /// <summary>Gets or sets the file this audio is contained in.</summary>
        /// <value>The file this audio is contained in.</value>
        public IFile File {
            get { return _observedEntity.File; }
            //set {
            //    _audio.File = value;
            //    OnPropertyChanged();
            //}
        }

        #region Images

        public string CodecImage {
            get {
                if (string.IsNullOrEmpty(CodecId)) {
                    return null;
                }

                string mapping;
                FileFeatures.AudioCodecIdMappings.TryGetValue(CodecId, out mapping);
                return GetImageSourceFromPath("Images/FlagsE/acodec_" + (mapping ?? CodecId) + ".png");
            }
        }

        public string AudioChannelsImage {
            get {
                if (!NumberOfChannels.HasValue) {
                    return null;
                }

                return GetImageSourceFromPath("Images/FlagsE/achan_" + NumberOfChannels + ".png");
            }
        }
        #endregion 
    }
}
